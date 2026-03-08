using UnityEngine;
using UnityEngine.EventSystems;

public class ARModelManipulator : MonoBehaviour
{
    [Header("Camera (recommended)")]
    public Camera arCamera; // drag ARCamera here

    [Header("Rotate")]
    public float rotateSpeed = 0.25f;
    public float rotateDeadZonePixels = 2f;

    [Header("Pinch Zoom")]
    public float pinchSpeed = 0.005f;
    public float minScale = 0.3f;
    public float maxScale = 3.0f;

    [Header("Two-finger Move (Pan)")]
    public float panSpeed = 0.0015f;
    public float maxPanDistance = 0.25f;

    private Vector3 baseScale;
    private float scaleMultiplier = 1f;
    private Vector3 startPosition;

    void Start()
    {
        if (arCamera == null) arCamera = Camera.main;

        baseScale = transform.localScale;
        startPosition = transform.position;

        ApplyScale();
    }

    void Update()
    {
#if UNITY_EDITOR
        HandleMouse();
#else
        HandleTouch();
#endif
    }

    bool TouchIsOverUI(int fingerId)
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(fingerId);
    }

    bool MouseIsOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    void HandleTouch()
    {
        // 1 finger = rotate
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            if (TouchIsOverUI(t.fingerId)) return;

            if (t.phase == TouchPhase.Moved && t.deltaPosition.magnitude > rotateDeadZonePixels)
            {
                float rotY = -t.deltaPosition.x * rotateSpeed;
                float rotX = t.deltaPosition.y * rotateSpeed;

                transform.Rotate(Vector3.up, rotY, Space.World);
                transform.Rotate(Vector3.right, rotX, Space.World);
            }
        }
        // 2 fingers = pinch zoom + pan
        else if (Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            if (TouchIsOverUI(t1.fingerId) || TouchIsOverUI(t2.fingerId)) return;

            // pinch
            Vector2 t1Prev = t1.position - t1.deltaPosition;
            Vector2 t2Prev = t2.position - t2.deltaPosition;

            float prevDist = Vector2.Distance(t1Prev, t2Prev);
            float currDist = Vector2.Distance(t1.position, t2.position);
            float delta = currDist - prevDist;

            scaleMultiplier += delta * pinchSpeed;
            scaleMultiplier = Mathf.Clamp(scaleMultiplier, minScale, maxScale);
            ApplyScale();

            // pan
            Vector2 avgDelta = (t1.deltaPosition + t2.deltaPosition) * 0.5f;
            Vector3 move = new Vector3(avgDelta.x, avgDelta.y, 0f) * panSpeed;

            Camera cam = arCamera != null ? arCamera : Camera.main;
            if (cam != null)
                transform.position += cam.transform.right * move.x + cam.transform.up * move.y;
            else
                transform.position += new Vector3(move.x, move.y, 0);

            ClampPanDistance();
        }
    }

    void HandleMouse()
    {
        if (MouseIsOverUI()) return;

        if (Input.GetMouseButton(0))
        {
            float dx = Input.GetAxis("Mouse X");
            float dy = Input.GetAxis("Mouse Y");

            if (Mathf.Abs(dx) + Mathf.Abs(dy) > 0.001f)
            {
                float rotY = -dx * rotateSpeed * 200f;
                float rotX = dy * rotateSpeed * 200f;

                transform.Rotate(Vector3.up, rotY, Space.World);
                transform.Rotate(Vector3.right, rotX, Space.World);
            }
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            scaleMultiplier += scroll * 0.5f;
            scaleMultiplier = Mathf.Clamp(scaleMultiplier, minScale, maxScale);
            ApplyScale();
        }
    }

    void ApplyScale()
    {
        transform.localScale = baseScale * scaleMultiplier;
    }

    void ClampPanDistance()
    {
        Vector3 offset = transform.position - startPosition;
        if (offset.magnitude > maxPanDistance)
            transform.position = startPosition + offset.normalized * maxPanDistance;
    }

    public void ResetTransform()
    {
        scaleMultiplier = 1f;
        transform.position = startPosition;
        transform.localScale = baseScale;
    }
}