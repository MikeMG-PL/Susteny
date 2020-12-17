using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyDoor : MonoBehaviour
{
    public int ID;
    bool unlocked;
    ViewMode v;
    GameObject blackPanel;
    public static event Action<bool, int> Opened;

    void Start()
    {
        blackPanel = GameObject.FindGameObjectWithTag("BlackPanel");
        v = GameObject.FindGameObjectWithTag("Player").GetComponent<ViewMode>();
    }

    void OnMouseDown()
    {
        v.ToggleViewMode(null, false, true);
        if (unlocked)
        {    
            FadeIn();
            gameObject.AddComponent<ItemWorld>();
            GetComponent<ItemWorld>().action = Action.interactable;
        }
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
        Opened.Invoke(true, ID);
    }
}
