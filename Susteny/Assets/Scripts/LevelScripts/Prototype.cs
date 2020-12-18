using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prototype : MonoBehaviour
{
    void Start()
    {
        Padlock.PadlockUnlocked += MuteMusic;
        DialogueInteraction.Conversation += ConversationEvent;
        ViewMode.ViewingItem += QuittedPhotoViewing;
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

    public void LevelStarted(bool b)
    {
        LevelStart.Invoke(b);
    }

    void ShowPanelAtStart(bool b)
    {
        ShowStartPanel.Invoke(b);
    }

    public void UnfreezeLooking(bool b)
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

    void OnDisable()
    {
        DialogueInteraction.Conversation -= ConversationEvent;
        ViewMode.ViewingItem -= QuittedPhotoViewing;
        Door.Entered -= InBuilding;
        Padlock.PadlockUnlocked -= MuteMusic;
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
        Teleport();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActions>().inventoryAllowed = true;
        var t = GameObject.FindGameObjectWithTag("TaskSystem").GetComponent<TaskSystem>();
        t.addTask(t.availableTasks[3]);
    }

    void Teleport()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        p.GetComponent<CharacterController>().enabled = false;
        p.transform.position = new Vector3(140, 12, 140);
        p.transform.localEulerAngles = new Vector3(0, 0, 0);
        p.GetComponent<CharacterController>().enabled = true;
    }

    AudioSource a;
    void MuteMusic(bool b)
    {
        mute = true;
        a = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
    }

    bool mute;
    bool stopMuting;
    void Update()
    {
        if (mute && a.volume > 0 && !stopMuting)
        {
            a.volume -= Time.deltaTime / 30;
            if (a.volume <= 0)
                stopMuting = true;
        }
    }
}
