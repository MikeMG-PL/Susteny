using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prototype : MonoBehaviour
{
    public TaskSystem tasks;

    public void AddTaskFromList(int i)
    {
        tasks.addTask(tasks.availableTasks[i]);
    }
}
