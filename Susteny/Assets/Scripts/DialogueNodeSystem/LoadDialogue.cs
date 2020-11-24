using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Subtegral.DialogueSystem.DataContainers;

public class LoadDialogue : MonoBehaviour
{
    public string nameOfNPC;
    public int currentDialogueID;
    public int placeHolderChoice;
    public List<DialogueContainer> dialogues;
    DialogueContainer currentDialogue;

    string dialogueText;
    List<string> options;
    List<string> targetNodes;
    string nodeGUID;
    bool quitNode;

    void Start()
    {
        Load(currentDialogueID);
        FirstNode(0);
        GetOptions();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            ProcessDialogue();
    }

    // Wczytanie dialogu
    void Load(int i)
    {
        currentDialogue = dialogues[i];
    }

    // Prowadzenie dialogu - przenoszenie kwestii z wczytanego SO do gry
    void ProcessDialogue()
    {
        GetNode();
        GetOptions();
    }

    /// FUNKCJE ODCZYTUJĄCE DANE Z POBRANEGO SO W RAMACH FUNKCJI ProcessDialogue() ///


    void FirstNode(int i)
    {
        var quitNode = currentDialogue.DialogueNodeData[i].QuitNode;
        if (!quitNode)
        {
            nodeGUID = currentDialogue.DialogueNodeData[i].NodeGUID;
            dialogueText = currentDialogue.DialogueNodeData[i].DialogueText;
            Debug.Log($"{nameOfNPC}: {dialogueText}");
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
                    Debug.Log($"{nameOfNPC}: {dialogueText}");
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
            if (n.BaseNodeGUID == nodeGUID)
            {
                targetNodes.Add(n.TargetNodeGUID);
                if (string.IsNullOrEmpty(n.Sentence))
                {
                    options.Add(n.PortName);
                    Debug.Log($">>> {n.PortName}");
                }
                else
                {
                    options.Add(n.Sentence);
                    Debug.Log($">>> {n.Sentence}");
                }
            }
        }

    }
}