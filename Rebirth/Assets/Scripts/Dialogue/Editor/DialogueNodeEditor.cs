using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueNode), true)]
public class DialogueNodeEditor : Editor
{
    // Serialized properties
    SerializedProperty nodeIDProp;
    SerializedProperty dialogueTextProp;
    SerializedProperty optionsProp;

    // PlainNextDialogueNode properties
    SerializedProperty nextNodeProp;

    // ConditionDialogueNode properties
    SerializedProperty conditionMetNextNodeProp;
    SerializedProperty conditionNotMetNextNodeProp;
    SerializedProperty conditionsProp;

    // QuestDialogueNode properties
    SerializedProperty questProp;

    private void OnEnable()
    {
        nodeIDProp = serializedObject.FindProperty("nodeID");
        dialogueTextProp = serializedObject.FindProperty("dialogueText");
        optionsProp = serializedObject.FindProperty("options");

        // PlainNextDialogueNode properties
        nextNodeProp = serializedObject.FindProperty("nextNode");

        // ConditionDialogueNode properties
        conditionMetNextNodeProp = serializedObject.FindProperty("conditionMetNextNode");
        conditionNotMetNextNodeProp = serializedObject.FindProperty("conditionNotMetNextNode");
        conditionsProp = serializedObject.FindProperty("conditions");

        // QuestDialogueNode properties
        questProp = serializedObject.FindProperty("associatedQuest");
    }

    public override void OnInspectorGUI()
    {
        // Update the serialized object
        serializedObject.Update();

        // Draw the common fields
        EditorGUILayout.PropertyField(nodeIDProp);
        EditorGUILayout.PropertyField(dialogueTextProp);

        // Determine if the current target
        if (target is PlainNextDialogueNode)
        {
            EditorGUILayout.PropertyField(nextNodeProp);
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Options are managed automatically for PlainNextDialogueNode.", MessageType.Info);
        }
        else if (target is ConditionDialogueNode)
        {
            EditorGUILayout.PropertyField(conditionMetNextNodeProp);
            EditorGUILayout.PropertyField(conditionNotMetNextNodeProp);
            EditorGUILayout.PropertyField(conditionsProp, true);
        }
        else if (target is LeafDialogueNode)
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("LeafDialogueNode has no options.", MessageType.Info);
        }
        else if (target is QuestDialogueNode)
        {
            EditorGUILayout.PropertyField(questProp);
            // EditorGUILayout.PropertyField(optionsProp, true);
        }
        else
        {
            // default DialogueNode
            EditorGUILayout.PropertyField(optionsProp, true);
        }

        // Apply changes to the serialized object
        serializedObject.ApplyModifiedProperties();
    }
}
