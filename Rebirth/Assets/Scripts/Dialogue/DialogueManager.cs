using UnityEngine;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public DialogueNode startNode;
    private DialogueNode currentNode;
    private DialogueUI dialogueUI;

    public static DialogueManager Instance { get; private set; }
    public bool IsDialogueActive;
    private void Awake()
    {
        // use singleton pattern
        if (Instance == null && Instance != this)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (dialogueUI == null)
        {
            Debug.LogError("DialogueUI not found.");
        }
    }

    private void Start()
    {
        currentNode = startNode; // TODO: initialize
        // dialogueUI = GetComponent<DialogueUI>(); DialogueUI를 어디에 둘 것인가
    }

    public void StartDialogue(DialogueNode startingNode)
    {
        currentNode = startingNode;
        IsDialogueActive = true;
        DisplayCurrentDialogueNode();
    }

    public void SelectOption(DialogueOption selectedOption)
    {
        if (selectedOption.nextNode == null)
        {
            EndDialogue();
            return;
        }

        if (selectedOption.AreConditionsMet())
        {
            currentNode = selectedOption.nextNode;
            DisplayCurrentDialogueNode();
        }
        else
        {
            EndDialogue(); // TODO: fallback dialogue
            Debug.Log("Conditions not met.");
        }
    }

    public void SelectOption(int optionIndex)
    {
        if (optionIndex < 0 || optionIndex >= currentNode.options.Count)
        {
            Debug.LogError("Invalid option index.");
            return;
        }

        DialogueOption selectedOption = currentNode.options[optionIndex];
        SelectOption(selectedOption);
    }

    private void DisplayCurrentDialogueNode()
    {
        dialogueUI.ShowDialogue(currentNode);
    }

    private void EndDialogue()
    {
        Debug.Log("Dialogue ended.");
        IsDialogueActive = false;
        currentNode = null;
        dialogueUI.HideDialogue();
    }
}
