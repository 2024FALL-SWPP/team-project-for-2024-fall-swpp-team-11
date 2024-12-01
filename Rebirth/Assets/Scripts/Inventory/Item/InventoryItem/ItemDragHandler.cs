using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        RemoveItemFromOriginalCell();
    }

    override protected void HandleDragging(PointerEventData eventData)
    {
        
    }

    protected override void HandleDragEnd(PointerEventData eventData)
    {
        Camera eventCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        if (IsWithinViewport(eventData, eventCamera))
        {
            HandleDropWithinInventory(eventData);
        }
        else
        {
            HandleDropOutsideInventory(eventData);
        }
    }

    private void RemoveItemFromOriginalCell()
    {
        GridCell gridCell = originalParent.GetComponent<GridCell>();
        if (gridCell != null)
        {
            gridCell.RemoveItem();
        }
    }

    private bool IsWithinViewport(PointerEventData eventData, Camera eventCamera)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(viewportRectTransform, eventData.position, eventCamera);
    }

    private void HandleDropWithinInventory(PointerEventData eventData)
    {
        GridCell targetCell = DetectGridCell(eventData);

        if (targetCell != null)
        {
            if (targetCell.IsEmpty())
            {
                targetCell.AddItem(gameObject);
            }
            else
            {
                SwapItemsWithTargetCell(targetCell);
            }
        }
        else
        {
            ResetToOriginalCell();
        }
    }

    private void HandleDropOutsideInventory(PointerEventData eventData)
    {
        Vector3? spawnPosition = CalculateSpawnPosition(eventData);

        if (spawnPosition.HasValue)
        {
            Instantiate(itemData.prefab, spawnPosition.Value, Quaternion.identity);
            InventoryManager.Instance.RemoveItem(itemData);
            Destroy(gameObject);
        }
        else
        {
            ResetToOriginalCell();
        }
    }

    private void SwapItemsWithTargetCell(GridCell targetCell)
    {
        GameObject tmpObj = targetCell.inventoryItemObj;
        targetCell.AddItem(gameObject);

        GridCell originalCell = originalParent.GetComponent<GridCell>();
        if (originalCell != null)
        {
            originalCell.AddItem(tmpObj);
            InventoryEvents.ItemSwapped(originalCell, targetCell);
        }
    }

    private void ResetToOriginalCell()
    {
        GridCell originalCell = originalParent.GetComponent<GridCell>();
        if (originalCell != null)
        {
            originalCell.AddItem(gameObject);
        }
    }

    private GridCell DetectGridCell(PointerEventData eventData)
    {
        GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
        if (raycaster == null) return null;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        foreach (RaycastResult result in results)
        {
            GridCell gridCell = result.gameObject.GetComponent<GridCell>();
            if (gridCell != null)
            {
                return gridCell;
            }
        }

        return null;
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
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.point;
            }
            return null;
        }
    }
}
