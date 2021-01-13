using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    /// Enum ///
    public enum SceneState { AllInOne, Unload, AdditiveLoad, SingleLoad };
    public enum NameOrID { Name, ID };

    /// CUSTOM INSPECTOR PART ///
    // Door settings
    public int ID;
    public Vector3 defaultDestination;
    public bool alwaysReturnToLastPosition;
    public float teleportCameraRotationY;

    // Scene settings
    public SceneState doorMode;
    public NameOrID nameOrID;
    public string sceneName; public int sceneID;

    /// SCRIPT PROPERTIES ///
    Vector3 lastPosition;

    /// EVENTS ///

    /// FUNCTIONS ///

    void Interact()
    {

    }
}
