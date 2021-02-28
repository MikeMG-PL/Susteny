using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TaskData", menuName = "Task system/TaskDataContainer", order = 1)]
[System.Serializable]
public class TaskScriptableObject : ScriptableObject
{
    public string id; // must be unique
    public new string name; // name for task (not unique)
    public string descprition; 
    public bool isDone = false;
    public bool isVisible = true; // is it visible for player?
    public float timeSpent = 0; // time spent on certain quest (in seconds)
}