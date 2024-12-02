using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected RectTransform rectTransform;
    protected Canvas canvas;
    protected CanvasGroup canvasGroup;
    protected GameObject originalParent;

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvas = canvas == null ? GetComponentInParent<Canvas>() : canvas;
        
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        originalParent = transform.parent.gameObject;
        rectTransform.SetParent(canvas.transform, true);

        HandleDragStart(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        HandleDragging(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        HandleDragEnd(eventData);
    }

    protected virtual void HandleDragStart(PointerEventData eventData) { }
    protected virtual void HandleDragging(PointerEventData eventData) { }
    protected virtual void HandleDragEnd(PointerEventData eventData) { }
    protected virtual bool ShouldReturnToOriginalPosition(PointerEventData eventData) => false;
}
