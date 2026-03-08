using UnityEngine;
using TMPro;

public class FractureModeManager : MonoBehaviour
{
    [Header("References")]
    public Transform skeletonRoot;     // SkeletonRoot
    public Camera arCamera;
    public TMP_Text infoText;

    [Header("Colors")]
    public Color fractureColor = Color.red;
    public Color correctColor = Color.green;
    public Color normalTextColor = Color.black;
    public Color alertTextColor = Color.red;

    [Header("Raycast")]
    public float raycastDistance = 5000f;

    [Header("State")]
    public bool fractureMode = false;

    BoneInfo[] bones;
    BoneInfo fracturedBone;

    void Awake()
    {
        if (arCamera == null) arCamera = Camera.main;

        if (skeletonRoot != null)
            bones = skeletonRoot.GetComponentsInChildren<BoneInfo>(true);
        else
            bones = FindObjectsOfType<BoneInfo>();

        SetText("Tap 'Fracture Mode' to start", normalTextColor);
    }

    void Update()
    {
        if (!fractureMode) return;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0)) CheckTap(Input.mousePosition);
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            CheckTap(Input.GetTouch(0).position);
#endif
    }

    public void ToggleFractureMode()
    {
        fractureMode = !fractureMode;

        // clear old tints
        if (bones != null)
            foreach (var b in bones) b.RestoreTint();

        fracturedBone = null;

        if (fractureMode)
        {
            PickRandomFracture();
        }
        else
        {
            SetText("Fracture Mode OFF. Tap a bone to learn.", normalTextColor);
        }
    }

    void PickRandomFracture()
    {
        if (bones == null || bones.Length == 0)
        {
            SetText("No bones found under SkeletonRoot.", alertTextColor);
            fractureMode = false;
            return;
        }

        fracturedBone = bones[Random.Range(0, bones.Length)];
        fracturedBone.SetTint(fractureColor);

        SetText("Fracture Mode: Find the fractured bone (RED) and tap it!", alertTextColor);
    }

    void CheckTap(Vector2 screenPos)
    {
        Ray ray = arCamera.ScreenPointToRay(screenPos);
        if (!Physics.Raycast(ray, out RaycastHit hit, raycastDistance)) return;

        BoneInfo tapped = hit.collider.GetComponentInParent<BoneInfo>();
        if (tapped == null) return;

        if (tapped == fracturedBone)
        {
            tapped.SetTint(correctColor);
            SetText($"Correct! {tapped.boneName} was fractured", correctColor);

            // optional: auto-stop fracture mode after correct
            fractureMode = false;
        }
        else
        {
            tapped.Flash();
            SetText($"Not {tapped.boneName}. Try again.", alertTextColor);
        }
    }

    void SetText(string msg, Color c)
    {
        if (infoText == null) return;
        infoText.text = msg;
        infoText.color = c;
    }
}