using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Padlock : MonoBehaviour
{
    RotatePadlockWheels[] children;
    Color defaultColor;
    bool unlocked = false;

    public static event Action<bool> PadlockUnlocked;

    void Awake()
    {
        children = GetComponentsInChildren<RotatePadlockWheels>();
        defaultColor = GetComponent<Renderer>().material.color;
    }

    void CheckIfWheelsAreCorrect()
    {
        bool unlock = true;

        foreach (RotatePadlockWheels wheel in children)
        {
            if (!wheel.correct) unlock = false;
        }

        if (unlock && !unlocked) Unlock();
        else if (!unlock && unlocked) Lock();
    }

    void Unlock()
    {
        GetComponent<Renderer>().material.color = Color.green;
        GetComponent<Interactable>().playerActions.StopFocusingOnObject(true);
        unlocked = true;
        PadlockUnlocked.Invoke(true);
    }

    void Lock()
    {
        GetComponent<Renderer>().material.color = defaultColor;
        unlocked = false;
        PadlockUnlocked.Invoke(false);
    }

    void Update()
    {
        if (GetComponent<Interactable>().isInteractedWith) CheckIfWheelsAreCorrect();
    }
}
