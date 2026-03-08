using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class VenipunctureTrainer_MaskBased : MonoBehaviour
{
    [Header("References")]
    public Camera arCamera;
    public TMP_Text feedbackText;
    public MeshCollider armMeshCollider;
    public Texture2D veinMask;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip correctClip;
    public AudioClip wrongClip;

    [Header("Raycast")]
    public float raycastDistance = 5000f;

    [Header("Mask Colors")]
    public Color32 correctColor = new Color32(0, 255, 0, 255);   // green
    public Color32 wrong1Color = new Color32(255, 0, 0, 255);   // red
    public Color32 wrong2Color = new Color32(0, 0, 255, 255);   // blue

    [Header("Color Match Tolerance")]
    [Range(0, 50)]
    public int tolerance = 20;

    private bool completed;

    void Awake()
    {
        if (arCamera == null) arCamera = Camera.main;
        ResetTrainer();
        Debug.Log("[MaskTrainer] Awake OK");
    }

    void Update()
    {
        if (completed) return;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
            CheckTap(Input.mousePosition);
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            CheckTap(Input.GetTouch(0).position);
#endif
    }

    void CheckTap(Vector2 screenPos)
    {
        if (EventSystem.current != null)
        {
#if UNITY_EDITOR
            if (EventSystem.current.IsPointerOverGameObject()) return;
#else
            if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;
#endif
        }

        if (arCamera == null || armMeshCollider == null || veinMask == null) return;

        Ray ray = arCamera.ScreenPointToRay(screenPos);

        if (!Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
        {
            Debug.Log("[MaskTrainer] Raycast hit nothing");
            return;
        }

        if (hit.collider != armMeshCollider)
        {
            Debug.Log("[MaskTrainer] Hit something else: " + hit.collider.name);
            return;
        }

        Vector2 uv = hit.textureCoord;
        Color32 px = veinMask.GetPixelBilinear(uv.x, uv.y);

        Debug.Log($"[MaskTrainer] UV={uv} Pixel=({px.r},{px.g},{px.b},{px.a})");

        if (ColorsMatch(px, correctColor))
        {
            completed = true;
            SetFeedback("Correct! Median Cubital Vein", Color.green);
            Play(correctClip);
        }
        else if (ColorsMatch(px, wrong1Color))
        {
            SetFeedback("Wrong. Cephalic vein. Try again.", Color.red);
            Play(wrongClip);
        }
        else if (ColorsMatch(px, wrong2Color))
        {
            SetFeedback("Wrong. Basilic vein. Try again.", Color.red);
            Play(wrongClip);
        }
        else
        {
            SetFeedback("Not a marked vein area. Try again.", Color.red);
            Play(wrongClip);
        }
    }

    bool ColorsMatch(Color32 a, Color32 b)
    {
        return Mathf.Abs(a.r - b.r) <= tolerance &&
               Mathf.Abs(a.g - b.g) <= tolerance &&
               Mathf.Abs(a.b - b.b) <= tolerance;
    }

    void SetFeedback(string msg, Color c)
    {
        if (feedbackText == null) return;
        feedbackText.text = msg;
        feedbackText.color = c;
    }

    void Play(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }

    public void ResetTrainer()
    {
        completed = false;
        SetFeedback("Tap the best vein site on the arm.", Color.black);
    }
}