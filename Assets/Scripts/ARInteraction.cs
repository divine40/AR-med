using UnityEngine;
using TMPro;

public class ARInteraction : MonoBehaviour
{
    [Header("Camera")]
    public Camera arCamera; // Drag ARCamera's Camera here (recommended)

    [Header("UI")]
    public TMP_Text selectionText; // optional: can be the same as InfoText if you want

    [Header("Info Controller")]
    public HeartInfoController heartInfoController; // Drag Heart root here (the one with HeartInfoController)

    [Header("Raycast")]
    public float raycastDistance = 5000f;

    private Camera cam;

    void Awake()
    {
        cam = arCamera != null ? arCamera : Camera.main;
        UpdateSelectionUI(null);

        // If not assigned, try auto-find ONCE (not every tap)
        if (heartInfoController == null)
            heartInfoController = FindObjectOfType<HeartInfoController>();
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
            var hitHeartInfo = hit.collider.GetComponentInParent<HeartInfoController>();

            if (hitHeartInfo != null)
            {
                hitHeartInfo.ShowInfo();
                UpdateSelectionUI("Heart");
            }
            else
            {
                // Tapped something else -> hide the heart info
                if (heartInfoController != null)
                    heartInfoController.HideInfo();

                UpdateSelectionUI(hit.collider.gameObject.name);
            }
        }
        else
        {
            // Tapped empty space -> hide info + reset prompt
            if (heartInfoController != null)
                heartInfoController.HideInfo();

            UpdateSelectionUI(null);
        }
    }

    void UpdateSelectionUI(string hitName)
    {
        if (selectionText == null) return;

        if (string.IsNullOrEmpty(hitName))
            selectionText.text = "Tap the Arm";
        else
            selectionText.text = "Tapped: " + hitName;
    }
}