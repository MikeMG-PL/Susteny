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

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1)) Talk(false);
    }

    private void OnMouseDown()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= interactionDistance && !talking)
        {
            Talk(true);
        }
    }

    public static event Action<bool> Talking;

    void Talk(bool b)
    {
        talking = b;
        panel.SetActive(b);
        Talking.Invoke(b);

        if (b)
        {
            var d = GetComponent<LoadDialogue>();
            d.Load(d.currentDialogueID);
            d.ProcessDialogue();
        }
    }
}
