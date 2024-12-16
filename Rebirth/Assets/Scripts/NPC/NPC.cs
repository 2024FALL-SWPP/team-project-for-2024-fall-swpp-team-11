using UnityEngine;
using System;

public class NPC : MonoBehaviour, IInteractable
{   
    public string npcName;

    [Header("NPC Quest")]
    public QuestNodes questNodes;

    private static string logPrefix = "[NPC] ";

    private Outline outline;
    private InteractableObject2D interactable2D; // 2D용 InteractableObject2D 참조

    private void Awake()
    {
        outline = GetComponent<Outline>();
        interactable2D = GetComponent<InteractableObject2D>(); // 2D용 컴포넌트 참조
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(npcName))
        {
            NPCManager.Instance.RegisterNPC(npcName);
            Debug.Log(logPrefix + "Registered NPC: " + npcName);
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
        if (string.IsNullOrEmpty(gameObject.name))
        {
            Debug.LogWarning(logPrefix + "GameObject name이 설정되지 않았습니다.");
            return;
        }

        Debug.Log(logPrefix + "OnFocus called for NPC: " + gameObject.name);

        if (gameObject.name.EndsWith("2D", StringComparison.OrdinalIgnoreCase))
        {
            // 2D NPC의 경우 InteractableObject2D의 OnFocus 호출
            if (interactable2D != null)
            {
                interactable2D.OnFocus();
                Debug.Log(logPrefix + "InteractableObject2D OnFocus called for NPC: " + gameObject.name);
            }
            else
            {
                Debug.LogWarning(logPrefix + "InteractableObject2D 컴포넌트가 존재하지 않습니다.");
            }
        }
        else
        {
            // 3D NPC의 경우 Outline 활성화
            if (outline)
            {
                outline.enabled = true;
                Debug.Log(logPrefix + "3D Outline enabled for NPC: " + gameObject.name);
            }
            else
            {
                Debug.LogWarning(logPrefix + "Outline 컴포넌트가 존재하지 않습니다.");
            }
        }
    }

    public void OnDefocus()
    {
        if (string.IsNullOrEmpty(gameObject.name))
        {
            Debug.LogWarning(logPrefix + "GameObject name이 설정되지 않았습니다.");
            return;
        }

        Debug.Log(logPrefix + "OnDefocus called for NPC: " + gameObject.name);

        if (gameObject.name.EndsWith("2D", StringComparison.OrdinalIgnoreCase))
        {
            // 2D NPC의 경우 InteractableObject2D의 OnDefocus 호출
            if (interactable2D != null)
            {
                interactable2D.OnDefocus();
                Debug.Log(logPrefix + "InteractableObject2D OnDefocus called for NPC: " + gameObject.name);
            }
            else
            {
                Debug.LogWarning(logPrefix + "InteractableObject2D 컴포넌트가 존재하지 않습니다.");
            }
        }
        else
        {
            // 3D NPC의 경우 Outline 비활성화
            if (outline)
            {
                outline.enabled = false;
                Debug.Log(logPrefix + "3D Outline disabled for NPC: " + gameObject.name);
            }
            else
            {
                Debug.LogWarning(logPrefix + "Outline 컴포넌트가 존재하지 않습니다.");
            }
        }
    }
}
