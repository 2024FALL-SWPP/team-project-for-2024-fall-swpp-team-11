using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler
{
    private readonly RectTransform rectTransform;
    private readonly CanvasGroup canvasGroup;
    private readonly Canvas canvas;
    private readonly Vector2 originalPosition;
    private readonly Transform originalParent;

    public ItemDragHandler(RectTransform rectTransform, CanvasGroup canvasGroup, Canvas canvas, 
        Vector2 originalPosition, Transform originalParent)
    {
        this.rectTransform = rectTransform;
        this.canvasGroup = canvasGroup;
        this.canvas = canvas;
        this.originalPosition = originalPosition;
        this.originalParent = originalParent;
    }

    public void StartDrag()
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        rectTransform.SetParent(canvas.transform, true);
    }

    public void Drag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void EndDrag()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        rectTransform.SetParent(originalParent, true);
        rectTransform.anchoredPosition = originalPosition;
    }
}