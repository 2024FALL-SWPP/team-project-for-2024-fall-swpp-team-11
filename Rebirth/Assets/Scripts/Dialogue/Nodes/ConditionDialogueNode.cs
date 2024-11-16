using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Plain Next Dialogue Node", menuName = "Dialogue/Condition Dialogue Node")]
public class ConditionDialogueNode : DialogueNode
{
    public DialogueNode conditionMetNode;
    public DialogueNode conditionNotMetNode;
    public List<DialogueCondition> conditions;

    private void OnEnable()
    {
        if (options == null || options.Count == 0)
        {
            options = new List<DialogueOption>();

            DialogueOption option = new DialogueOption
            {
                optionText = "Continue",
                nextNode = conditionMetNode,
                fallbackNode = conditionNotMetNode,
                conditions = conditions
            };

            options.Add(option);
        }
    }
}
