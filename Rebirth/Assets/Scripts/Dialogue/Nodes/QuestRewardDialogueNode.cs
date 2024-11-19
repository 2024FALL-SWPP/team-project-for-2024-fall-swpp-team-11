using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Quest Reward Dialogue Node", menuName = "Dialogue/Quest Reward Dialogue Node")]
public class QuestRewardDialogueNode : QuestDialogueNode
{
    [Header("Transition")]
    public DialogueNode nextNode;
    
    private void OnEnable()
    {
        options.Clear();

        DialogueOption option = new QuestRewardOption(associatedQuest)
        {
            optionText = "Continue",
            nextNode = nextNode,
        };
        options.Add(option);
    }
}
