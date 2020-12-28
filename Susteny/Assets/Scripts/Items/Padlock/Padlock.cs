﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Padlock : MonoBehaviour
{
    RotatePadlockWheels[] children;

    public static event Action<bool> PadlockUnlocked;

    void Awake()
    {
        children = GetComponentsInChildren<RotatePadlockWheels>();
    }

    void CheckIfWheelsAreCorrect()
    {
        bool unlock = true;

        foreach (RotatePadlockWheels wheel in children)
        {
            if (!wheel.correct) unlock = false;
        }

        if (unlock) Unlocked();
    }

    void Unlocked()
    {
        GetComponent<Renderer>().material.color = Color.green;
        PadlockUnlocked.Invoke(true);
    }

    void Update()
    {
        CheckIfWheelsAreCorrect();
    }
}
