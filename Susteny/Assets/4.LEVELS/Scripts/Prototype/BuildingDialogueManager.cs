using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDialogueManager : MonoBehaviour
{
    public LoadDialogue buddy;
    public LoadDialogue Vlad;

    void Awake()
    {
        DialogueInteraction.Conversation += ConversationStart;    
    }

    void OnDisable()
    {
        DialogueInteraction.Conversation -= ConversationStart;
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
}
