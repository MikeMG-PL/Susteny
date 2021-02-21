using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Subtegral.DialogueSystem.DataContainers
{
    [System.Serializable]
    public class ChoiceEvent
    {
        public string baseGUID, text;
        public DialogueContainer container;
        public UnityEvent evt;
    }

    [System.Serializable]
    public class ChoiceManager : MonoBehaviour
    {
        /// CUSTOM EDITOR ///

        public List<DialogueContainer> dialogues;
        public List<ChoiceEvent> choiceEvents;
        public List<UnityEvent> unityEventList;


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
            foreach(ChoiceEvent c in choiceEvents)
            {
                if (baseGUID == c.baseGUID && text == c.text)
                    c.evt.Invoke();
            }

            /*foreach (DialogueContainer d in dialogues)
            {
                if (d == dialogue)
                {
                    for(int i = 1; i < d.NodeLinks.Count; i++)
                    {

                    }
                }
            }*/
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

