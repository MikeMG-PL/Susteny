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

    public void Talk(bool b)
    {
        talking = b;
        panel.transform.GetChild(0).gameObject.SetActive(b);
        Talking.Invoke(b);

        var d = GetComponent<LoadDialogue>();
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

    void DestroyButtons(LoadDialogue d)
    {
        foreach (GameObject o in d.buttons)
        {
            Destroy(o);
        }
        d.buttons?.Clear();
    }
}
