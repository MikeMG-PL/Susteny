using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerDoor : MonoBehaviour
{
    /// CUSTOM INSPECTOR PART ///
    public Vector3 defaultDestination;
    [HideInInspector] public Vector3 lastPosition;
    public bool alwaysReturnToLastPosition;

    /// SCRIPT PROPERTIES ///
    Door doorScript;
    int doorID;

    /// EVENTS ///
    public static event Action<int> WalkOut;

    /// FUNCTIONS ///
    void Awake()
    {
        doorScript = GetComponentInParent<Door>();
        doorID = doorScript.ID;
    }

    void Interact()
    {

        WalkOut?.Invoke(doorID);
        Debug.Log($"Player walked OUT of the door {doorID}");
    }
}
