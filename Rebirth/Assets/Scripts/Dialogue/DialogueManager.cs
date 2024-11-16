using UnityEngine;
using System.Collections.Generic;
using System;

public class DialogueManager : MonoBehaviour
{
    // public DialogueNode startNode;

    [HideInInspector]
    public DialogueNode currentNode;
    public DialogueUI dialogueUI;
    public DialogueGraph activeDialogueGraph;

    public event Action OnDialogueStart;
    public event Action OnDialogueEnd;

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
        // dialogueUI = GetComponent<DialogueUI>(); // DialogueUI를 어디에 둘 것인가

        // TODO default graph
        if (activeDialogueGraph == null)
        {
            LoadDefaultDialogueGraph();
        }

        // currentNode = startNode; // TODO: initialize
    }

    private void LoadDefaultDialogueGraph()
    {
        DialogueGraph defaultGraph = Resources.Load<DialogueGraph>("DefaultDialogueGraph");
        if (defaultGraph != null)
        {
            LoadDialogueGraph(defaultGraph);
        }
        else
        {
            Debug.LogWarning("DefaultDialogueGraph not found.");
        }
    }

    public void LoadDialogueGraph(DialogueGraph graph)
    {
        activeDialogueGraph = graph;
        Debug.Log("Loaded dialogue graph.");
        // TODO validate graph
    }

    public static void EditorLoadDialogueGraph(DialogueGraph graph)
    {
#if UNITY_EDITOR
        if (Instance != null)
        {
            Instance.LoadDialogueGraph(graph);
            Debug.Log("Editor loaded dialogue graph.");
        }
        else
        {
            Debug.LogWarning("DialogueManager not found.");
        }
#endif
    }

    public void StartDialogue(DialogueNode startingNode)
    {
        if (activeDialogueGraph == null)
        {
            Debug.LogError("No active dialogue graph.");
            return;
        }

        if (!activeDialogueGraph.dialogueNodes.Contains(startingNode))
        {
            Debug.LogWarning("Starting node not in active dialogue graph.");
        }

        currentNode = startingNode;
        IsDialogueActive = true;
        OnDialogueStart?.Invoke();
        DisplayCurrentDialogueNode();
    }

    public void SelectOption(DialogueOption selectedOption)
    {
        Debug.Log("Current node: " + currentNode.dialogueText);
        Debug.Log("Selected option: " + selectedOption.optionText + " nextNode: " + selectedOption.nextNode + " fallbackNode: " + selectedOption.fallbackNode); 
        if (selectedOption.nextNode == null)
        {
            EndDialogue();
            return;
        }

        if (selectedOption.AreConditionsMet())
        {
            currentNode = selectedOption.nextNode;
        }
        else
        {
            Debug.Log("Conditions not met.");
            currentNode = selectedOption.fallbackNode;
        }
        DisplayCurrentDialogueNode();
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
        OnDialogueEnd?.Invoke();
        dialogueUI.HideDialogue();
    }
}
