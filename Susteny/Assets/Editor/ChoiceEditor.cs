using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using Subtegral.DialogueSystem.DataContainers;

[CustomEditor(typeof(ChoiceManager))]
[CanEditMultipleObjects]
public class ChoiceEditor : Editor
{
    SerializedProperty dialogues;
    SerializedProperty choices;
    SerializedProperty triggeredAction;
    SerializedProperty actions;

    private void OnEnable()
    {
        dialogues = serializedObject.FindProperty("dialogues");
        choices = serializedObject.FindProperty("choices");
        triggeredAction = serializedObject.FindProperty("triggeredAction");
        actions = serializedObject.FindProperty("actions");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //DrawDefaultInspector();
        ChoiceManager choiceManager = (ChoiceManager)target;

        EditorGUILayout.PropertyField(dialogues, new GUIContent("Important dialogues", "Dialogues that cause events/are influenced by events"), true);
        EditorGUILayout.Space(3);

        if(dialogues.arraySize > 0)
        {
            for (int i = 0; i < dialogues.arraySize; i++)
            {
                // Creating list of actions
                if(choiceManager.actions == null)
                    choiceManager.actions = new List<UnityEvent>();

                choiceManager.actions.Add(new UnityEvent());

                if (dialogues.GetArrayElementAtIndex(i).objectReferenceValue != null)
                {
                    var container = dialogues.GetArrayElementAtIndex(i).objectReferenceValue;
                    GenerateDialogueChoices((DialogueContainer)container);
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    void GenerateDialogueChoices(DialogueContainer container)
    {
        // Bold style
        GUIStyle style1 = new GUIStyle(GUI.skin.label);
        style1.wordWrap = true;
        style1.fontStyle = FontStyle.Bold;
        style1.fontSize = 15;

        // Header style
        GUIStyle header = new GUIStyle(GUI.skin.label);
        header.wordWrap = true;
        header.fontStyle = FontStyle.Bold;
        header.alignment = TextAnchor.MiddleCenter;
        header.fontSize = 20;

        // Header of dialogue
        GUILayout.Label(container.name, header);
        EditorGUILayout.Space(1);

        // Generating choice UnityEvents
        for (int i = 1; i < container.NodeLinks.Count; i++)
        {
            GUILayout.Label(container.NodeLinks[i].PortName, style1);
            EditorGUILayout.Space(1);

            // i == 1: Pominięcie "Nexta", pierwsza opcja z listy i odpowiadający jej UnityEvent
            EditorGUILayout.PropertyField(actions.GetArrayElementAtIndex(i - 1), new GUIContent($"Base node: {container.NodeLinks[i].BaseNodeGUID}"));
            EditorGUILayout.Space(5);
        }
    }
}
