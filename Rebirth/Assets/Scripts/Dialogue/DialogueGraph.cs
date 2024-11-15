using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueGraph", menuName = "Dialogue/Dialogue Graph")]
public class DialogueGraph : ScriptableObject
{
    public List<DialogueNode> dialogueNodes = new List<DialogueNode>();

    public void AddDialogueNode(DialogueNode node)
    {
        if (!dialogueNodes.Contains(node))
        {
            dialogueNodes.Add(node);
        }
    }

    public void RemoveDialogueNode(DialogueNode node)
    {
        if (dialogueNodes.Contains(node))
        {
            dialogueNodes.Remove(node);
        }
    }
}
