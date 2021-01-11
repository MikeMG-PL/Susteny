using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Interactable))]
[CanEditMultipleObjects]
public class InteractableEditor : Editor
{
    SerializedProperty positionToGo;
    SerializedProperty objectToLookAt;
    SerializedProperty cursorOnWhenOnPosition;
    SerializedProperty enableLookAt;
    SerializedProperty enableGoTo;
    SerializedProperty moveSpeed;
    SerializedProperty lookSpeed;
    SerializedProperty distance;

    private void OnEnable()
    {
        positionToGo = serializedObject.FindProperty("positionToGo");
        objectToLookAt = serializedObject.FindProperty("objectToLookAt");
        cursorOnWhenOnPosition = serializedObject.FindProperty("cursorOnWhenOnPosition");
        enableLookAt = serializedObject.FindProperty("enableLookAt");
        enableGoTo = serializedObject.FindProperty("enableGoTo");
        moveSpeed = serializedObject.FindProperty("moveSpeed");
        lookSpeed = serializedObject.FindProperty("lookSpeed");
        distance = serializedObject.FindProperty("distance");
    }

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
            if (GUILayout.Button("Remove hints", style)) DestroyImmediate(interactable.GetComponent<Hints>());
        }
        EditorGUILayout.Space(3f);

        EditorGUILayout.PropertyField(enableGoTo, new GUIContent("Walk to the object", "If the transform is null, the default position will be chosen"));

        if (interactable.enableGoTo)
        {
            EditorGUILayout.PropertyField(positionToGo, new GUIContent("Transform", "Player will stand at this position during interaction"));

            EditorGUILayout.Space(3f);

            EditorGUILayout.PropertyField(moveSpeed, new GUIContent("Move speed", "Speed the player will have while moving to the object"));

            EditorGUILayout.Space(3f);

            EditorGUILayout.PropertyField(distance, new GUIContent("Default distance multiplier", "How many times is the distance between player and object longer than default"));

            EditorGUILayout.Space(3f);
        }

        EditorGUILayout.PropertyField(enableLookAt, new GUIContent("Look at object", "Leave transform null, if you want player to look at interactable obj itself"));

        if (interactable.enableLookAt)
        {
            EditorGUILayout.PropertyField(objectToLookAt, new GUIContent("Transform", "Player will look at the center of this object during interaction"));

            EditorGUILayout.Space(3f);

            EditorGUILayout.PropertyField(lookSpeed, new GUIContent("Look speed", "Speed the player will look at the object"));

            EditorGUILayout.Space(3f);
        }


        if (interactable.enableGoTo || interactable.enableLookAt)
        {
            EditorGUILayout.PropertyField(cursorOnWhenOnPosition, new GUIContent("Cursor enabled after player on position"));
            EditorGUILayout.Space(3f);
        }
        else cursorOnWhenOnPosition.boolValue = false;

        serializedObject.ApplyModifiedProperties();
    }
}

[CustomEditor(typeof(DialogueInteraction))]
[CanEditMultipleObjects]
public class DialogueInteractionEditor : Editor
{
    SerializedProperty positionToGo;
    SerializedProperty objectToLookAt;
    SerializedProperty enableLookAt;
    SerializedProperty enableGoTo;
    SerializedProperty moveSpeed;
    SerializedProperty lookSpeed;
    SerializedProperty distance;
    SerializedProperty interactionHint;

    private void OnEnable()
    {
        positionToGo = serializedObject.FindProperty("positionToGo");
        objectToLookAt = serializedObject.FindProperty("objectToLookAt");
        enableLookAt = serializedObject.FindProperty("enableLookAt");
        enableGoTo = serializedObject.FindProperty("enableGoTo");
        moveSpeed = serializedObject.FindProperty("moveSpeed");
        lookSpeed = serializedObject.FindProperty("lookSpeed");
        distance = serializedObject.FindProperty("distance");
        interactionHint = serializedObject.FindProperty("interactionHint");
    }

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
            if (GUILayout.Button("Remove hints", style)) DestroyImmediate(speaker.GetComponent<Hints>());
        }
        EditorGUILayout.Space(3f);

        EditorGUILayout.PropertyField(enableGoTo, new GUIContent("Walk to the object", "If the transform is null, the default position will be chosen"));

        if (speaker.enableGoTo)
        {
            EditorGUILayout.PropertyField(positionToGo, new GUIContent("Transform", "Player will stand at this position during interaction"));

            EditorGUILayout.Space(3f);

            EditorGUILayout.PropertyField(moveSpeed, new GUIContent("Move speed", "Speed the player will have while moving to the object"));

            EditorGUILayout.Space(3f);

            EditorGUILayout.PropertyField(distance, new GUIContent("Default distance multiplier", "How many times is the distance between player and object longer than default"));

            EditorGUILayout.Space(3f);
        }

        EditorGUILayout.PropertyField(enableLookAt, new GUIContent("Look at object", "Leave transform null, if you want player to look at interactable obj itself"));

        if (speaker.enableLookAt)
        {
            EditorGUILayout.PropertyField(objectToLookAt, new GUIContent("Transform", "Player will look at the center of this object during interaction"));

            EditorGUILayout.Space(3f);

            EditorGUILayout.PropertyField(lookSpeed, new GUIContent("Look speed", "Speed the player will look at the object"));

            EditorGUILayout.Space(3f);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
