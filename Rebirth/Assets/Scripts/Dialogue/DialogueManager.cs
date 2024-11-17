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

    private int currentOptionIndex = 0;
    private bool isProcessingSelection = false;

    public static DialogueManager Instance { get; private set; }
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

    public void NavigateOption(int direction)
    {
        if (currentNode == null || currentNode.options.Count == 0)
        {
            Debug.LogError("No current node.");
            return;
        }

        currentOptionIndex = (currentOptionIndex + direction + currentNode.options.Count) % currentNode.options.Count;

        dialogueUI.UpdateSelectedOption(currentOptionIndex);
    }

    public void SelectCurrentOption()
    {
        if (currentNode == null)
        {
            Debug.LogError("No current node.");
            return;
        }

        if (currentNode.options.Count == 0)
        {
            // leaf node
            EndDialogue();
            return;
        }

        SelectOption(currentOptionIndex);
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
        OnDialogueStart?.Invoke();
        DisplayCurrentDialogueNode();
    }

    public void SelectOption(DialogueOption selectedOption)
    {
        if (isProcessingSelection)
        {
            return;
        }
        isProcessingSelection = true;

        if (selectedOption == null)
        {
            EndDialogue();
            return;
        }

        selectedOption.onSelectActions?.Invoke();

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

        isProcessingSelection = false;
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
        currentNode = null;
        OnDialogueEnd?.Invoke();
        dialogueUI.HideDialogue();
    }
}
