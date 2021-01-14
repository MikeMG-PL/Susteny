using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Prototype : MonoBehaviour
{
    public TaskSystem taskSystem;
    public PlayableDirector timeline5;
    public PlayableDirector timeline8;
    public Item item;
    /////////////////////////////////////
    Item inventoryItem;
    int viewCounter;

    /// SUBSCRIBING EVENTS ///
    void Awake()
    {
        ViewMode.ViewingDetails += OnViewModeSwitch;
        Door.WalkThrough += WalkThroughDoor;
    }

    /// UNSUBSCRIBING EVENTS ///
    void OnDisable()
    {
        ViewMode.ViewingDetails -= OnViewModeSwitch;
        Door.WalkThrough -= WalkThroughDoor;
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

    void WalkThroughDoor(int ID)
    {
        if (ID == 0)
        {
            foreach (TaskScriptableObject so in taskSystem.tasks) taskSystem.hideTask(so.id.ToString());
            AddTaskFromList(3);
            timeline8.Play();
        }
    }
}
