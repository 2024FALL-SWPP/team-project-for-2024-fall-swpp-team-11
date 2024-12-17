using UnityEngine.EventSystems;
using UnityEngine;

public class ItemPointerHandler : PointerHandler
{
    public ItemData itemData;
    protected override void HandlePointerEnter(PointerEventData eventData)
    {
        Debug.Log("[ItemPointerHandler] HandlePointerEnter / itemData : " + itemData + ".");
        InventoryManager.Instance.ShowTooltip(itemData, eventData.position);
    }

    protected override void HandlePointerExit()
    {
        InventoryManager.Instance.HideTooltip();
    }

    protected override void HandlePointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            InventoryManager.Instance.UseItem(itemData);
        }
    }
}
