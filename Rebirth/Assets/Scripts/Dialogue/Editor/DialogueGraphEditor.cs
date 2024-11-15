// Assets/Scripts/Editor/DialogueGraphEditor.cs
using UnityEngine;
using UnityEditor;
using System.IO;

public class DialogueGraphEditor : EditorWindow
{
    private DialogueGraph dialogueGraph;
    private Vector2 scrollPos;

    // Add menu named "Dialogue Graph Editor" to the Window menu
    [MenuItem("Window/Dialogue Graph Editor")]
    public static void ShowWindow()
    {
        GetWindow<DialogueGraphEditor>("Dialogue Graph Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Dialogue Graph Editor", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // Select DialogueGraph Asset
        dialogueGraph = (DialogueGraph)EditorGUILayout.ObjectField("Dialogue Graph", dialogueGraph, typeof(DialogueGraph), false);

        EditorGUILayout.Space();

        if (dialogueGraph != null)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Dialogue Node"))
            {
                CreateDialogueNode();
            }

            if (GUILayout.Button("Save Dialogue Graph"))
            {
                EditorUtility.SetDirty(dialogueGraph);
                AssetDatabase.SaveAssets();
                EditorUtility.DisplayDialog("Saved", "Dialogue Graph has been saved.", "OK");

                // Inform DialogueManager to reload the Dialogue Graph
                DialogueManager.EditorLoadDialogueGraph(dialogueGraph);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            // Display list of DialogueNodes
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            for (int i = 0; i < dialogueGraph.dialogueNodes.Count; i++)
            {
                DialogueNode node = dialogueGraph.dialogueNodes[i];
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label($"Node {i + 1}: {node.name}", EditorStyles.boldLabel);
                if (GUILayout.Button("Select", GUILayout.Width(60)))
                {
                    Selection.activeObject = node;
                }
                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    RemoveDialogueNode(node);
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Label(node.dialogueText, EditorStyles.wordWrappedLabel);
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndScrollView();
        }
        else
        {
            EditorGUILayout.HelpBox("Please assign a Dialogue Graph to manage its Dialogue Nodes.", MessageType.Info);
        }
    }

    private void CreateDialogueNode()
    {
        // Create a new DialogueNode asset
        DialogueNode newNode = CreateInstance<DialogueNode>();
        string path = AssetDatabase.GetAssetPath(dialogueGraph);
        if (string.IsNullOrEmpty(path))
        {
            path = "Assets/ScriptableObjects/Dialogue/";
        }
        else
        {
            path = path.Replace(Path.GetFileName(path), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New Dialogue Node.asset");

        AssetDatabase.CreateAsset(newNode, assetPathAndName);
        AssetDatabase.SaveAssets();

        // Add to DialogueGraph
        dialogueGraph.AddDialogueNode(newNode);
        EditorUtility.SetDirty(dialogueGraph);

        // Focus on the new node
        Selection.activeObject = newNode;
    }

    private void RemoveDialogueNode(DialogueNode node)
    {
        if (EditorUtility.DisplayDialog("Remove Dialogue Node",
            $"Are you sure you want to remove '{node.name}' from the Dialogue Graph?",
            "Yes", "No"))
        {
            dialogueGraph.RemoveDialogueNode(node);
            EditorUtility.SetDirty(dialogueGraph);

            // Optionally delete the asset
            string assetPath = AssetDatabase.GetAssetPath(node);
            if (!string.IsNullOrEmpty(assetPath))
            {
                AssetDatabase.DeleteAsset(assetPath);
            }

            AssetDatabase.SaveAssets();

            if (DialogueManager.Instance != null && DialogueManager.Instance.activeDialogueGraph == dialogueGraph)
            {
                DialogueManager.EditorLoadDialogueGraph(dialogueGraph);
            }
        }
    }

    private void OnSelectionChange()
    {
        // Optional: Handle selection changes if needed
        Repaint();
    }
}
