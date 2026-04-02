using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class VenipunctureTrainer_MaskBased : MonoBehaviour
{
    [Header("References")]
    public Camera arCamera;
    public TMP_Text feedbackText;
    public TMP_Text instructionText;

    [Header("Colliders")]
    public Collider armCollider;      // Arm_Real collider
    public Collider correctZone;      // Quad collider

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip correctClip;
    public AudioClip wrongClip;

    [Header("Raycast")]
    public float raycastDistance = 5000f;

    private bool completed = false;
    private bool firstTapDone = false;

    void Awake()
    {
        if (arCamera == null) arCamera = Camera.main;
        ResetTrainer();
        Debug.Log("[ZoneTrainer] Awake OK");
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

        if (arCamera == null || armCollider == null || correctZone == null) return;

        Ray ray = arCamera.ScreenPointToRay(screenPos);

        if (!Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
        {
            Debug.Log("[ZoneTrainer] Raycast hit nothing");
            return;
        }

        bool tappedArm = hit.collider == armCollider || hit.collider.transform.IsChildOf(armCollider.transform);
        bool tappedCorrect = hit.collider == correctZone || hit.collider.transform.IsChildOf(correctZone.transform);

        if (tappedCorrect)
        {
            CorrectTap();
            return;
        }

        if (tappedArm)
        {
            WrongTap();
            return;
        }

        Debug.Log("[ZoneTrainer] Hit something else: " + hit.collider.name);
    }

    public void CorrectTap()
    {
        if (completed) return;

        HideInstructionIfNeeded();

        completed = true;
        SetFeedback("Correct! Median Cubital Vein", Color.green);
        Play(correctClip);
        Debug.Log("[ZoneTrainer] Correct zone tapped");
    }

    public void WrongTap()
    {
        if (completed) return;

        HideInstructionIfNeeded();

        SetFeedback("Not a marked vein area", Color.red);
        Play(wrongClip);
        Debug.Log("[ZoneTrainer] Wrong arm area tapped");
    }

    void HideInstructionIfNeeded()
    {
        if (!firstTapDone)
        {
            firstTapDone = true;

            if (instructionText != null)
                instructionText.gameObject.SetActive(false);
        }
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
        firstTapDone = false;

        if (feedbackText != null)
        {
            feedbackText.text = "";
            feedbackText.color = Color.black;
        }

        if (instructionText != null)
        {
            instructionText.text = "Tap the best vein site on the arm.";
            instructionText.gameObject.SetActive(true);
        }
    }
}