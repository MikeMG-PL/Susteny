using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Interactable))]
[CanEditMultipleObjects]
public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
        Interactable interactable = (Interactable)target;

        EditorGUILayout.Space(3f);
        if (interactable.GetComponent<Hints>() == null)
        {
            if (GUILayout.Button("Add hints")) interactable.gameObject.AddComponent<Hints>();
        }

        else
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.normal.textColor = new Color(0.6f, 0.0f, 0.2f);
            style.fontStyle = FontStyle.Bold;
            if (GUILayout.Button("Remove hints", style))
            {
                if (EditorUtility.DisplayDialog("Delete hints?", "Are you sure you want to delete hints from this object?", "Yes", "No"))
                    DestroyImmediate(interactable.GetComponent<Hints>());
            }
        }
        EditorGUILayout.Space(3f);

        EditorGUILayout.Space(3f);
        if (interactable.GetComponent<ManipulatePlayer>() == null)
        {
            if (GUILayout.Button("Add player position or rotation")) interactable.gameObject.AddComponent<ManipulatePlayer>();
        }

        else
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.normal.textColor = new Color(0.6f, 0.0f, 0.2f);
            style.fontStyle = FontStyle.Bold;
            if (GUILayout.Button("Remove player position and rotation", style))
            {
                if (EditorUtility.DisplayDialog("Delete manipulation of player?", "Are you sure you want to player manipulation from this object?", "Yes", "No"))
                    DestroyImmediate(interactable.GetComponent<ManipulatePlayer>());
            }
        }
        EditorGUILayout.Space(3f);

        serializedObject.ApplyModifiedProperties();
    }
}

[CustomEditor(typeof(DialogueInteraction))]
[CanEditMultipleObjects]
public class DialogueInteractionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
        DialogueInteraction speaker = (DialogueInteraction)target;

        EditorGUILayout.Space(3f);
        if (speaker.GetComponent<Hints>() == null)
        {
            if (GUILayout.Button("Add hints")) speaker.gameObject.AddComponent<Hints>();
        }

        else
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.normal.textColor = new Color(0.6f, 0.0f, 0.2f);
            style.fontStyle = FontStyle.Bold;
            if (GUILayout.Button("Remove hints", style))
            {
                if (EditorUtility.DisplayDialog("Delete hints?", "Are you sure you want to delete hints from this object?", "Yes", "No"))
                    DestroyImmediate(speaker.GetComponent<Hints>());
            }
        }
        EditorGUILayout.Space(3f);

        EditorGUILayout.Space(3f);
        if (speaker.GetComponent<ManipulatePlayer>() == null)
        {
            if (GUILayout.Button("Add player position or rotation")) speaker.gameObject.AddComponent<ManipulatePlayer>();
        }

        else
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.normal.textColor = new Color(0.6f, 0.0f, 0.2f);
            style.fontStyle = FontStyle.Bold;
            if (GUILayout.Button("Remove player position and rotation", style))
            {
                if (EditorUtility.DisplayDialog("Delete manipulation of player?", "Are you sure you want to player manipulation from this object?", "Yes", "No"))
                    DestroyImmediate(speaker.GetComponent<ManipulatePlayer>());
            }
        }
        EditorGUILayout.Space(3f);

        serializedObject.ApplyModifiedProperties();
    }
}
