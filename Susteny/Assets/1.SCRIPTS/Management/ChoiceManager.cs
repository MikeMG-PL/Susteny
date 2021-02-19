using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Subtegral.DialogueSystem.DataContainers
{
    public class ChoiceManager : MonoBehaviour
    {
        /// CUSTOM EDITOR ///

        public List<DialogueContainer> dialogues;
        public List<NodeLinkData> choices;
        public List<UnityEvent> actions;
    }
}

