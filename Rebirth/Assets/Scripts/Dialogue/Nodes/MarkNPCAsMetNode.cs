using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CreateAssetMenu(fileName = "Mark NPC As Met Node", menuName = "Dialogue/Mark NPC As Met Node")]
public class MarkNPCAsMetNode : DialogueNode
{
    [Header("Transition")]
    public DialogueNode nextNode;
    public string NPCName;
  
    private void OnEnable()
    {
        options.Clear();

        DialogueOption option = new HasMetOption(NPCName)
        {
            optionText = "Continue",
            nextNode = nextNode,
        };

        options.Add(option);
    }
}
