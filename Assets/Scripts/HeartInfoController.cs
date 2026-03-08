using UnityEngine;
using TMPro;

public class HeartInfoController : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text infoText;

    [Header("Heart Logic")]
    public HeartBeat heartBeat;

    [TextArea(3, 10)]
    public string normalInfo =
        "HEART (Normal)\n\nFunction: Pumps oxygenated blood throughout the body.\nStatus: Stable sinus rhythm.";

    [TextArea(3, 10)]
    public string arrhythmiaInfo =
        "ARRHYTHMIA\n\nMeaning: Irregular heartbeat.\nSymptoms: Palpitations, dizziness, fatigue.\nClinical Note: Irregular electrical conduction may disrupt blood flow.";

    void Start()
    {
        if (heartBeat == null) heartBeat = GetComponent<HeartBeat>();

        // IMPORTANT: Don't show text immediately
        HideInfo();
    }

    // Call this when user taps the heart
    public void ShowInfo()
    {
        if (infoText == null) return;

        infoText.gameObject.SetActive(true);

        bool isArr = (heartBeat != null && heartBeat.isArrhythmia);

        if (isArr)
        {
            // Professional dark medical red
            infoText.color = new Color(0.75f, 0.05f, 0.05f);

            infoText.text =
                "<b>ARRHYTHMIA</b>\n\n" +
                "Meaning: Irregular heartbeat.\n" +
                "Symptoms: Palpitations, dizziness, fatigue.\n\n" +
                "Clinical Note: Irregular electrical conduction may disrupt blood flow.";
        }
        else
        {
            // Normal = black
            infoText.color = Color.black;

            infoText.text =
                "<b>HEART (Normal)</b>\n\n" +
                "Function: Pumps oxygenated blood throughout the body.\n\n" +
                "Status: Stable sinus rhythm.";
        }
    }

    // ARInteraction might call this when you tap empty space
    public void ClearInfo()
    {
        if (infoText == null) return;
        infoText.text = "";
        infoText.gameObject.SetActive(false);
    }

    // Some scripts call HideInfo() specifically
    public void HideInfo()
    {
        if (infoText == null) return;
        infoText.gameObject.SetActive(false);
    }

    // Button calls this
    public void ToggleArrhythmiaFromUI()
    {
        if (heartBeat == null) return;
        heartBeat.ToggleArrhythmia();

        // Refresh the text if it's currently visible
        if (infoText != null && infoText.gameObject.activeSelf)
            ShowInfo();
    }
}