using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Prototype : MonoBehaviour
{
    public TaskSystem taskSystem;
    public PlayableDirector timeline5;
    public GameObject item;
    /////////////////////////////////////
    int viewOffCounter;

    /// SUBSCRIBING EVENTS ///
    void Awake()
    {
        ViewMode.ViewingItem += OnViewModeSwitch;
    }

    /// UNSUBSCRIBING EVENTS ///
    void OnDisable()
    {
        
    }

    /// FUNCTIONS ///
    public void AddTaskFromList(int i)
    {
        taskSystem.addTask(taskSystem.availableTasks[i]);
    }

    public void HideTask(int i)
    {
        taskSystem.hideTask($"{i}");
    }

    void OnViewModeSwitch(bool b)
    {
        if (!b)
            viewOffCounter++;

        if (viewOffCounter == 3)
            timeline5.Play();
    }
}
