using UnityEngine;
using TMPro;

public class SkeletonTapController : MonoBehaviour
{
    [Header("Camera")]
    public Camera arCamera;

    [Header("UI")]
    public TMP_Text infoText;      // your existing canvas text
    public Color normalTextColor = Color.black;

    [Header("Raycast")]
    public float raycastDistance = 5000f;

    void Awake()
    {
        if (arCamera == null) arCamera = Camera.main;

        if (infoText != null)
        {
            infoText.text = "Tap a bone";
            infoText.color = normalTextColor;
        }
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0)) TryTap(Input.mousePosition);
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            TryTap(Input.GetTouch(0).position);
#endif
    }

    void TryTap(Vector2 screenPos)
    {
        if (arCamera == null) return;

        Ray ray = arCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
        {
            BoneInfo bone = hit.collider.GetComponentInParent<BoneInfo>();
            if (bone != null)
            {
                bone.Flash();

                if (infoText != null)
                {
                    infoText.color = normalTextColor;
                    infoText.text = $"{bone.boneName}\n{bone.functionText}";
                }
            }
        }
    }
}