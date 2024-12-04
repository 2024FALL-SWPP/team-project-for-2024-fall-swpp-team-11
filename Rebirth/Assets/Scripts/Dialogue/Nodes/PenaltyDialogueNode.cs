using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Penalty Dialogue Node", menuName = "Dialogue/Penalty Dialogue Node")]
public class PenaltyDialogueNode : DialogueNode
{
    [Header("Transition")]
    public DialogueNode nextNode;
    [Header("Penalty Item")]
    public ItemData penaltyItem;

    private void OnEnable()
    {
        options.Clear();

        DialogueOption option = new PenaltyOption(penaltyItem)
        {
            optionText = "Continue",
            nextNode = nextNode,
        };

        options.Add(option);
    }
}
