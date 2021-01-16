using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;

public class BuildingManager : MonoBehaviour
{
    public LoadDialogue buddy;
    public LoadDialogue Vlad;
    public PlayableDirector timeline11;
    public AudioClip Seamstress;
    public Door doorToUnlock;
    TaskSystem tasks;
    Inventory inv;
    AudioSource playerSource;

    void Awake()
    {
        DialogueInteraction.Conversation += ConversationStart;
        Padlock.PadlockUnlocked += UnlockedPadlock;
        playerSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        inv = playerSource.GetComponent<Inventory>();
    }

    void OnDisable()
    {
        DialogueInteraction.Conversation -= ConversationStart;
        Padlock.PadlockUnlocked -= UnlockedPadlock;
    }

    public void SetDialogue(LoadDialogue character, int i)
    {
        character.currentDialogueID = i;
    }

    public void ConversationStart(bool start, string objSpeakerName, int ID)
    {
        if (objSpeakerName == "UnknownBuddy")
        {
            switch (ID)
            {
                default:
                    break;

                case 0:
                    if (!start)
                    {
                        SetDialogue(buddy, 1);
                        buddy.GetComponent<ManipulatePlayer>().enableGoTo = false;
                    }
                    break;
            }
        }
        if (objSpeakerName == "Vlad")
            switch (ID)
            {
                default:
                    break;

                case 0:
                    if (!start)
                    {
                        SetDialogue(Vlad, 1);
                        Vlad.GetComponent<ManipulatePlayer>().enableGoTo = false;
                        tasks = GameObject.FindGameObjectWithTag("TaskSystem").GetComponent<TaskSystem>();
                        tasks.hideTask("3");
                        tasks.addTask(tasks.availableTasks[4]);
                        GameObject.FindGameObjectWithTag("LEVELS").GetComponent<Prototype>().FadeAmble();
                    }
                    break;
            }
    }

    void UnlockedPadlock(bool b)
    {
        if (b)
            timeline11.Play();
    }

    public void FadeSong()
    {
        playerSource.clip = Seamstress;
        playerSource.time = 33.5f;
        playerSource.Play();

        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        playerSource.volume = 0;
        while (playerSource.volume <= 0.225f)
        {
            playerSource.volume += 0.00175f;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        playerSource.volume = 0.225f;
        StopCoroutine(Fade());
    }

    bool unlocked;
    void Update()
    {
        if (inv.GetInventory().Count > 0 && !unlocked)
        {
            for (int i = 0; i < inv.GetInventory().Count; i++)
            {
                if (inv.GetInventory()[i].item.name == "Klucz")
                {
                    doorToUnlock.unlocked = true;
                    unlocked = true;
                }
            }
        }
    }
}

