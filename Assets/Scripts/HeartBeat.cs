using UnityEngine;

public class HeartBeat : MonoBehaviour
{
    [Header("Beat Motion")]
    public bool isBeating = true;
    public float beatSpeed = 1.2f;
    public float beatStrength = 0.15f;

    [Header("Arrhythmia")]
    public bool isArrhythmia = false;
    public float arrhythmiaMinSpeed = 2f;
    public float arrhythmiaMaxSpeed = 4f;

    [Header("Sound")]
    public AudioSource beatAudioSource;
    public float arrhythmiaSoundPitch = 1.5f;

    private Vector3 baseScale;
    private Renderer heartRenderer;
    private Color originalColor;

    void Start()
    {
        baseScale = transform.localScale;

        heartRenderer = GetComponent<Renderer>();

        if (heartRenderer != null)
        {
            originalColor = heartRenderer.material.GetColor("_Color");
        }

        if (beatAudioSource == null)
            beatAudioSource = GetComponent<AudioSource>();

        if (beatAudioSource != null)
        {
            beatAudioSource.loop = true;
            beatAudioSource.playOnAwake = false;
        }
    }
    void Update()
    {
        if (!isBeating) return;

        float speed = isArrhythmia
            ? Random.Range(arrhythmiaMinSpeed, arrhythmiaMaxSpeed)
            : beatSpeed;

        float t = Time.time * speed;

        // Create double beat pattern (Lub-Dub)
        float pulse1 = Mathf.Sin(t * 3f) * beatStrength;
        float pulse2 = Mathf.Sin(t * 6f) * (beatStrength * 0.5f);

        float pulse = Mathf.Max(0, pulse1) + Mathf.Max(0, pulse2);

        transform.localScale = baseScale + Vector3.one * pulse;
    }

    public void ToggleArrhythmia()
    {
        isArrhythmia = !isArrhythmia;

        // Change heart color
        if (heartRenderer != null)
        {
            if (isArrhythmia)
                heartRenderer.material.SetColor("_Color", new Color(0.6f, 0.1f, 0.1f));
            else
                heartRenderer.material.SetColor("_Color", originalColor);
        }

        // Change sound speed
        if (beatAudioSource != null)
        {
            beatAudioSource.pitch = isArrhythmia ? arrhythmiaSoundPitch : 1f;
        }
    }

    public void StartBeat()
    {
        isBeating = true;
        if (beatAudioSource != null) beatAudioSource.Play();
    }

    public void StopBeat()
    {
        isBeating = false;
        if (beatAudioSource != null) beatAudioSource.Stop();
    }

    public void ResetHeart()
    {
        // stop arrhythmia
        isArrhythmia = false;

        // restore scale
        transform.localScale = baseScale;

        // restore color
        if (heartRenderer != null)
            heartRenderer.material.SetColor("_Color", originalColor);

        // restore sound
        if (beatAudioSource != null)
            beatAudioSource.pitch = 1f;

        // optionally ensure beating is ON
        isBeating = true;
        if (beatAudioSource != null && !beatAudioSource.isPlaying)
            beatAudioSource.Play();
    }
}