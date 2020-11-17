using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TaskSystem : MonoBehaviour
{
    [SerializeField] // List of tasks
    List<TaskScriptableObject> tasks = new List<TaskScriptableObject>();

    public GameObject tasksData;

    void Start()
    {
        updateTasksDisplay();
    }

    void Update()
    {
        incrementTime(Time.deltaTime);
        updateTasksDisplay();
    }

    // Ends task (set its isDone value to true), does not erase it from list 
    public void endTask(string taskId)
    {
        if (getTaskIterator(taskId) != -1) tasks[getTaskIterator(taskId)].isDone = true;
        else Debug.LogError("Trying to end task that doesnt exist!");
    }

    // Hides tasks (set its isVisible value to false), does not erase it from list  
    public void hideTask(string taskId)
    {
        if (getTaskIterator(taskId) != -1) tasks[getTaskIterator(taskId)].isVisible = false;
        else Debug.LogError("Trying to hide task that doesnt exist!");
    }

    // Adds new task, must have uniqueId
    public void addTask(TaskScriptableObject newTask)
    {
        if (getTaskIterator(newTask.id) == -1) tasks.Add(newTask); // there is no task in the list with such Id
        else Debug.LogError("Trying to add task with not unique Id!");
    }

    // ---------------------- PRIVATE PART ---------------------- //

    // Increments time for all tasks
    void incrementTime(float deltaTime) 
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            if(tasks[i].isDone == false) tasks[i].timeSpent = tasks[i].timeSpent  + deltaTime;
        }
    }

    // Pass task id, return that task iterator in list
    int getTaskIterator(string taskId) 
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].id == taskId) return i;
        }
        return -1; // there is no task with such taskId
    }

    // Updates display for tasks
    void updateTasksDisplay()
    {
        tasksData.GetComponent<Text>().text = "--- Tasks --- \n";
        for (int i = 0; i < tasks.Count; i++)
        {
            if(tasks[i].isVisible == true && tasks[i].isDone == false)
            {
                tasksData.GetComponent<Text>().text = tasksData.GetComponent<Text>().text + "+ " 
                + tasks[i].name + "\n       " + tasks[i].descprition + "\n       "
                + tasks[i].timeSpent + "\n";
            }
        }
    }

}
