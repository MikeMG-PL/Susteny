using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Subtegral.DialogueSystem.DataContainers;

[CustomEditor(typeof(ChoiceManager))]
[CanEditMultipleObjects]
public class ChoiceEditor : Editor
{
    SerializedProperty dialogues;
    SerializedProperty choices;
    SerializedProperty triggeredAction;

    private void OnEnable()
    {
        dialogues = serializedObject.FindProperty("dialogues");
        choices = serializedObject.FindProperty("choices");
        triggeredAction = serializedObject.FindProperty("triggeredAction");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //DrawDefaultInspector();
        ChoiceManager choiceManager = (ChoiceManager)target;

        EditorGUILayout.PropertyField(dialogues, new GUIContent("Important dialogues", "Dialogues that cause events/are influenced by events"), true);

        EditorGUILayout.Space(1);

        if(dialogues.arraySize > 0)
        {
            for(int i = 0; i < dialogues.arraySize; i++)
            {
                if (dialogues.GetArrayElementAtIndex(i) != null)
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
        // DZIAŁA :O
        ; // tu coś będzie
    }
}
