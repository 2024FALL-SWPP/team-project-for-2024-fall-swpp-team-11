using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class QuestOfferRejectOption : QuestOption
{
    public QuestOfferRejectOption(
        QuestData associatedQuest
    ) : base(associatedQuest)
    {
        onSelectActions.AddListener(() => { 
            // Debug.Log("Rejecting quest: " + associatedQuest.questTitle);
        });
    }
}