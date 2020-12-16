using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueInteraction : MonoBehaviour
{
    GameObject player;
    GameObject panel;
    public float interactionDistance = 5f;
    bool talking;
    string talkingGameObjectName;
    int dialogueID;

    private void OnMouseDown()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= interactionDistance && !talking)
        {
            Talk(true);
        }
    }

    void Awake()
    {
        panel = GameObject.FindGameObjectWithTag("DialoguePanel");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public static event Action<bool> Talking;
    public static event Action<bool, string, int> Conversation;

    public void Talk(bool b)
    {
        var d = GetComponent<LoadDialogue>();
        talkingGameObjectName = d.gameObject.name;
        dialogueID = d.currentDialogueID;

        if (d.dialogues.Count > 0)
        {
            talking = b;
            panel.transform.GetChild(0).gameObject.SetActive(b);
            Conversation.Invoke(b, talkingGameObjectName, dialogueID);
            Talking.Invoke(b);

            if (b)
            {
                DestroyButtons(d);
                d.Load(d.currentDialogueID);
                d.ProcessDialogue();
            }
            else
            {
                DestroyButtons(d);
            }
        }

        void DestroyButtons(LoadDialogue dial)
        {
            foreach (GameObject o in dial.buttons)
            {
                Destroy(o);
            }
            dial.buttons?.Clear();
        }
    }
}
