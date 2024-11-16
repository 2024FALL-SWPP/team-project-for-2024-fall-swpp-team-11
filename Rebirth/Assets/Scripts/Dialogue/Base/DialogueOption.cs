using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DialogueOption
{
    public string optionText;
    public DialogueNode nextNode;
    public DialogueNode fallbackNode;
    public List<DialogueCondition> conditions;

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