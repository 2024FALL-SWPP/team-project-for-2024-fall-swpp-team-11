using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : DragHandler
{
    public ItemData itemData;
    private RectTransform viewportRectTransform;

    private void Start()
    {
        viewportRectTransform ??= transform.parent.parent.GetComponent<RectTransform>();
    }

    override protected void HandleDragStart(PointerEventData eventData)
    {
    }
    
    override protected void HandleDragging(PointerEventData eventData)
    {
    }
    
    override protected void HandleDragEnd(PointerEventData eventData) 
    {
        Vector3? spawnPosition = CalculateSpawnPosition(eventData);
        
        if (spawnPosition.HasValue)
        {
            Instantiate(itemData.prefab, spawnPosition.Value, Quaternion.identity);
            InventoryManager.Instance.RemoveItem(itemData);
            Destroy(gameObject);
        }
    }
    
    override protected bool ShouldReturnToOriginalPosition(PointerEventData eventData)
    {
        Camera eventCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        return RectTransformUtility.RectangleContainsScreenPoint(
            viewportRectTransform,
            eventData.position,
            eventCamera);
    }

    private Vector3? CalculateSpawnPosition(PointerEventData eventData)
    {
        if (DimensionManager.Instance.GetCurrentDimension() == Dimension.TWO_DIMENSION)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            worldPosition.z = 0;
            return worldPosition;
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                return hit.point;
            }
            
            return null;
        }
    }

















    // public ItemDragHandler(RectTransform rectTransform, CanvasGroup canvasGroup, Canvas canvas,
    //     Vector2 originalPosition, Transform originalParent)
    // {
    //     this.rectTransform = rectTransform;
    //     this.canvasGroup = canvasGroup;
    //     this.canvas = canvas;
    //     this.originalPosition = originalPosition;
    //     this.originalParent = originalParent;
    //     this.previewHandler = new ItemPreviewHandler();
    // }

    // public void StartDrag(GameObject prefab)
    // {
    //     canvasGroup.alpha = 0.6f;
    //     canvasGroup.blocksRaycasts = false;
    //     rectTransform.SetParent(canvas.transform, true);
    //     previewHandler.CreatePreview(prefab);
    // }

    // public void Drag(PointerEventData eventData)
    // {
    //     rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    //     previewHandler.UpdatePreviewPosition(eventData.position);
    // }

    // public void EndDrag(bool returnToInventory)
    // {
    //     previewHandler.DestroyPreview();

    //     if (returnToInventory)
    //     {
    //         canvasGroup.alpha = 1f;
    //         canvasGroup.blocksRaycasts = true;
    //         rectTransform.SetParent(originalParent, true);
    //         rectTransform.anchoredPosition = originalPosition;
    //     }
    // }

    // public Vector3? GetSpawnPosition(PointerEventData eventData)
    // {
    //     return previewHandler.GetFinalPosition(eventData.position);
    // }
}