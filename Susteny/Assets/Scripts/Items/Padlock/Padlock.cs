using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Padlock : MonoBehaviour
{
    RotatePadlockWheels[] children; 
    bool isUnlocked;

    private void Awake()
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

        if (unlock) isUnlocked = true;
        else isUnlocked = false;
    }

    void Update()
    {
        CheckIfWheelsAreCorrect();
    }
}
