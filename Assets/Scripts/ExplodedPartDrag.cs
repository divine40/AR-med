using UnityEngine;

public class ExplodedPartDrag : MonoBehaviour
{
    [Header("Refs")]
    public Camera arCamera;
    public ExplodeModel explodeModel; // drag your ExplodeModel here (or auto-find)

    [Header("Raycast")]
    public float raycastDistance = 5000f;

    [Header("Drag")]
    public float dragDepthFromCamera = 0.6f; // how far in front of camera the drag plane is

    private Transform selectedPart;
    private Plane dragPlane;
    private Vector3 dragOffsetWorld;

    void Awake()
    {
        if (arCamera == null) arCamera = Camera.main;
        if (explodeModel == null) explodeModel = GetComponent<ExplodeModel>();
    }

    void Update()
    {
        // Only allow dragging AFTER explode
        if (explodeModel == null || !explodeModel.IsExploded) return;
        if (arCamera == null) return;

#if UNITY_EDITOR
        HandleMouse();
#else
        HandleTouch();
#endif
    }

    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0)) BeginDrag(Input.mousePosition);
        if (Input.GetMouseButton(0)) DragTo(Input.mousePosition);
        if (Input.GetMouseButtonUp(0)) EndDrag();
    }

    void HandleTouch()
    {
        if (Input.touchCount == 0) return;

        Touch t = Input.GetTouch(0);
        if (t.phase == TouchPhase.Began) BeginDrag(t.position);
        if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary) DragTo(t.position);
        if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) EndDrag();
    }

    void BeginDrag(Vector2 screenPos)
    {
        // IMPORTANT: don’t start dragging if user tapped UI
        // If you already use EventSystem, this prevents dragging through buttons.
        if (UnityEngine.EventSystems.EventSystem.current != null &&
            UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = arCamera.ScreenPointToRay(screenPos);

        if (!Physics.Raycast(ray, out RaycastHit hit, raycastDistance)) return;

        // We want the individual piece (child). Prefer the collider’s transform.
        Transform hitT = hit.collider.transform;

        // Ensure it's actually part of THIS root (so you don’t drag random objects)
        if (!hitT.IsChildOf(transform)) return;

        selectedPart = hitT;

        // Plane in front of camera through the part (better than using world Z)
        Vector3 planePoint = selectedPart.position;
        dragPlane = new Plane(-arCamera.transform.forward, planePoint);

        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            dragOffsetWorld = selectedPart.position - hitPoint;
        }
        else
        {
            dragOffsetWorld = Vector3.zero;
        }
    }

    void DragTo(Vector2 screenPos)
    {
        if (selectedPart == null) return;

        Ray ray = arCamera.ScreenPointToRay(screenPos);

        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 targetWorld = hitPoint + dragOffsetWorld;

            selectedPart.position = targetWorld;
        }
    }

    void EndDrag()
    {
        selectedPart = null;
    }
}