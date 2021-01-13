using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OuterDoor))]
[CanEditMultipleObjects]
public class OuterDoorEditor : Editor
{
    // declare serialized properties here

    private void OnEnable()
    {
        // assign serialized properties here
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // draw GUI here

        serializedObject.ApplyModifiedProperties();
    }
}
