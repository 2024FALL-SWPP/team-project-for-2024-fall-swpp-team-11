using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Plain Next Dialogue Node", menuName = "Dialogue/Quest Reward Dialogue Node")]
public class QuestRewardDialogueNode : QuestDialogueNode
{
    [Header("Transition")]
    public DialogueNode nextNode;

    private void OnEnable()
    {
        DialogueOption option = new QuestRewardOption(associatedQuest)
        {
            optionText = "Continue",
            nextNode = nextNode,
        };
        options.Add(option);
    }
}
