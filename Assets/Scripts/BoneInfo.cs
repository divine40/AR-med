using UnityEngine;
using System.Collections;

public class BoneInfo : MonoBehaviour
{
    [Header("Info")]
    public string boneName = "Bone";

    [TextArea(2, 6)]
    public string functionText = "Function of this bone...";

    private Renderer[] rends;
    private Color[] originalColors;

    void Awake()
    {
        rends = GetComponentsInChildren<Renderer>(true);

        originalColors = new Color[rends.Length];
        for (int i = 0; i < rends.Length; i++)
        {
            if (rends[i] != null && rends[i].material != null && rends[i].material.HasProperty("_Color"))
                originalColors[i] = rends[i].material.color;
        }
    }

    public void SetTint(Color c)
    {
        if (rends == null) return;

        for (int i = 0; i < rends.Length; i++)
        {
            if (rends[i] != null && rends[i].material != null && rends[i].material.HasProperty("_Color"))
                rends[i].material.color = c;
        }
    }

    public void RestoreTint()
    {
        if (rends == null) return;

        for (int i = 0; i < rends.Length; i++)
        {
            if (rends[i] != null && rends[i].material != null && rends[i].material.HasProperty("_Color"))
                rends[i].material.color = originalColors[i];
        }
    }

    public void Flash(float duration = 0.15f)
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine(duration));
    }

    private IEnumerator FlashRoutine(float duration)
    {
        SetTint(Color.white);
        yield return new WaitForSeconds(duration);
        RestoreTint();
    }
}