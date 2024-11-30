using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Mark NPC As Met Node", menuName = "Dialogue/Mark NPC As Met Node")]
public class MarkNPCAsMetNode : DialogueNode
{
    [Header("Transition")]
    public DialogueNode nextNode;
    public string NPCName;
  
    private void OnEnable()
    {
        options.Clear();

        if (Application.isPlaying)
        {
            NPCManager.Instance.MarkNPCAsMet(NPCName);
        }
    
        DialogueOption option = new AlwaysTrueOption()
        {
            optionText = "Continue",
            nextNode = nextNode
        };

        options.Add(option);
    }
}
