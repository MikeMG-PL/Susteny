using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSystem : MonoBehaviour
{
    [SerializeField]
    List<TaskScriptableObject> tasks;

    public TaskSystem() => tasks = new List<TaskScriptableObject>();

    void Start()
    {
        TaskScriptableObject task = ScriptableObject.CreateInstance<TaskScriptableObject>();
        task.name = "gra";
        task.id = "sda";
        task.descprition = "sgsdfsdfd";
        tasks.Add(task);
    }

    void Update()
    {
        incrementTime(Time.deltaTime);
    }

    public void endTask(string taskId)
    {
        if (getTaskIterator(taskId) != -1) tasks[getTaskIterator(taskId)].isDone = true;
        else Debug.LogError("Trying to end task that doesnt exist!");
    }

    public void hideTask(string taskId)
    {
        if (getTaskIterator(taskId) != -1) tasks[getTaskIterator(taskId)].isVisible = false;
        else Debug.LogError("Trying to hide task that doesnt exist!");
    }

    public void addTask(TaskScriptableObject newTask) // add new task
    {
        if (getTaskIterator(newTask.id) == -1) tasks.Add(newTask); // there is no task in the list with such Id
        else Debug.LogError("Trying to add task with not unique Id!");
    }

    void incrementTime(float deltaTime) // incrementing time for all tasks
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            tasks[i].timeSpent = +deltaTime;
        }
    }

    int getTaskIterator(string taskId) // give task id, get that task iterator in list
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].id == taskId) return i;
        }
        return -1; // there is no task with such taskId
    }


}
