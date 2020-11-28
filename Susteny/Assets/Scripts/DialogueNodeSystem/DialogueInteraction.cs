using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueInteraction : MonoBehaviour
{
    public GameObject player;
    public GameObject panel;
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

    public static event Action<bool> Talking;

    public void Talk(bool b)
    {
        talking = b;
        panel.SetActive(b);
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
