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

    public void TryToTalk()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= interactionDistance && player.GetComponent<PlayerActions>().canInteract)
        {
            Talk(true);
        }
    }

    public static event Action<bool> Talking;
    public static event Action<bool, string, int> Conversation;

    void Awake()
    {
        panel = GameObject.FindGameObjectWithTag("DialoguePanel");
        player = GameObject.FindGameObjectWithTag("Player");
        Conversation += Conv;
    }

    private void Conv(bool arg1, string arg2, int arg3) { ; }

    public void Talk(bool b)
    {
        var actions = player.GetComponent<PlayerActions>();
        var d = GetComponent<LoadDialogue>();
        talkingGameObjectName = d.gameObject.name;
        dialogueID = d.currentDialogueID;

        if (d.dialogues.Count > 0)
        {
            talking = b;
            panel.transform.GetChild(0).gameObject.SetActive(b);
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
                Conversation.Invoke(false, talkingGameObjectName, dialogueID);
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
    }

    void OnDisable()
    {
        Conversation -= Conv;
    }
}
