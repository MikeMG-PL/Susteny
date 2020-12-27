using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public AnyDoor secondDoor;
    ViewMode v;
    TaskSystem t;
    public GameObject blackPanel;
    public static event Action<bool> Entered;
    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Padlock.PadlockUnlocked += UnlockSecondDoor;
        v = GameObject.FindGameObjectWithTag("Player").GetComponent<ViewMode>();
    }

    public void OpenDoor()
    {
        v.ToggleViewMode(null, false, false);
        t = GameObject.FindGameObjectWithTag("TaskSystem").GetComponent<TaskSystem>();
        foreach (TaskScriptableObject so in t.tasks) t.hideTask(so.id.ToString());
        FadeIn();
}

    void FadeIn()
    {
        var a = blackPanel.GetComponent<Animator>();
        var b = blackPanel.GetComponent<BlackScreen>();
        a.enabled = true;
        a.runtimeAnimatorController = b.Fade;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActions>().inventoryAllowed = false;
        StartCoroutine(Entering());
    }

    IEnumerator Entering()
    {
        var a = blackPanel.GetComponent<Animator>();
        var b = blackPanel.GetComponent<BlackScreen>();
        yield return new WaitForSeconds(2f);
        Entered.Invoke(true);
        a.runtimeAnimatorController = b.Unfade;
    }

    void OnDestroy()
    {
        Padlock.PadlockUnlocked -= UnlockSecondDoor;
    }

    bool openSecondDoor;
    void UnlockSecondDoor(bool b)
    {
        if (!openSecondDoor)
        {
            secondDoor.UnlockDoor();
            openSecondDoor = true;
        }
    }
}
