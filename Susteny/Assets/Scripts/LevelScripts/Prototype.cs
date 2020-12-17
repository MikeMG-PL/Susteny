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
        ViewMode.ViewingItem += QuittedPhotoViewing;
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
    bool buildingUnlocked;

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
        buildingUnlocked = true;
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

    }

    void OnDisable()
    {
        DialogueInteraction.Conversation -= ConversationEvent;
        ViewMode.ViewingItem -= QuittedPhotoViewing;
    }

    bool proceed = true;
    void QuittedPhotoViewing(bool b)
    {
        if (!b && buildingUnlocked && proceed)
        {
            var t = GameObject.FindGameObjectWithTag("TaskSystem").GetComponent<TaskSystem>();

            t.hideTask("0");
            t.hideTask("1");
            t.addTask(t.availableTasks[2]);
            t.showTask("1");
            proceed = false;
        }
    }
}
