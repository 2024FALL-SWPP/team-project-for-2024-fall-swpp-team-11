// using UnityEngine;
// using UnityEditor;

// [CustomEditor(typeof(DialogueNode), true)]
// public class DialogueNodeEditor : Editor
// {
//     // Serialized properties
//     SerializedProperty nodeIDProp;
//     SerializedProperty dialogueTextProp;
//     SerializedProperty optionsProp;

//     private void OnEnable()
//     {
//         nodeIDProp = serializedObject.FindProperty("nodeID");
//         dialogueTextProp = serializedObject.FindProperty("dialogueText");
//         optionsProp = serializedObject.FindProperty("options");
//     }

//     public override void OnInspectorGUI()
//     {
//         // Update the serialized object
//         serializedObject.Update();

//         // Draw the common fields
//         EditorGUILayout.PropertyField(nodeIDProp);
//         EditorGUILayout.PropertyField(dialogueTextProp);

//         // Determine if the current target is not PlainNextDialogueNode
//         if (target is not PlainNextDialogueNode)
//         {
//             EditorGUILayout.PropertyField(optionsProp, true);
//         }
//         else
//         {
//             // Optionally, you can add a space or a note indicating that options are hidden
//             EditorGUILayout.Space();
//             EditorGUILayout.HelpBox("Options are managed automatically for PlainNextDialogueNode.", MessageType.Info);
//         }

//         // Apply changes to the serialized object
//         serializedObject.ApplyModifiedProperties();
//     }
// }
