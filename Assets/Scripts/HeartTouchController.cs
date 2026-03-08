using UnityEngine;

public class HeartTouchController : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationSpeed = 0.2f;

    [Header("Zoom")]
    public float pinchZoomSpeed = 1.0f;
    public float scrollZoomSpeed = 0.2f;
    public float minMultiplier = 0.3f;
    public float maxMultiplier = 2.0f;

    private Vector3 initialScale;
    private float scaleMultiplier = 1f;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
#if UNITY_EDITOR
        HandleMouse();
#else
        HandleTouch();
#endif

        ApplyScale();
    }

    void HandleMouse()
    {
        if (Input.GetMouseButton(0))
        {
            float rotX = Input.GetAxis("Mouse X") * rotationSpeed * 200f;
            float rotY = Input.GetAxis("Mouse Y") * rotationSpeed * 200f;

            transform.Rotate(Vector3.up, -rotX, Space.World);
            transform.Rotate(Vector3.right, rotY, Space.World);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            scaleMultiplier *= (1f + scroll * scrollZoomSpeed);
            scaleMultiplier = Mathf.Clamp(scaleMultiplier, minMultiplier, maxMultiplier);
        }
    }

    void HandleTouch()
    {
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Moved)
            {
                float rotX = t.deltaPosition.x * rotationSpeed;
                float rotY = t.deltaPosition.y * rotationSpeed;

                transform.Rotate(Vector3.up, -rotX, Space.World);
                transform.Rotate(Vector3.right, rotY, Space.World);
            }
        }

        if (Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            Vector2 t1Prev = t1.position - t1.deltaPosition;
            Vector2 t2Prev = t2.position - t2.deltaPosition;

            float prevDist = Vector2.Distance(t1Prev, t2Prev);
            float currDist = Vector2.Distance(t1.position, t2.position);

            if (prevDist > 0.0001f)
            {
                float ratio = currDist / prevDist;
                float zoomFactor = Mathf.Lerp(1f, ratio, pinchZoomSpeed);

                scaleMultiplier *= zoomFactor;
                scaleMultiplier = Mathf.Clamp(scaleMultiplier, minMultiplier, maxMultiplier);
            }
        }
    }

    void ApplyScale()
    {
        transform.localScale = initialScale * scaleMultiplier;
    }
}