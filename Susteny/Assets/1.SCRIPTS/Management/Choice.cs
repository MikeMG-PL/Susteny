using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Subtegral.DialogueSystem.DataContainers;

[CreateAssetMenu(fileName = "Choice 1", menuName = "Choice system/Choice container")]
public class Choice : ScriptableObject
{
    public DialogueContainer connectedDialogue;
    public List<UnityEvent> dialogueChoices;
}