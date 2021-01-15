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
    AudioSource playerSource;

    void Awake()
    {
        DialogueInteraction.Conversation += ConversationStart;
        Padlock.PadlockUnlocked += UnlockedPadlock;
        playerSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
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
                    }
                    break;
            }
    }

    void UnlockedPadlock(bool b)
    {
        if (b)
        {
            playerSource.clip = Seamstress;
            playerSource.time = 33.5f;
            playerSource.Play();
            StartCoroutine(Fade());
            timeline11.Play();
        }
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
}
