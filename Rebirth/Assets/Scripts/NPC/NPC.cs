using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NPC : MonoBehaviour, IInteractable
{   
    public string npcName;

    [Header("NPC Quest")]
    public QuestNodes questNodes;

    private static string logPrefix = "[NPC] ";

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
            Debug.LogError(logPrefix + "npcName is not set.");
        }
    }

    public virtual void Interact()
    {
        DialogueManager.Instance.OnDialogueEnd += HandleDialogueEnd;

        DialogueManager.Instance.StartDialogue(this);
    }

    public virtual void HandleDialogueEnd()
    {
        DialogueManager.Instance.OnDialogueEnd -= HandleDialogueEnd;

        // NPCManager.Instance.MarkNPCAsMet(npcName);
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