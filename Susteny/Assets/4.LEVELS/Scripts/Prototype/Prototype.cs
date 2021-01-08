using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Prototype : MonoBehaviour
{
    public TaskSystem tasks;
    public PlayableDirector timeline5;

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
        tasks.addTask(tasks.availableTasks[i]);
    }

    void OnViewModeSwitch(bool b)
    {
        if(!b)
        {
            Debug.Log("Off");
        }
    }
}
