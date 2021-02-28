using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using UnityEditor.SceneManagement;
using Subtegral.DialogueSystem.DataContainers;

[CustomEditor(typeof(ChoiceManager))]
[CanEditMultipleObjects]
public class ChoiceEditor : Editor
{
    SerializedProperty dialogue;
    SerializedProperty choiceList;
    SerializedProperty canView;
    GUIStyle style1;
    GUIStyle header;
    ChoiceManager c;

    private void OnEnable()
    {
        dialogue = serializedObject.FindProperty("dialogue");
        choiceList = serializedObject.FindProperty("choiceList");
        canView = serializedObject.FindProperty("canView");
    }

    public override void OnInspectorGUI()
    {
        // Bold style
        style1 = new GUIStyle(GUI.skin.label)
        {
            wordWrap = true,
            fontStyle = FontStyle.Bold,
            fontSize = 15
        };

        // Header style
        header = new GUIStyle(GUI.skin.label)
        {
            wordWrap = true,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 20
        };

        ////////////////////////////////////////

        serializedObject.Update();
        c = (ChoiceManager)target;

        if (c.dialogue != null)
        {
            if (GUILayout.Button("Generate choices"))
            {
                Generate();
                canView.boolValue = true;
            }
        }

        Space(15);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(dialogue, new GUIContent("Important dialogue", "Dialogue that causes events"), true);
        if (EditorGUI.EndChangeCheck())
            canView.boolValue = false;

        Space(15);

        //if (c.dialogue != null && c.actions.Count > 0 && canView)
        if (c.dialogue != null && canView.boolValue)
            Display();

        serializedObject.ApplyModifiedProperties();
    }

    void Generate()
    {
        //c.actions = new List<UnityEvent>();
        choiceList.ClearArray();

        if (c.dialogue != null)
        {
            for (int i = 1; i < c.dialogue.NodeLinks.Count; i++)
            {
                choiceList.InsertArrayElementAtIndex(i - 1);
                //Debug.Log(dialogue.FindPropertyRelative(""));
                choiceList.GetArrayElementAtIndex(i - 1).FindPropertyRelative("optionText").stringValue =
                c.dialogue.NodeLinks[i].PortName;

                choiceList.GetArrayElementAtIndex(i - 1).FindPropertyRelative("baseGUID").stringValue =
                c.dialogue.NodeLinks[i].BaseNodeGUID;
            }
        }
    }

    void Display()
    {
        /*for (int i = 1; i < c.dialogue.NodeLinks.Count; i++)
        {
            GUILayout.Label($"Element {i - 1}: {c.dialogue.NodeLinks[i].PortName}", style1);
            Space(15);
        }
        EditorGUILayout.PropertyField(actions, new GUIContent($"{c.dialogue.name}"));*/
        EditorGUILayout.PropertyField(choiceList);
    }

    void Space(float p)
    {
        GUILayout.Space(p);
    }
}
