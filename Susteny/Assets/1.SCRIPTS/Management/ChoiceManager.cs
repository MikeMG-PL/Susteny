using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Subtegral.DialogueSystem.DataContainers
{
    [System.Serializable]
    public class Choice
    {
        public UnityEvent choice;
        public string optionText;
        public string baseGUID;
    }


    [System.Serializable]
    public class ChoiceManager : MonoBehaviour
    {
        /// CUSTOM EDITOR ///

        //public Choice choice;
        public DialogueContainer dialogue;
        public List<Choice> choiceList;
        public bool canView;

        void Awake()
        {
            Subscribe();
        }

        /// SUBSCRIBING AND UNSUBSCRIBING EVENTS, EVENT FUNCTIONS ///

        void Subscribe()
        {
            LoadDialogue.OptionChosen += CheckForOption;
        }

        void CheckForOption(DialogueContainer dial, string guid, string text)
        {
            if(dial == dialogue)
            {
                for(int i = 0; i < choiceList.Count; i++)
                {
                    var v = choiceList[i];
                    if (v.baseGUID == guid && v.optionText == text)
                        v.choice.Invoke();
                }
            }
        }

        void Unsubscribe()
        {
            LoadDialogue.OptionChosen -= CheckForOption;
        }

        void OnDisable()
        {
            Unsubscribe();
        }
    }
}

