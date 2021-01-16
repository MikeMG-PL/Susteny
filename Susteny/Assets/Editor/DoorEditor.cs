#if UNITY_EDITOR
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Door))]
[CanEditMultipleObjects]
public class DoorEditor : Editor
{
    SerializedProperty defaultDestination;
    SerializedProperty teleportCameraRotation;
    SerializedProperty ID;
    SerializedProperty sceneName; SerializedProperty sceneID;
    SerializedProperty doorMode;
    SerializedProperty nameOrID;
    SerializedProperty rotType;
    SerializedProperty unlocked;

    private void OnEnable()
    {
        defaultDestination = serializedObject.FindProperty("defaultDestination");
        teleportCameraRotation = serializedObject.FindProperty("teleportCameraRotation");

        doorMode = serializedObject.FindProperty("doorMode");
        nameOrID = serializedObject.FindProperty("nameOrID");
        rotType = serializedObject.FindProperty("rotType");

        sceneName = serializedObject.FindProperty("sceneName"); sceneID = serializedObject.FindProperty("sceneID");

        ID = serializedObject.FindProperty("ID");
        unlocked = serializedObject.FindProperty("unlocked");
    }

    public override void OnInspectorGUI()
    {
        // Defining styles for labels
        GUIStyle style1 = new GUIStyle(GUI.skin.label);
        style1.wordWrap = true;
        style1.fontStyle = FontStyle.Bold;

        GUIStyle header = new GUIStyle(GUI.skin.label);
        header.wordWrap = true;
        header.fontStyle = FontStyle.Bold;
        header.alignment = TextAnchor.MiddleCenter;
        header.fontSize = 15;

        // Initialization...
        serializedObject.Update();
        Door instance = (Door)target;

        // Drawing GUI
        GUILayout.Label("------------------------------", header);

        GUILayout.Label("DOOR PARAMETERS", header);

        EditorGUILayout.Space(1);

        EditorGUILayout.PropertyField(ID, new GUIContent("ID", "Door ID - can be either different or equal on particular doorside"));

        EditorGUILayout.Space(1);

        EditorGUILayout.PropertyField(unlocked, new GUIContent("Door unlocked", "Can you walk through this doors?"));

        EditorGUILayout.Space(1);

        EditorGUILayout.PropertyField(defaultDestination, new GUIContent("Opposite destination", "Player will always be teleported to this destination when walking through the door"));

        EditorGUILayout.Space(1);

        EditorGUILayout.PropertyField(rotType);
        if (rotType.intValue == (int)Door.RotationType.Default)
            GUILayout.Label("Rotation of the destination point will be applied", style1);
        else
            EditorGUILayout.PropertyField(teleportCameraRotation, new GUIContent("Teleport rotation", "Define localEulerAngles of player camera after walking through the door"));

        EditorGUILayout.Space(1);

        GUILayout.Label("SCENE MANAGEMENT", header);

        EditorGUILayout.Space(1);

        EditorGUILayout.PropertyField(doorMode);

        EditorGUILayout.Space(1);

        switch ((Door.SceneState)doorMode.intValue)
        {
            case Door.SceneState.AllInOne:
                GUILayout.Label("All in one? Cool, nothing to do there! :)", style1);
                break;

            case Door.SceneState.AdditiveLoad:
            case Door.SceneState.SingleLoad:
            case Door.SceneState.Unload:

                EditorGUILayout.PropertyField(nameOrID);

                if (nameOrID.intValue == (int)Door.NameOrID.ID)
                    EditorGUILayout.PropertyField(sceneID, new GUIContent("Scene ID", "ID (Build Settings) of the scene you want to load/unload"));
                else
                    EditorGUILayout.PropertyField(sceneName, new GUIContent("Scene name", "Name of the scene you want to load/unload"));

                break;
        }

        EditorGUILayout.Space(1);

        GUILayout.Label("------------------------------", header);
        // Apply...
        serializedObject.ApplyModifiedProperties();
    }
}
#endif