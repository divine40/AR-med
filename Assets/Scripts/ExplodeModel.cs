using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ExplodeModel : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float explodeDistance = 1.2f;  // increase/decrease to control spacing
    public float moveSpeed = 6f;          // smoothness/speed

    [Header("UI")]
    public TMP_Text explodeButtonText;

    private bool isExploded = false;
    public bool IsExploded => isExploded;

    private Transform[] parts;
    private Vector3[] originalPositions;
    private Vector3[] explodedPositions;

    // Groups
    private List<int> ribs = new List<int>();
    private List<int> spine = new List<int>();
    private List<int> arms = new List<int>();
    private List<int> legs = new List<int>();
    private List<int> head = new List<int>();

    void Awake()
    {
        CacheParts();
        CategorizeBones();
        ComputeExplodedTargets();
        UpdateButtonLabel();
    }

    void CacheParts()
    {
        int count = transform.childCount;

        parts = new Transform[count];
        originalPositions = new Vector3[count];
        explodedPositions = new Vector3[count];

        for (int i = 0; i < count; i++)
        {
            parts[i] = transform.GetChild(i);
            originalPositions[i] = parts[i].localPosition;
            explodedPositions[i] = originalPositions[i]; // default
        }
    }

    void CategorizeBones()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            string name = parts[i].name.ToLower();

            if (name.Contains("rip"))
                ribs.Add(i);
            else if (name.Contains("spin"))
                spine.Add(i);
            else if (name.Contains("forearm") || name.Contains("hand") || name.Contains("shoulder"))
                arms.Add(i);
            else if (name.Contains("femur") || name.Contains("thigh") || name.Contains("tibia") || name.Contains("foot"))
                legs.Add(i);
            else if (name.Contains("skull") || name.Contains("jaw"))
                head.Add(i);
        }
    }

    // IMPORTANT: fixed target positions (no drifting)
    void ComputeExplodedTargets()
    {
        // Start with original positions
        for (int i = 0; i < parts.Length; i++)
            explodedPositions[i] = originalPositions[i];

        // Apply offsets per group (local space)
        ApplyOffset(ribs, Vector3.right);
        ApplyOffset(spine, Vector3.forward);
        ApplyOffset(arms, Vector3.left);
        ApplyOffset(legs, Vector3.back);
        ApplyOffset(head, Vector3.up);
    }

    void ApplyOffset(List<int> group, Vector3 dir)
    {
        for (int j = 0; j < group.Count; j++)
        {
            int i = group[j];
            explodedPositions[i] = originalPositions[i] + dir.normalized * explodeDistance;
        }
    }

    void Update()
    {
        if (parts == null || parts.Length == 0) return;

        Vector3[] target = isExploded ? explodedPositions : originalPositions;

        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].localPosition = Vector3.Lerp(parts[i].localPosition, target[i], Time.deltaTime * moveSpeed);
        }
    }

    public void ToggleExplode()
    {
        isExploded = !isExploded;
        UpdateButtonLabel();
    }

    public void ResetModel()
    {
        isExploded = false;
        UpdateButtonLabel();

        for (int i = 0; i < parts.Length; i++)
            parts[i].localPosition = originalPositions[i];
    }

    void UpdateButtonLabel()
    {
        if (explodeButtonText != null)
            explodeButtonText.text = isExploded ? "Assemble" : "Explode";
    }
}