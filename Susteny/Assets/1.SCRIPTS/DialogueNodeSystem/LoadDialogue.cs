using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Subtegral.DialogueSystem.DataContainers;

public class LoadDialogue : MonoBehaviour
{
    public string nameOfNPC;
    public int currentDialogueID;
    int choice;
    public List<DialogueContainer> dialogues;
    DialogueContainer currentDialogue;
    bool dialogueStarted;

    string dialogueText;
    List<string> options;
    List<string> targetNodes;
    [HideInInspector] public List<GameObject> buttons;
    string nodeGUID;
    bool quitNode;

    GameObject panel;
    public GameObject buttonPrefab;
    Text npcName;
    Text sentence;
    public int instantiatedButtons;

    void Awake()
    {
        panel = GameObject.FindGameObjectWithTag("DialoguePanel");
    }

    // Wczytanie dialogu
    public void Load(int i)
    {
        if (dialogues.Count > 0)
        {
            currentDialogue = dialogues[i];
            if (currentDialogue == null)
                Debug.LogError($"Beware! NPC \"{name}\", a.k.a. \"{nameOfNPC}\" has empty places for dialogue data! Assign them in the inspector.");

            npcName = panel.GetComponent<Panel>().npcName;
            sentence = panel.GetComponent<Panel>().sentence;
            Clear();
        }

    }

    void Clear()
    {
        instantiatedButtons = 0;
        dialogueStarted = false;
        dialogueText = null;
        options?.Clear();
        targetNodes?.Clear();
        buttons = new List<GameObject>();
        options = new List<string>();
        targetNodes = new List<string>();
        nodeGUID = null;
        quitNode = false;
    }

    // Prowadzenie dialogu - przenoszenie kwestii z wczytanego SO do gry
    public void ProcessDialogue()
    {
        if (!dialogueStarted && dialogues.Count > 0)
            FirstNode();
        else if(dialogues.Count > 0)
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
        Clear();
        var target = currentDialogue.NodeLinks[0].TargetNodeGUID;
        var dialogueNodeData = currentDialogue.DialogueNodeData;

        foreach (DialogueNodeData d in dialogueNodeData)
        {
            if (target == d.NodeGUID && !d.QuitNode)
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

                GetOptions();
            }
        }
    }

    void GetNode()
    {
        var dialogueNodeData = currentDialogue.DialogueNodeData;
        foreach (DialogueNodeData d in dialogueNodeData)
        {
            if (targetNodes[choice] == d.NodeGUID)
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
                else
                {
                    GetComponent<DialogueInteraction>().Talk(false);
                    DestroyButtons();
                }
            }
        }
    }

    void GetOptions()
    {
        options = new List<string>();
        targetNodes = new List<string>();

        instantiatedButtons = 0;
        var nodeLinks = currentDialogue.NodeLinks;
        foreach (NodeLinkData n in nodeLinks)
        {
            if (n.BaseNodeGUID == nodeGUID)
            {
                // Sprawdzenie czy ta opcja kończy dialog
                var nodes = currentDialogue.DialogueNodeData;
                bool quitOption = false;
                foreach (DialogueNodeData d in nodes)
                {
                    if (n.TargetNodeGUID == d.NodeGUID && d.QuitNode)
                        quitOption = true;
                }

                // Przekazanie zdania lub tytułu opcji na przycisk
                targetNodes.Add(n.TargetNodeGUID);
                if (string.IsNullOrEmpty(n.Sentence))
                {
                    options.Add(n.PortName);
                    if (!quitOption)
                        CreateButton(n.PortName);
                    else
                        CreateButton($"[ZAKOŃCZ] {n.PortName}");
                }
                else
                {
                    options.Add(n.Sentence);
                    if (!quitOption)
                        CreateButton(n.Sentence);
                    else
                        CreateButton($"[ZAKOŃCZ] {n.Sentence}");
                }
            }
        }
    }

    void CreateButton(string text)
    {
        instantiatedButtons++;
        var option = Instantiate(buttonPrefab, panel.transform.GetChild(0));

        option.transform.localPosition = new Vector2(0, (instantiatedButtons - 1) * (-40) - 100);
        option.transform.GetChild(0).GetComponent<Text>().text = text;
        option.GetComponent<ButtonID>().buttonID = instantiatedButtons - 1;

        buttons.Add(option);
        option.GetComponent<Button>().onClick.AddListener(delegate { DBG(option.GetComponent<ButtonID>().buttonID); });
    }

    void DBG(int n)
    {
        choice = n;

        DestroyButtons();
        GetNode();
        GetOptions();
    }

    void DestroyButtons()
    {
        foreach (GameObject o in buttons)
        {
            Destroy(o);
        }
        buttons?.Clear();
    }
}