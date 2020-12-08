﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Level { None, Prototype };
    public Level level;

    void Awake()
    {
        LoadLevelData();
    }

    void LoadLevelData()
    {
        switch (level)
        {
            case Level.Prototype:
                gameObject.AddComponent<Prototype>();
                break;

            default:
                break;
        }
    }
}