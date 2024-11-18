using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class QuestRewardOption : QuestOption
{
    public QuestRewardOption(
        QuestData associatedQuest
    ) : base(associatedQuest)
    {
        onSelectActions.AddListener(() => { 
            QuestManager.Instance.CompleteQuest(associatedQuest.questID);
        });
    }
}