using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Reward Dialogue Node", menuName = "Dialogue/Reward Dialogue Node")]
public class RewardDialogueNode : QuestDialogueNode
{
    [Header("Transition")]
    public DialogueNode nextNode;
    
    private void OnEnable()
    {
        options.Clear();

        DialogueOption option = new RewardOption(associatedQuest)
        {
            optionText = "Continue",
            nextNode = nextNode,
        };
        options.Add(option);
    }
}