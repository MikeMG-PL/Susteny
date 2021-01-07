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

    private void OnEnable()
    {
        positionToGo = serializedObject.FindProperty("positionToGo");
        objectToLookAt = serializedObject.FindProperty("objectToLookAt");
        cursorOnWhenOnPosition = serializedObject.FindProperty("cursorOnWhenOnPosition");
        enableLookAt = serializedObject.FindProperty("enableLookAt");
        enableGoTo = serializedObject.FindProperty("enableGoTo");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
        Interactable interactable = (Interactable)target;

        EditorGUILayout.PropertyField(enableGoTo, new GUIContent("Walk to the object", "If the transform is null, the default position will be chosen"));

        if (interactable.enableGoTo)
        {
            EditorGUILayout.PropertyField(positionToGo, new GUIContent("Transform", "Player will stand at this position during interaction"));

            EditorGUILayout.Space(3f);
        }

        EditorGUILayout.PropertyField(enableLookAt, new GUIContent("Look at object", "Leave transform null, if you want player to look at interactable obj itself"));

        if (interactable.enableLookAt)
        {
            EditorGUILayout.PropertyField(objectToLookAt, new GUIContent("Transform", "Player will look at the center of this object during interaction"));

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
