using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueNode), true)]
public class DialogueNodeEditor : Editor
{
    // Serialized properties
    SerializedProperty nodeIDProp;
    SerializedProperty dialogueTextProp;
    SerializedProperty optionsProp;

    // ConditionDialogueNode properties
    SerializedProperty conditionMetNodeProp;
    SerializedProperty conditionNotMetNodeProp;
    SerializedProperty conditionsProp;

    private void OnEnable()
    {
        nodeIDProp = serializedObject.FindProperty("nodeID");
        dialogueTextProp = serializedObject.FindProperty("dialogueText");
        optionsProp = serializedObject.FindProperty("options");

        // ConditionDialogueNode properties
        conditionMetNodeProp = serializedObject.FindProperty("conditionMetNode");
        conditionNotMetNodeProp = serializedObject.FindProperty("conditionNotMetNode");
        conditionsProp = serializedObject.FindProperty("conditions");
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
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Options are managed automatically for PlainNextDialogueNode.", MessageType.Info);
        }
        else if (target is ConditionDialogueNode)
        {
            EditorGUILayout.PropertyField(conditionMetNodeProp);
            EditorGUILayout.PropertyField(conditionNotMetNodeProp);
            EditorGUILayout.PropertyField(conditionsProp, true);
        }
        else if (target is LeafDialogueNode)
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("LeafDialogueNode has no options.", MessageType.Info);
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
