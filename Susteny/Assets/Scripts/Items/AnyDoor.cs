using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyDoor : MonoBehaviour
{
    public int ID;
    public List<Transform> doorSides;
    public bool switchSides;
    public bool unlocked;
    ViewMode v;
    GameObject blackPanel;

    void Start()
    {
        blackPanel = GameObject.FindGameObjectWithTag("BlackPanel");
        v = GameObject.FindGameObjectWithTag("Player").GetComponent<ViewMode>();
    }

    public void UnlockDoor()
    {
        unlocked = true;
        gameObject.AddComponent<ItemWorld>();
        GetComponent<ItemWorld>().action = Action.door;
    }

    public void OpenDoor()
    {
        if (unlocked) FadeIn();
    }

    void FadeIn()
    {
        var a = blackPanel.GetComponent<Animator>();
        var b = blackPanel.GetComponent<BlackScreen>();
        a.enabled = true;
        a.runtimeAnimatorController = b.Fade;
        StartCoroutine(Entering());
    }

    IEnumerator Entering()
    {
        var a = blackPanel.GetComponent<Animator>();
        var b = blackPanel.GetComponent<BlackScreen>();
        yield return new WaitForSeconds(2f);
        a.runtimeAnimatorController = b.Unfade;

        Teleport();
    }

    void Teleport()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        p.GetComponent<CharacterController>().enabled = false;
        p.transform.position = doorSides[0].position;
        p.transform.localEulerAngles = new Vector3(0, 0, 0);
        p.GetComponent<CharacterController>().enabled = true;
        v.ToggleViewMode(null, false, false);
    }

    void ChangeSide()
    {
        if (switchSides)
        {
            var t = doorSides[0];
            doorSides[0] = doorSides[1];
            doorSides[1] = t;
        }
    } // TASKIIIII
}
