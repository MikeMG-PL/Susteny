using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Door))]
[CanEditMultipleObjects]
public class InnerDoorEditor : Editor
{
    SerializedProperty defaultDestination;
    SerializedProperty alwaysReturnToLastPosition;
    SerializedProperty teleportCameraRotationY;
    SerializedProperty ID;
    SerializedProperty sceneName; SerializedProperty sceneID;
    SerializedProperty doorType;
    SerializedProperty sceneNameOrID;

    Door.SceneState doorMode;
    Door.NameOrID nameOrID;

    private void OnEnable()
    {
        defaultDestination = serializedObject.FindProperty("defaultDestination");
        alwaysReturnToLastPosition = serializedObject.FindProperty("alwaysReturnToLastPosition");
        teleportCameraRotationY = serializedObject.FindProperty("teleportCameraRotationY");

        doorType = serializedObject.FindProperty("doorMode");
        sceneNameOrID = serializedObject.FindProperty("nameOrID");

        sceneName = serializedObject.FindProperty("sceneName"); sceneID = serializedObject.FindProperty("sceneID");

        ID = serializedObject.FindProperty("ID");
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

        EditorGUILayout.PropertyField(alwaysReturnToLastPosition, new GUIContent("Return to last position", "This is a simple door - you walk in, so when you walk out you go back to your previous position B)"));

        EditorGUILayout.Space(1);

        // Conditional GUI draws (it's super cool! :O)
        if (!instance.alwaysReturnToLastPosition)
            EditorGUILayout.PropertyField(defaultDestination, new GUIContent("Opposite destination", "Player will always be teleported to this destination when walking through the door"));
        else
            GUILayout.Label("Player will always return to his previous position he had in the moment of walking through the door. The default (eg. if none) is (0, 0, 0)", style1);

        // Drawing GUI
        EditorGUILayout.Space(1);

        EditorGUILayout.PropertyField(teleportCameraRotationY, new GUIContent("Y rotation after walkthrough", "Define localEulerAngles.y of player camera after walking through the door"));

        EditorGUILayout.Space(1);

        GUILayout.Label("SCENE MANAGEMENT", header);

        EditorGUILayout.Space(1);

        GUILayout.Label("Door mode:", style1);

        doorMode = (Door.SceneState)EditorGUILayout.EnumPopup(doorMode);
        doorType.intValue = (int)doorMode;

        EditorGUILayout.Space(1);

        switch(doorMode)
        {
            case Door.SceneState.AllInOne:
                GUILayout.Label("All in one? Cool, nothing to do there! :)", style1);
                break;

            case Door.SceneState.AdditiveLoad:
            case Door.SceneState.SingleLoad:
            case Door.SceneState.Unload:

                GUILayout.Label("Either scene name or ID?", style1);
                nameOrID = (Door.NameOrID)EditorGUILayout.EnumPopup(nameOrID);

                if(nameOrID == Door.NameOrID.ID)
                    EditorGUILayout.PropertyField(sceneID, new GUIContent("Scene ID", "ID (Build Settings) of the scene you want to load/unload"));
                else
                    EditorGUILayout.PropertyField(sceneName, new GUIContent("Scene name", "Name of the scene you want to load/unload"));

                break;
        }
        sceneNameOrID.intValue = (int)nameOrID;

        EditorGUILayout.Space(1);

        GUILayout.Label("------------------------------", header);
        // Apply...
        serializedObject.ApplyModifiedProperties();
    }
}