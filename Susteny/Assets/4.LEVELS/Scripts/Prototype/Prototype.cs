using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Prototype : MonoBehaviour
{
    public TaskSystem taskSystem;
    public PlayableDirector timeline5;
    public Item item;
    /////////////////////////////////////
    Item inventoryItem;
    int viewCounter;

    /// SUBSCRIBING EVENTS ///
    void Awake()
    {
        ViewMode.ViewingDetails += OnViewModeSwitch;
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

    void OnViewModeSwitch(bool b, GameObject o)
    {
        if(b)
            inventoryItem = o?.GetComponent<ItemID>().thisItem;

        if (!b && inventoryItem == item)
        {
            viewCounter++;
            inventoryItem = null;
        }

        if(viewCounter == 2)
            timeline5.Play();
    }
}
