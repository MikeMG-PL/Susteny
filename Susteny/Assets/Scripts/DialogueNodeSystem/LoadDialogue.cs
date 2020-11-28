using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Subtegral.DialogueSystem.DataContainers;

public class LoadDialogue : MonoBehaviour
{
    public string nameOfNPC;
    public int currentDialogueID;
    public int placeHolderChoice;
    public List<DialogueContainer> dialogues;
    DialogueContainer currentDialogue;
    bool dialogueStarted;

    string dialogueText;
    List<string> options;
    List<string> targetNodes;
    string nodeGUID;
    bool quitNode;

    public GameObject panel;
    public GameObject buttonPrefab;
    Text npcName;
    Text sentence;

    // Wczytanie dialogu
    public void Load(int i)
    {
        currentDialogue = dialogues[i];
        if (currentDialogue == null)
            Debug.LogError($"Beware! NPC \"{name}\", a.k.a. \"{nameOfNPC}\" has empty places for dialogue data! Assign them in the inspector.");

        npcName = panel.GetComponent<Panel>().npcName;
        sentence = panel.GetComponent<Panel>().sentence;
    }

    // Prowadzenie dialogu - przenoszenie kwestii z wczytanego SO do gry
    public void ProcessDialogue()
    {
        if (!dialogueStarted)
            FirstNode();
        else
        {
            GetNode();
            if (!quitNode)
                GetOptions();
        }
        dialogueStarted = true;
    }

    /// FUNKCJE ODCZYTUJĄCE DANE Z POBRANEGO SO W RAMACH FUNKCJI ProcessDialogue() ///

    void FirstNode()
    {
        quitNode = currentDialogue.DialogueNodeData[0].QuitNode;
        if (!quitNode)
        {
            nodeGUID = currentDialogue.DialogueNodeData[0].NodeGUID;
            dialogueText = currentDialogue.DialogueNodeData[0].DialogueText;
            Debug.Log(dialogueText);

            if(!currentDialogue.DialogueNodeData[0].PlayerText)
            {
                npcName.text = nameOfNPC;
                sentence.text = dialogueText;
            }
            else
            {
                npcName.text = nameOfNPC;
                sentence.text = ($"[TY:] {dialogueText}");
            }
                

            GetOptions();
        }
        else Debug.Log("Koniec dialogu.");
    }

    void GetNode()
    {
        var dialogueNodeData = currentDialogue.DialogueNodeData;
        foreach (DialogueNodeData d in dialogueNodeData)
        {
            if (targetNodes[placeHolderChoice] == d.NodeGUID)
            {
                quitNode = d.QuitNode;
                if (!quitNode)
                {
                    nodeGUID = d.NodeGUID;
                    dialogueText = d.DialogueText;

                    if (!d.PlayerText)
                    {
                        npcName.text = nameOfNPC;
                        sentence.text = dialogueText;
                    }
                    else
                    {
                        npcName.text = nameOfNPC;
                        sentence.text = ($"[TY:] {dialogueText}");
                    }
                }
                else Debug.Log("Koniec dialogu.");
            }
        }
    }

    void GetOptions()
    {
        options = new List<string>();
        targetNodes = new List<string>();

        var nodeLinks = currentDialogue.NodeLinks;
        foreach (NodeLinkData n in nodeLinks)
        {
            int instantiatedButtons = 0;
            if (n.BaseNodeGUID == nodeGUID)
            {
                targetNodes.Add(n.TargetNodeGUID);
                if (string.IsNullOrEmpty(n.Sentence))
                {
                    options.Add(n.PortName);
                    CreateButton(instantiatedButtons, n.PortName);
                }
                else
                {
                    options.Add(n.Sentence);
                    CreateButton(instantiatedButtons, n.Sentence);
                }
            }
        }
    }

    void CreateButton(int n, string text)
    {
        n++;
        var option = Instantiate(buttonPrefab, panel.transform);
        option.transform.localPosition = new Vector2(0, (n - 1) * (-40) - 100);
        option.transform.GetChild(0).GetComponent<Text>().text = text;
    }
}