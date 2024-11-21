using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected RectTransform rectTransform;
    protected Canvas canvas;
    protected CanvasGroup canvasGroup;
    protected Vector2 originalPosition;
    protected Transform originalParent;

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
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
        if (ShouldReturnToOriginalPosition(eventData))
        {
            rectTransform.anchoredPosition = originalPosition;
            rectTransform.SetParent(originalParent, true);
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            HandleDragEnd(eventData);
        }
    }

    protected virtual void HandleDragStart(PointerEventData eventData) { }
    protected virtual void HandleDragging(PointerEventData eventData) { }
    protected virtual void HandleDragEnd(PointerEventData eventData) { }
    protected virtual bool ShouldReturnToOriginalPosition(PointerEventData eventData) => false;
}
