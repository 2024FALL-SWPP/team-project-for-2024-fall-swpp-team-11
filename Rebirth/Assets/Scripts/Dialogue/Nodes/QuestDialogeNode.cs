using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

// [CreateAssetMenu(fileName = "Plain Next Dialogue Node", menuName = "Dialogue/Quest Reward Dialogue Node")]
public class QuestDialogueNode : DialogueNode
{
    [Header("Quest Data")]
    public QuestData associatedQuest;
}
