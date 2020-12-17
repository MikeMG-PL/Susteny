using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    ViewMode v;
    TaskSystem t;
    public GameObject blackPanel;

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
        blackPanel.GetComponent<Animator>().enabled = true;
    }

    void EnterTheBuilding()
    {
        ;
    }

}
