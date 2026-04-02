using UnityEngine;

public class HeartBeat : MonoBehaviour
{
    [Header("Beat Motion")]
    public bool isBeating = true;

    [Tooltip("Beats per second — 1.2 = normal resting heart")]
    public float beatSpeed = 1.2f;

    [Tooltip("How much the heart grows on each beat — 0.08 to 0.15 is realistic")]
    public float beatStrength = 0.1f;

    [Header("Physical Motion")]
    [Tooltip("How much the heart moves up during a beat")]
    public float verticalMotion = 0.03f;

    [Tooltip("How much the heart slightly twists")]
    public float rotationMotion = 3f;

    [Header("Arrhythmia")]
    public bool isArrhythmia = false;
    public float arrhythmiaMinSpeed = 1.8f;
    public float arrhythmiaMaxSpeed = 2.6f;

    [Header("Sound")]
    public AudioSource beatAudioSource;
    public float arrhythmiaSoundPitch = 1.5f;

    private Vector3 baseScale;
    private Vector3 basePosition;
    private Quaternion baseRotation;

    private Renderer heartRenderer;
    private Color originalColor;

    private float beatTimer = 0f;
    private float currentBeatSpeed;

    void Start()
    {
        baseScale = transform.localScale;
        basePosition = transform.localPosition;
        baseRotation = transform.localRotation;

        heartRenderer = GetComponentInChildren<Renderer>();
        if (heartRenderer == null)
            heartRenderer = GetComponent<Renderer>();

        if (heartRenderer != null)
            originalColor = heartRenderer.material.color;

        if (beatAudioSource == null)
            beatAudioSource = GetComponent<AudioSource>();

        if (beatAudioSource != null)
        {
            beatAudioSource.loop = true;
            beatAudioSource.playOnAwake = false;
            beatAudioSource.pitch = 1f;
            beatAudioSource.Play();
        }

        currentBeatSpeed = beatSpeed;
    }

    void Update()
    {
        if (!isBeating) return;

        if (isArrhythmia)
            currentBeatSpeed = Random.Range(arrhythmiaMinSpeed, arrhythmiaMaxSpeed);
        else
            currentBeatSpeed = beatSpeed;

        beatTimer += Time.deltaTime * currentBeatSpeed;

        // Lub beat
        float lub = Mathf.Clamp01(Mathf.Sin(beatTimer * Mathf.PI));
        lub = Mathf.Pow(lub, 3f);

        // Dub beat
        float dubOffset = beatTimer - 0.35f;
        float dub = Mathf.Clamp01(Mathf.Sin(dubOffset * Mathf.PI * 1.4f));
        dub = Mathf.Pow(dub, 5f) * 0.45f;

        if (beatTimer >= 1f)
            beatTimer = 0f;

        float pulse = (lub + dub) * beatStrength;

        // Scale
        transform.localScale = baseScale * (1f + pulse);

        // Vertical movement
        transform.localPosition = basePosition + new Vector3(0f, pulse * verticalMotion, 0f);

        // Slight rotation
        transform.localRotation = baseRotation * Quaternion.Euler(pulse * rotationMotion, 0f, 0f);
    }

    public void ToggleArrhythmia()
    {
        isArrhythmia = !isArrhythmia;

        if (beatAudioSource != null)
            beatAudioSource.pitch = isArrhythmia ? arrhythmiaSoundPitch : 1f;
    }

    public void StartBeat()
    {
        isBeating = true;

        if (beatAudioSource != null && !beatAudioSource.isPlaying)
            beatAudioSource.Play();
    }

    public void StopBeat()
    {
        isBeating = false;

        transform.localScale = baseScale;
        transform.localPosition = basePosition;
        transform.localRotation = baseRotation;

        if (beatAudioSource != null)
            beatAudioSource.Stop();
    }

    public void ResetHeart()
    {
        isArrhythmia = false;
        isBeating = true;
        beatTimer = 0f;

        transform.localScale = baseScale;
        transform.localPosition = basePosition;
        transform.localRotation = baseRotation;

        if (heartRenderer != null)
            heartRenderer.material.color = originalColor;

        if (beatAudioSource != null)
        {
            beatAudioSource.pitch = 1f;

            if (!beatAudioSource.isPlaying)
                beatAudioSource.Play();
        }
    }
}