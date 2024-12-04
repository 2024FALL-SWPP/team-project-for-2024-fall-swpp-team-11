using UnityEngine;
using System.Collections.Generic;
using System;

public class DialogueManager : SingletonManager<DialogueManager>
{
    // public DialogueNode startNode;

    [HideInInspector]
    public DialogueNode currentNode;
    public DialogueNode lastLeafNode;
    public DialogueUI dialogueUI;
    // public DialogueGraph activeDialogueGraph;

    public event Action OnDialogueStart;
    public event Action OnDialogueEnd;

    private int currentOptionIndex = 0;
    private bool isProcessingSelection = false;

    private static string logPrefix = "[DialogueManager] ";

    protected override void Awake()
    {
        base.Awake();

        if (dialogueUI == null)
        {
            Debug.LogError(logPrefix + "DialogueUI not found.");
        }
    }

    private void Start()
    {
        // dialogueUI = GetComponent<DialogueUI>(); // DialogueUI를 어디에 둘 것인가

        // TODO default graph
        // if (activeDialogueGraph == null)
        // {
        //     LoadDefaultDialogueGraph();
        // }
    }

    public void NavigateOption(int direction)
    {
        if (currentNode == null || currentNode.options.Count == 0)
        {
            Debug.LogError(logPrefix + "No current node.");
            return;
        }

        currentOptionIndex = (currentOptionIndex + direction + currentNode.options.Count) % currentNode.options.Count;

        dialogueUI.UpdateSelectedOption(currentOptionIndex);
    }

    public void SelectCurrentOption()
    {
        if (currentNode == null)
        {
            Debug.LogError(logPrefix + "No current node.");
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

    // private void LoadDefaultDialogueGraph()
    // {
    //     DialogueGraph defaultGraph = Resources.Load<DialogueGraph>("DefaultDialogueGraph");
    //     if (defaultGraph != null)
    //     {
    //         LoadDialogueGraph(defaultGraph);
    //     }
    //     else
    //     {
    //         Debug.LogWarning(logPrefix + "DefaultDialogueGraph not found.");
    //     }
    // }

    // public void LoadDialogueGraph(DialogueGraph graph)
    // {
    //     activeDialogueGraph = graph;
    //     Debug.Log(logPrefix + "Loaded dialogue graph.");
    //     // TODO validate graph
    // }

//     public static void EditorLoadDialogueGraph(DialogueGraph graph)
//     {
// #if UNITY_EDITOR
//         if (Instance != null)
//         {
//             Instance.LoadDialogueGraph(graph);
//             Debug.Log(logPrefix + "Editor loaded dialogue graph.");
//         }
//         else
//         {
//             Debug.LogWarning(logPrefix + "DialogueManager not found.");
//         }
// #endif
//     }

    public void StartDialogue(NPC npc)
    {
        // if (activeDialogueGraph == null)
        // {
        //     Debug.LogError(logPrefix + "No active dialogue graph.");
        //     return;
        // }

        DialogueNode startingNode = GetDialogueNodeFromNPC(npc);

        // if (!activeDialogueGraph.dialogueNodes.Contains(startingNode))
        // {
        //     Debug.LogWarning(logPrefix + "Starting node not in active dialogue graph.");
        // }

        SetCurrentNode(startingNode);
        OnDialogueStart?.Invoke();
        DisplayCurrentDialogueNode();
    }

    private DialogueNode GetDialogueNodeFromNPC(NPC npc)
    {
        // check quest status
        QuestData relatedQuest = npc.questNodes.questData;
        QuestStatus questStatus = QuestManager.Instance.GetQuestStatus(relatedQuest);
        Debug.Log(logPrefix + "Quest Title: " + relatedQuest.questTitle + ", Status: " + questStatus);

        DialogueNode startingNode = npc.questNodes.GetQuestNodeOrDefault(questStatus);
        startingNode.conversationNpcName = npc.npcName;
        return startingNode;
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
            // leaf node TODO: better management considering both no option leaf nodes and this case
            EndDialogue();
            return;
        }

        if (selectedOption.AreConditionsMet())
        {
            TransitionToNode(selectedOption.nextNode);
        }
        else
        {
            TransitionToNode(selectedOption.fallbackNode);
        }
        DisplayCurrentDialogueNode();

        isProcessingSelection = false;
    }

    public void SelectOption(int optionIndex)
    {
        if (optionIndex < 0 || optionIndex >= currentNode.options.Count)
        {
            Debug.LogError(logPrefix + "Invalid option index :" + optionIndex + " valid range: 0-" + (currentNode.options.Count - 1));
            return;
        }

        DialogueOption selectedOption = currentNode.options[optionIndex];
        SelectOption(selectedOption);
    }

    private void DisplayCurrentDialogueNode()
    {
        dialogueUI.ShowDialogue(currentNode);
    }

    private void TransitionToNode(DialogueNode nextNode)
    {
        Debug.Log(logPrefix + "Transitioning to node: " + nextNode.name);
        nextNode.conversationNpcName = currentNode.conversationNpcName; // keep the same speaker
        currentOptionIndex = 0;
        currentNode = nextNode;
    }

    private void SetCurrentNode(DialogueNode node)
    {
        currentNode = node;
    }

    private void ResetCurrentNode()
    {
        currentNode = null;
    }

    public DialogueNode getLastLeafNode()
    {
        return lastLeafNode;
    }

    private void EndDialogue()
    {
        lastLeafNode = currentNode;
        ResetCurrentNode();
        OnDialogueEnd?.Invoke();
        dialogueUI.HideDialogue();
        
    }
}
