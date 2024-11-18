using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[System.Serializable]
public class DialogueOption
{
    public string optionText;
    public DialogueNode nextNode;
    public DialogueNode fallbackNode;
    public List<DialogueCondition> conditions;

    public UnityEvent onSelectActions;

    public DialogueOption()
    {
        conditions = new List<DialogueCondition>();
        onSelectActions = new UnityEvent();
    }

    public bool AreConditionsMet()
    {
        foreach (DialogueCondition condition in conditions)
        {
            if (!condition.IsMet())
            {
                return false;
            }
        }

        return true;
    }
}