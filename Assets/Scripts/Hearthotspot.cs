using UnityEngine;
public class HeartHotspot : MonoBehaviour
{
    [Header("Part Information")]
    public string partName = "Heart Part";

    [TextArea(3, 8)]
    public string description = "Enter description here.";

    // How close (in world units) a raycast hit must be to trigger this hotspot
    public float detectionRadius = 0.05f;

    private void Start()
    {
        // Make the hotspot sphere invisible at runtime
        // (it's only visible in the editor so you can position it)
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
            rend.enabled = false;

        // Disable the collider on the hotspot itself
        // (we use the heart's main collider for the raycast)
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;
    }
}