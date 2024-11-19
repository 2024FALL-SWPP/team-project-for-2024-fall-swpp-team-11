using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class QuestOfferAcceptOption : QuestOption
{
    public QuestOfferAcceptOption(
        QuestData associatedQuest
    ) : base(associatedQuest)
    {
        onSelectActions.AddListener(() => { 
            QuestManager.Instance.AcceptQuest(associatedQuest); 
        });
    }
}