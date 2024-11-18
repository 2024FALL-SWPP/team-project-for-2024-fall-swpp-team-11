using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Conditions Dialogue Node", menuName = "Dialogue/Conditions Dialogue Node")]
public class ConditionsDialogueNode : DialogueNode
{
    public DialogueNode conditionMetNextNode;
    public DialogueNode conditionNotMetNextNode;
    public List<DialogueCondition> conditions;

    private void OnEnable()
    {
        options.Clear();

        DialogueOption option = new DialogueOption
        {
            optionText = "Continue",
            nextNode = conditionMetNextNode,
            fallbackNode = conditionNotMetNextNode,
            conditions = conditions
        };

        options.Add(option);
    }
}
