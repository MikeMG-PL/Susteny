using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prototype : MonoBehaviour
{
    void Start()
    {
        AnyDoor.Opened += WalkThorughDoors;
        DialogueInteraction.Conversation += ConversationEvent;
        ViewMode.ViewingItem += QuittedPhotoViewing;
        Padlock.PadlockUnlocked += AfterPadlock;
        Door.Entered += InBuilding;
        LevelEvents();
    }

    void LevelEvents()
    {
        if (!GetComponent<GameManager>().skipPW)
        {
            LevelStarted(true);
            StartCoroutine(PanelAndUnfreezing());
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(140, 12, 140);
        }
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
        //102 -10 91
        if(Input.GetKeyDown(KeyCode.R))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().enabled = false;
            GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(33, 11, 106);
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().enabled = true;

        }
            
    }

    void OnDisable()
    {
        DialogueInteraction.Conversation -= ConversationEvent;
        ViewMode.ViewingItem -= QuittedPhotoViewing;
        Padlock.PadlockUnlocked -= AfterPadlock;
        Door.Entered -= InBuilding;
        AnyDoor.Opened -= WalkThorughDoors;
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

    void InBuilding(bool b)
    {
        Debug.Log("aaa");
        Teleport();
    }

    void AfterPadlock(bool b)
    {
        
    }

    void WalkThorughDoors(bool b, int i)
    {
        ;
    }

    void Teleport()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        p.GetComponent<CharacterController>().enabled = false;
        p.transform.position = new Vector3(140, 12, 140);
        p.transform.localEulerAngles = new Vector3(0, 0, 0);
        p.GetComponent<CharacterController>().enabled = true;
    }
}
