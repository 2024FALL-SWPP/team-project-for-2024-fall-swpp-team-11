using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class QuestOption : AlwaysTrueOption
{
    public QuestData associatedQuest;

    private static string logPrefix = "[QuestOption] ";

    public QuestOption(
        QuestData associatedQuest
    )
    {
        if (associatedQuest == null)
        {
            Debug.LogError(logPrefix + "associatedQuest is null");
        }
        this.associatedQuest = associatedQuest;
    }
}