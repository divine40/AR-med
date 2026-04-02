using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClickEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = originalScale * 0.9f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }
}