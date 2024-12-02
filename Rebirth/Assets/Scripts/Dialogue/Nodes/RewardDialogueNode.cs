using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Reward Dialogue Node", menuName = "Dialogue/Reward Dialogue Node")]
public class RewardDialogueNode : DialogueNode
{
    [Header("Transition")]
    public DialogueNode nextNode;
    [Header("Rewarded Item")]
    public ItemData rewardedItem;

    private void OnEnable()
    {
        options.Clear();

        DialogueOption option = new RewardOption(rewardedItem)
        {
            optionText = "Continue",
            nextNode = nextNode,
        };

        options.Add(option);
    }
}


