using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ManipulatePlayer))]
[CanEditMultipleObjects]
public class ManipulatePlayerEditor : Editor
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
        //DrawDefaultInspector();

        ManipulatePlayer instance = (ManipulatePlayer)target;

        EditorGUILayout.PropertyField(enableGoTo, new GUIContent("Walk to the object", "If the transform is null, the default position will be chosen"));

        if (instance.enableGoTo)
        {
            EditorGUILayout.PropertyField(positionToGo, new GUIContent("Transform", "Player will stand at this position during interaction"));

            EditorGUILayout.Space(3f);

            EditorGUILayout.PropertyField(moveSpeed, new GUIContent("Move speed", "Speed the player will have while moving to the object"));

            EditorGUILayout.Space(3f);

            EditorGUILayout.PropertyField(distance, new GUIContent("Default distance multiplier", "How many times is the distance between player and object longer than default"));

            EditorGUILayout.Space(3f);
        }

        EditorGUILayout.PropertyField(enableLookAt, new GUIContent("Look at object", "Leave transform null, if you want player to look at interactable obj itself"));

        if (instance.enableLookAt)
        {
            EditorGUILayout.PropertyField(objectToLookAt, new GUIContent("Transform", "Player will look at the center of this object during interaction"));

            EditorGUILayout.Space(3f);

            EditorGUILayout.PropertyField(lookSpeed, new GUIContent("Look speed", "Speed the player will look at the object"));

            EditorGUILayout.Space(3f);
        }


        if (instance.enableGoTo || instance.enableLookAt)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.wordWrap = true;
            GUILayout.Label("Cursor enabled after the player has stood in a given position or looked at a given object", style);
            EditorGUILayout.PropertyField(cursorOnWhenOnPosition, new GUIContent("Enable cursor delay"));
            EditorGUILayout.Space(3f);
        }
        else cursorOnWhenOnPosition.boolValue = false;

        serializedObject.ApplyModifiedProperties();
    }
}
