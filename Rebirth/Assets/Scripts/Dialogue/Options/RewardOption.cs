using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class RewardOption : MonoBehaviour
{
    public RewardOption() 
    {
        onSelectActions.AddListener(() => { 
            QuestManager.Instance.CompleteQuest(associatedQuest.questID);
        });
    }
}