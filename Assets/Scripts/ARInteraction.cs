using UnityEngine;
using TMPro;

public class ARInteraction : MonoBehaviour
{
    [Header("Camera")]
    public Camera arCamera;

    [Header("UI")]
    public TMP_Text selectionText;

    [Header("Heart Touch Manager")]
    public HeartTouchManager heartTouchManager; // Drag HeartTouchManager GameObject here

    [Header("Raycast")]
    public float raycastDistance = 5000f;

    private Camera cam;

    void Awake()
    {
        cam = arCamera != null ? arCamera : Camera.main;
        UpdateSelectionUI(null);

        // Auto-find if not assigned
        if (heartTouchManager == null)
            heartTouchManager = FindObjectOfType<HeartTouchManager>();
    }

    void Update()
    {
        if (cam == null) return;

#if UNITY_EDITOR
        HandleMouse();
#else
        HandleTouch();
#endif
    }

    void HandleTouch()
    {
        if (Input.touchCount == 0) return;
        Touch t = Input.GetTouch(0);
        if (t.phase == TouchPhase.Began)
            TryTap(t.position);
    }

    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
            TryTap(Input.mousePosition);
    }

    void TryTap(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
        {
            // Check if the tapped object belongs to the heart
            var hitManager = hit.collider.GetComponentInParent<HeartTouchManager>();

            if (hitManager != null)
            {
                // HeartTouchManager handles its own tap logic - no need to call anything here
                UpdateSelectionUI("Heart");
            }
            else
            {
                // Tapped something else - close the heart info panel
                if (heartTouchManager != null)
                    heartTouchManager.ClosePanel();

                UpdateSelectionUI(hit.collider.gameObject.name);
            }
        }
        else
        {
            // Tapped empty space
            if (heartTouchManager != null)
                heartTouchManager.ClosePanel();

            UpdateSelectionUI(null);
        }
    }

    void UpdateSelectionUI(string hitName)
    {
        if (selectionText == null) return;

        if (string.IsNullOrEmpty(hitName))
            selectionText.text = "Tap the Heart";
        else
            selectionText.text = "Tapped: " + hitName;
    }
}