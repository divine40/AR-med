using UnityEngine;
using TMPro;

public class HeartTouchManager : MonoBehaviour
{
    [Header("Camera")]
    public Camera arCamera;

    [Header("UI Panel")]
    public GameObject infoPanel;
    public TextMeshProUGUI partNameText;
    public TextMeshProUGUI descriptionText;

    [Header("Instruction Text")]
    public TextMeshProUGUI instructionText;

    [Header("Heart")]
    public GameObject heartRoot;
    public HeartBeat heartBeat;

    [Header("Arrhythmia Heart Color")]
    public Color normalColor = new Color(0.6f, 0.1f, 0.1f);
    public Color arrhythmiaColor = new Color(0.2f, 0.2f, 0.8f);

    private HeartHotspot[] allHotspots;
    private bool isPanelOpen = false;
    private Renderer heartRenderer;
    private bool firstTapDone = false;

    // Added for tap vs drag detection
    private Vector2 startPos;
    private bool isDragging = false;

    private void Start()
    {
        if (infoPanel != null)
            infoPanel.SetActive(false);

        if (instructionText != null)
        {
            instructionText.text = "Tap the heart to start";
            instructionText.gameObject.SetActive(true);
        }

        if (heartBeat == null && heartRoot != null)
            heartBeat = heartRoot.GetComponent<HeartBeat>();

        if (heartBeat != null)
            heartBeat.isArrhythmia = false;

        if (heartRoot != null)
        {
            allHotspots = heartRoot.GetComponentsInChildren<HeartHotspot>(true);
            heartRenderer = heartRoot.GetComponentInChildren<Renderer>();
            Debug.Log("HeartTouchManager ready. Hotspots: " + allHotspots.Length);
        }

        SetHeartColor(false);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            isDragging = false;
        }

        if (Input.GetMouseButton(0))
        {
            if (Vector2.Distance(startPos, Input.mousePosition) > 10f)
                isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!isDragging)
                HandleTap(Input.mousePosition);
        }
#else
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                startPos = t.position;
                isDragging = false;
            }

            if (t.phase == TouchPhase.Moved)
            {
                if (Vector2.Distance(startPos, t.position) > 10f)
                    isDragging = true;
            }

            if (t.phase == TouchPhase.Ended)
            {
                if (!isDragging)
                    HandleTap(t.position);
            }
        }
#endif
    }

    private void HandleTap(Vector2 screenPos)
    {
        if (arCamera == null || heartRoot == null) return;

        Ray ray = arCamera.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            bool hitHeart = hit.collider.gameObject == heartRoot ||
                            hit.collider.transform.IsChildOf(heartRoot.transform);

            if (hitHeart)
            {
                if (!firstTapDone)
                {
                    firstTapDone = true;

                    if (instructionText != null)
                        instructionText.gameObject.SetActive(false);
                }

                HeartHotspot closest = GetClosestHotspot(hit.point);
                if (closest != null)
                {
                    ShowPartInfo(closest.partName, closest.description);
                    return;
                }
            }
        }

        if (isPanelOpen)
            ClosePanel();
    }

    private HeartHotspot GetClosestHotspot(Vector3 worldPoint)
    {
        if (allHotspots == null || allHotspots.Length == 0) return null;

        HeartHotspot closest = null;
        float minDist = float.MaxValue;

        foreach (HeartHotspot h in allHotspots)
        {
            float dist = Vector3.Distance(worldPoint, h.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = h;
            }
        }

        Debug.Log("Closest: " + (closest != null ? closest.partName : "none"));
        return closest;
    }

    private void ShowPartInfo(string partName, string partDesc)
    {
        if (infoPanel != null) infoPanel.SetActive(true);
        isPanelOpen = true;

        bool isArrhythmia = (heartBeat != null && heartBeat.isArrhythmia);

        if (partNameText != null)
        {
            partNameText.color = Color.black;
            partNameText.text = partName;
        }

        if (descriptionText != null)
        {
            if (isArrhythmia)
            {
                descriptionText.color = Color.black;
                descriptionText.text = partDesc +
                    "\n\n<color=#BF0D0D><b>ARRHYTHMIA DETECTED</b>\n" +
                    "Irregular electrical conduction in this area\n" +
                    "may be disrupting normal blood flow.\n" +
                    "Symptoms: Palpitations, dizziness, fatigue.</color>";
            }
            else
            {
                descriptionText.color = Color.black;
                descriptionText.text = partDesc;
            }
        }
    }

    public void ToggleArrhythmiaFromUI()
    {
        if (heartBeat == null) return;

        heartBeat.ToggleArrhythmia();
        bool isArrhythmia = heartBeat.isArrhythmia;

        SetHeartColor(isArrhythmia);

        if (isArrhythmia)
        {
            if (infoPanel != null) infoPanel.SetActive(true);
            isPanelOpen = true;

            if (partNameText != null)
            {
                partNameText.color = new Color(0.75f, 0.05f, 0.05f);
                partNameText.text = "ARRHYTHMIA";
            }

            if (descriptionText != null)
            {
                descriptionText.color = new Color(0.75f, 0.05f, 0.05f);
                descriptionText.text =
                    "Irregular heartbeat detected.\n\n" +
                    "Meaning: The heart's electrical system is\n" +
                    "firing irregularly.\n\n" +
                    "Symptoms: Palpitations, dizziness, fatigue,\n" +
                    "shortness of breath.\n\n" +
                    "Clinical Note: Irregular electrical conduction\n" +
                    "may disrupt normal blood flow and reduce\n" +
                    "oxygen delivery to vital organs.";
            }
        }
        else
        {
            ClosePanel();
        }
    }

    private void SetHeartColor(bool isArrhythmia)
    {
        if (heartRenderer == null) return;
        Material mat = heartRenderer.material;
        mat.color = isArrhythmia ? arrhythmiaColor : normalColor;
    }

    public void ClosePanel()
    {
        if (infoPanel != null) infoPanel.SetActive(false);
        isPanelOpen = false;
    }
}