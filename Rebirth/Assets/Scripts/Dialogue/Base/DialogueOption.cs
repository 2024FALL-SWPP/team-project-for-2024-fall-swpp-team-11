using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

// [CreateAssetMenu(fileName = "New Dialogue Option", menuName = "Dialogue/Dialogue Option")]
public class DialogueOption // : ScriptableObject
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