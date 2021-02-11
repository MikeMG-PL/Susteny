using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueInteraction : MonoBehaviour
{   
    /// Custom editor ///
    [HideInInspector] public GameObject player;
    [HideInInspector] public PlayerActions playerActions;

    /// Private properties ///
    GameObject panel;
    string talkingGameObjectName;
    int dialogueID;

    /// Public properties ///
    public float interactionDistance = 5f;

    public void TryToTalk()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= interactionDistance && player.GetComponent<PlayerActions>().canInteract)
        {
            Talk(true);
        }
    }

    /// Events ///
    public static event Action<bool> Talking;
    public static event Action<bool, string, int> Conversation;

    void Awake()
    {
        panel = GameObject.FindGameObjectWithTag("DialoguePanel");
        player = GameObject.FindGameObjectWithTag("Player");
        playerActions = player.GetComponent<PlayerActions>();
        Conversation += Conv;
    }

    private void Conv(bool arg1, string arg2, int arg3) { ; }

    public void Talk(bool b)
    {
        LoadDialogue d = GetComponent<LoadDialogue>();
        talkingGameObjectName = d.gameObject.name;
        dialogueID = d.currentDialogueID;

        if (d.dialogues.Count > 0)
        {
            panel.transform.GetChild(0).gameObject.SetActive(b);
            Talking.Invoke(b);
            if (b)
            {
                if (GetComponent<ManipulatePlayer>() != null) GetComponent<ManipulatePlayer>().Manipulate();
                DestroyButtons(d);
                d.Load(d.currentDialogueID);
                d.ProcessDialogue();
            }
            else
            {
                player.GetComponent<SC_FPSController>().ForceStopGoingRotating();
                player.GetComponent<SC_FPSController>().LockControlsCursorOn(false);
                DestroyButtons(d);
                Conversation.Invoke(false, talkingGameObjectName, dialogueID);
            }
        }
    }

    void DestroyButtons(LoadDialogue dial)
    {
        var p = GameObject.FindGameObjectWithTag("DialoguePanel").transform.GetChild(0);
        for (int i = 0; i < p.childCount; i++)
        {
            if (p.GetChild(i).GetComponent<Button>() != null)
                Destroy(p.GetChild(i).gameObject);
        }
        foreach (GameObject o in dial.buttons)
        {
            Destroy(o);
        }
        dial.buttons?.Clear();
    }

    void OnDisable()
    {
        Conversation -= Conv;
    }
}
