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
    SerializedProperty choiceEvents;
    SerializedProperty dialogues;
    SerializedProperty unityEventList;
    ChoiceManager c;

    private void OnEnable()
    {
        choiceEvents = serializedObject.FindProperty("choiceEvents");
        dialogues = serializedObject.FindProperty("dialogues");
        unityEventList = serializedObject.FindProperty("unityEventList");
        c = (ChoiceManager)target;
    }

    public void OnValidate()
    {
        GenerateDialogueChoices();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        c = (ChoiceManager)target;

        if (GUILayout.Button("Generate dialogue choices") && c.dialogues.Count > 0 && !c.dialogues.Contains(null))
        {
            GenerateDialogueChoices();
        }
        GenerateDialogueChoices();

        EditorGUILayout.PropertyField(dialogues, new GUIContent("Important dialogues", "Dialogues that cause events"), true);
        EditorGUILayout.Space(5);

        if (c.choiceEvents != null && c.choiceEvents.Count > 0 && c.dialogues.Count > 0 && !c.dialogues.Contains(null))
        {
            Display();
            //Debug.Log(c.choiceEvents.Count);
        }

        serializedObject.ApplyModifiedProperties();
    }

    void GenerateDialogueChoices()
    {
        c.choiceEvents = new List<ChoiceEvent>();
        c.unityEventList = new List<UnityEvent>();

        if (c.dialogues.Count > 0 && !c.dialogues.Contains(null))
        {
            c.choiceEvents = new List<ChoiceEvent>();
            for (int i = 0; i < c.dialogues.Count; i++)
            {
                for (int j = 1; j < c.dialogues[i].NodeLinks.Count; j++)
                {
                    var d = c.dialogues[i];
                    c.choiceEvents.Add(new ChoiceEvent
                    {
                        container = d,
                        baseGUID = d.NodeLinks[j].BaseNodeGUID,
                        text = d.NodeLinks[j].PortName,
                        evt = new UnityEvent(),
                    });
                }
            }
        }

        UnityEvent e = new UnityEvent();
        foreach (ChoiceEvent cev in c.choiceEvents)
        {
            e = cev.evt;
            if (c.unityEventList == null)
                c.unityEventList = new List<UnityEvent>();
            c.unityEventList.Add(e);
        }
    }

    void Display()
    {
        // Bold style
        GUIStyle style1 = new GUIStyle(GUI.skin.label)
        {
            wordWrap = true,
            fontStyle = FontStyle.Bold,
            fontSize = 15
        };

        // Header style
        GUIStyle header = new GUIStyle(GUI.skin.label)
        {
            wordWrap = true,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 20
        };

        ///////////////////////////////////////////

        foreach (DialogueContainer d in c.dialogues)
        {
            GUILayout.Label(d.name, header);
            GUILayout.Space(5);
            for (int i = 1; i < d.NodeLinks.Count; i++)
            {
                GUILayout.Label(d.NodeLinks[i].PortName, style1);
                GUILayout.Space(5);

                if(i <= c.unityEventList.Count && c.unityEventList.Count > 0 && unityEventList.GetArrayElementAtIndex(i-1) != null)
                {
                    //Debug.Log($"i: {i}, i-1: {i-1}, {d.NodeLinks[i].PortName}, {unityEventList.GetArrayElementAtIndex(i - 1)}");
                    EditorGUILayout.PropertyField(unityEventList.GetArrayElementAtIndex(i - 1), new GUIContent($"Base node GUID: {d.NodeLinks[i].BaseNodeGUID}"));
                }
                    

                GUILayout.Space(5);
            }
        }
    }

    void ImportEvent()
    {
        

        /*UnityEvent e = new UnityEvent();
        foreach (ChoiceEvent cev in c.choiceEvents)
        {
            if (baseGUID == cev.baseGUID && text == cev.text)
            {
                e = cev.evt;
                if (c.unityEventList == null)
                    c.unityEventList = new List<UnityEvent>();
                c.unityEventList.Add(e);
            }
        }*/
    }
}
