using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Plain Next Dialogue Node", menuName = "Dialogue/Plain Next Dialogue Node")]
public class PlainNextDialogueNode : DialogueNode
{
    public DialogueNode nextNode;
    private AlwaysTrueCondition alwaysTrueCondition;

    private void OnEnable()
    {
        if (options == null)
        {
            options = new List<DialogueOption>();
        }

        options.Clear();

        DialogueOption option = new DialogueOption
        {
            optionText = "Continue",
            nextNode = nextNode,
            conditions = new List<DialogueCondition>()
        };

        // if (alwaysTrueCondition == null)
        // {
        alwaysTrueCondition = CreateInstance<AlwaysTrueCondition>();
        // }
        option.conditions.Add(alwaysTrueCondition);

        options.Add(option);
    }
}
