using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Dialogue/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    public string nodeID;
    
    [TextArea(3, 10)]
    public string dialogueText;
    public List<DialogueOption> options = new List<DialogueOption>();

    [HideInInspector]
    public string conversationNpcName;
}
