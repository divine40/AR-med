using UnityEngine;

public class HeartTouchController : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationSpeed = 0.2f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    [Header("Zoom")]
    public float pinchZoomSpeed = 0.005f;
    public float scrollZoomSpeed = 0.2f;
    public float minMultiplier = 0.3f;
    public float maxMultiplier = 2.0f;

    private Vector3 initialScale;
    private float scaleMultiplier = 1f;

    private float yaw;
    private float pitch;

    private void Start()
    {
        initialScale = transform.localScale;

        Vector3 startEuler = transform.localEulerAngles;
        yaw = startEuler.y;
        pitch = startEuler.x;
    }

    private void Update()
    {
#if UNITY_EDITOR
        HandleMouse();
#else
        HandleTouch();
#endif

        ApplyRotation();
        ApplyScale();
    }

    private void HandleMouse()
    {
        if (Input.GetMouseButton(0))
        {
            float deltaX = Input.GetAxis("Mouse X");
            float deltaY = Input.GetAxis("Mouse Y");

            yaw -= deltaX * rotationSpeed * 200f;
            pitch += deltaY * rotationSpeed * 200f;
            pitch = ClampAngle(pitch, minPitch, maxPitch);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.0001f)
        {
            scaleMultiplier += scroll * scrollZoomSpeed;
            scaleMultiplier = Mathf.Clamp(scaleMultiplier, minMultiplier, maxMultiplier);
        }
    }

    private void HandleTouch()
    {
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Moved)
            {
                yaw -= t.deltaPosition.x * rotationSpeed;
                pitch += t.deltaPosition.y * rotationSpeed;
                pitch = ClampAngle(pitch, minPitch, maxPitch);
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            Vector2 t1Prev = t1.position - t1.deltaPosition;
            Vector2 t2Prev = t2.position - t2.deltaPosition;

            float prevDist = Vector2.Distance(t1Prev, t2Prev);
            float currDist = Vector2.Distance(t1.position, t2.position);

            float delta = currDist - prevDist;

            scaleMultiplier += delta * pinchZoomSpeed;
            scaleMultiplier = Mathf.Clamp(scaleMultiplier, minMultiplier, maxMultiplier);
        }
    }

    private void ApplyRotation()
    {
        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void ApplyScale()
    {
        transform.localScale = initialScale * scaleMultiplier;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle > 180f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}