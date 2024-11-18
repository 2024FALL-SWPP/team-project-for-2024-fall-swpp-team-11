// QuestStatusCondition.cs
using UnityEngine;

[CreateAssetMenu(fileName = "QuestStatusCondition", menuName = "Dialogue/Conditions/Quest Status Condition")]
public class QuestStatusCondition : DialogueCondition
{
    public QuestStatus requiredStatus;
    public QuestData quest;

    public override bool IsMet()
    {
        return QuestManager.Instance.GetQuestStatus(quest) == requiredStatus;
    }
}