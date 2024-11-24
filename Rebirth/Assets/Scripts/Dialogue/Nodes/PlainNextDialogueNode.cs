using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Plain Next Dialogue Node", menuName = "Dialogue/Plain Next Dialogue Node")]
public class PlainNextDialogueNode : DialogueNode
{
    [Header("Transition")]
    public DialogueNode nextNode;

    private void OnEnable()
    {
        options.Clear();

        DialogueOption option = new AlwaysTrueOption()
        {
            optionText = "Continue",
            nextNode = nextNode
        };

        options.Add(option);
    }
}
