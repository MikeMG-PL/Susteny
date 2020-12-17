﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    ViewMode v;
    TaskSystem t;
    public GameObject blackPanel;
    public static event Action<bool> Entered;

    void Start()
    {
        v = GameObject.FindGameObjectWithTag("Player").GetComponent<ViewMode>();
    }

    void OnMouseDown()
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
        StartCoroutine(Entering());
    }

    IEnumerator Entering()
    {
        var a = blackPanel.GetComponent<Animator>();
        var b = blackPanel.GetComponent<BlackScreen>();
        yield return new WaitForSeconds(2f);
        a.runtimeAnimatorController = b.Unfade;
        Entered.Invoke(true);
    }
}
