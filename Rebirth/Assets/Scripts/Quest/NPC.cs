// NPC.cs
using UnityEngine;
using System;
using System.Collections;

public class NPC : MonoBehaviour, IInteractable
{   
    public Dialogue dialogue;
    public QuestData quest;

    private Outline outline;
    public float interactRange = 3f;
    
    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public void Interact()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(dialogue);
            DialogueManager.Instance.OnDialogueEnded += HandleDialogueEnded;
        }
    }

    private void HandleDialogueEnded(bool accepted)
    {
        if (accepted)
        {
            QuestManager.Instance.AddQuest(quest);
            Debug.Log($"퀘스트 수락: {quest.questTitle}");
        }
        else
        {
            Debug.Log("퀘스트 수락하지 않음.");
        }
        DialogueManager.Instance.OnDialogueEnded -= HandleDialogueEnded;
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
