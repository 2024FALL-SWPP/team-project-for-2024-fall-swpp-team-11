// NPC.cs
using UnityEngine;
using System;
using System.Collections;

public class NPC : MonoBehaviour, IInteractable
{   
    public string npcName;
    public DialogueNode startingDialogueNode;

    private Outline outline;
    public float interactRange = 3f;
    
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
        NPCManager.Instance.MarkNPCAsMet(npcName);
        DialogueManager.Instance.StartDialogue(startingDialogueNode);
        // TODO
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