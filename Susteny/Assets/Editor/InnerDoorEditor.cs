using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InnerDoor))]
[CanEditMultipleObjects]
public class InnerDoorEditor : Editor
{
    SerializedProperty defaultDestination;
    SerializedProperty lastPosition;
    SerializedProperty alwaysReturnToLastPosition;

    private void OnEnable()
    {
        defaultDestination = serializedObject.FindProperty("defaultDestination");
        lastPosition = serializedObject.FindProperty("lastPosition");
        alwaysReturnToLastPosition = serializedObject.FindProperty("alwaysReturnToLastPosition");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        InnerDoor instance = (InnerDoor)target;

        EditorGUILayout.PropertyField(alwaysReturnToLastPosition, new GUIContent("Walk to the object", "If the transform is null, the default position will be chosen"));

        if (!instance.alwaysReturnToLastPosition)
            EditorGUILayout.PropertyField(defaultDestination, new GUIContent("Default outer destination", "Player will always be teleported to this destination when leaving a building"));

        serializedObject.ApplyModifiedProperties();
    }
}
