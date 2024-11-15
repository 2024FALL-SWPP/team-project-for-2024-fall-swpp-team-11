using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Dialogue/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    public string dialogueText;
    public List<DialogueOption> options;
}
