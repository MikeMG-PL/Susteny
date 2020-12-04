using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prototype : MonoBehaviour
{
    void Start()
    {
        LevelEvents();    
    }

    void LevelEvents()
    {
        LevelStarted(true);
        StartCoroutine(PanelAndUnfreezing());
    }

    /////////////////////////////////////////

    public static event Action<bool> LevelStart;
    public static event Action<bool> ShowStartPanel;
    public static event Action<bool> MouseLookUnfreeze;

    void LevelStarted(bool b)
    {
        LevelStart.Invoke(b);
    }

    void ShowPanelAtStart(bool b)
    {
        ShowStartPanel.Invoke(b);
    }

    void UnfreezeLooking(bool b)
    {
        MouseLookUnfreeze.Invoke(b);
    }

    public IEnumerator PanelAndUnfreezing()
    {
        ShowPanelAtStart(true);
        yield return new WaitForSecondsRealtime(22.5f);
        UnfreezeLooking(true);
    }
}
