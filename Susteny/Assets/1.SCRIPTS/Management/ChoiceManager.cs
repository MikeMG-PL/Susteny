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
        public List<List<UnityEvent>> eventListContainer; // This is a container corelated with dialogues list. In every element it contains list of dialogue events
        public List<UnityEvent> actions;

        void Awake()
        {
            Subscribe();
        }



        /// SUBSCRIBING AND UNSUBSCRIBING EVENTS, EVENT FUNCTIONS ///

        void Subscribe()
        {
            LoadDialogue.OptionChosen += IterateThroughDialogues;
        }

        void IterateThroughDialogues(DialogueContainer dialogue, string baseGUID, string text)
        {
            foreach (DialogueContainer d in dialogues)
            {
                if (d == dialogue)
                {
                    for(int i = 1; i < d.NodeLinks.Count; i++)
                    {

                    }
                }
            }
        }

        void CheckForOption(DialogueContainer dialogue, string baseGUID, string text)
        {
            
        }

        void InvokeUnityEvent()
        {

        }

        void Unsubscribe()
        {
            LoadDialogue.OptionChosen -= IterateThroughDialogues;
        }

        void OnDisable()
        {
            Unsubscribe();
        }
    }
}

