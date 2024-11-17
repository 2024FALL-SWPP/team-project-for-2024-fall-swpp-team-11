using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Quest Nodes", menuName = "Quest/Create New Quest Nodes")]
public class QuestNodes : ScriptableObject
{
    public QuestData questData;
    
    public DialogueNode notStartedNode;
    public DialogueNode acceptedNode;
    public DialogueNode completedNode;
    public DialogueNode defaultNode;

    private Dictionary<QuestStatus, DialogueNode> questNodes;

    private void OnEnable()
    {
        questNodes = new Dictionary<QuestStatus, DialogueNode>
        {
            { QuestStatus.NotStarted, notStartedNode },
            { QuestStatus.Accepted, acceptedNode },
            { QuestStatus.Completed, completedNode },
        };
    }

    public DialogueNode GetQuestNodeOrDefault(QuestStatus status)
    {
        if (questNodes.ContainsKey(status))
        {
            return questNodes[status];
        }
        else
        {
            return defaultNode;
        }
    }
}