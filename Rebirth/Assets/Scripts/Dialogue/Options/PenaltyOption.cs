using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class PenaltyOption : DialogueOption
{
    public PenaltyOption(ItemData penaltyItem)
    {
        onSelectActions.AddListener(() => {
            InventoryManager.Instance.RemoveItem(penaltyItem);
        });
    }
}
