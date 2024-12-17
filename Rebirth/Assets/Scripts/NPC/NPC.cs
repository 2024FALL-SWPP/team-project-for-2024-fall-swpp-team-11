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
    private Outline2D outline2D;
    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline2D = GetComponent<Outline2D>();
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
        if (DimensionManager.Instance.GetCurrentDimension() == Dimension.THREE_DIMENSION)
        {
            if (!outline) return;
            outline.enabled = true;
        }
        else
        {
            if (!outline2D) return;
            outline2D.SetOutline();
        }
    }

    public void OnDefocus()
    {
        if (DimensionManager.Instance.GetCurrentDimension() == Dimension.THREE_DIMENSION)
        {
            if (!outline) return;
            outline.enabled = false;
        }
        else
        {
            if (!outline2D) return;
            outline2D.UnsetOutline();
        }
    }
}