using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prototype : MonoBehaviour
{
    void Start()
    {
        DialogueInteraction.Conversation += ConversationEvent;
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
    public static event Action<bool> UnlockBuilding;

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

    void UnlockTheBuilding(bool b)
    {
        UnlockBuilding.Invoke(b);
    }

    public IEnumerator PanelAndUnfreezing()
    {
        if (!GetComponent<GameManager>().skipIntro)
        {
            ShowPanelAtStart(true);
            yield return new WaitForSecondsRealtime(22.5f);
            UnfreezeLooking(true);
        }
        else
            UnfreezeLooking(true);
    }

    void ConversationEvent(bool b, string n, int i)
    {
        if (b == false && n == "Anna" && i == 1)
        {
            UnlockTheBuilding(true);
        }
    }

    ///////////////////////////////////////////////////////////
    //WARUNKI
    
    void Update()
    {
        ;
    }

    void OnDisable()
    {
        DialogueInteraction.Conversation -= ConversationEvent;
    }
}
