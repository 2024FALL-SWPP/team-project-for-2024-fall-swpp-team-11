using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class RewardOption : DialogueOption
{

    public RewardOption(ItemData rewardedItem)
    {
        onSelectActions.AddListener(() => {
            InventoryManager.Instance.AddItem(rewardedItem);
        });
    }

}