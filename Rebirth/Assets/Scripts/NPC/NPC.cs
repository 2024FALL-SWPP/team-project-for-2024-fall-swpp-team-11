using UnityEngine;
using System;
using System.Collections;

public class NPC : MonoBehaviour, IInteractable
{   
    public string npcName;
    public DialogueNode startingDialogueNode;

    private Outline outline;    
    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(npcName))
        {
            NPCManager.Instance.RegisterNPC(npcName);
        }
        else
        {
            Debug.LogError("NPC: npcName is not set.");
        }
    }

    public void Interact()
    {
        DialogueManager.Instance.OnDialogueEnd += HandleDialogueEnd;

        DialogueManager.Instance.StartDialogue(startingDialogueNode);
    }

    private void HandleDialogueEnd()
    {
        DialogueManager.Instance.OnDialogueEnd -= HandleDialogueEnd;

        NPCManager.Instance.MarkNPCAsMet(npcName);
    }


    public void OnFocus()
    {
        if (!outline) return;

        outline.enabled = true;
    }

    public void OnDefocus()
    {
        if (!outline) return;

        outline.enabled = false;
    }
}