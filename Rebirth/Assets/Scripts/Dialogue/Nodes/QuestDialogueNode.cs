using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Plain Next Dialogue Node", menuName = "Dialogue/Quest Dialogue Node")]
public class QuestDialogueNode : DialogueNode
{
    [Header("Quest Data")]
    public Quest associatedQuest;
}
