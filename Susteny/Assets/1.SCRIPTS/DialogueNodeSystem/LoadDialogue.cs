using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Subtegral.DialogueSystem.DataContainers;

public class LoadDialogue : MonoBehaviour
{
    // Public
    [HideInInspector] public List<GameObject> buttons;
    [HideInInspector] public int instantiatedButtons;
    public string nameOfNPC;
    public int currentDialogueID;
    public List<DialogueContainer> dialogues;
    public GameObject buttonPrefab;
    public GameObject quitButton;
    private Color chosenOptionColor;
    public float chosenOptionAlpha = 0.8f;

    // Private
    int choice;
    bool dialogueStarted, quitNode;
    string dialogueText, nodeGUID;

    List<string> options, targetNodes;
    DialogueContainer currentDialogue;
    GameObject panel;
    TMP_Text npcName, sentence;
    Color defaultColor;

    void Awake()
    {
        panel = GameObject.FindGameObjectWithTag("DialoguePanel");
    }

    // Wczytanie dialogu
    public void Load(int i)
    {
        if (dialogues.Count > 0)
        {
            defaultColor = buttonPrefab.transform.GetChild(0).GetComponent<TMP_Text>().color;
            chosenOptionColor = new Color(defaultColor.r, defaultColor.g * 0.725f, defaultColor.b, chosenOptionAlpha * defaultColor.a);

            currentDialogue = dialogues[i];
            if (currentDialogue == null)
                Debug.LogError($"Beware! NPC \"{name}\", a.k.a. \"{nameOfNPC}\" has empty places for dialogue data! Assign them in the inspector.");

            if(currentDialogue.canLeaveDialogue)
            {
                GameObject leaveConversationButton;
                leaveConversationButton = Instantiate(quitButton, panel.transform.GetChild(0));
                leaveConversationButton.GetComponent<Button>().onClick.AddListener(delegate { GetComponent<DialogueInteraction>().Talk(false); });
            }

            npcName = panel.GetComponent<Panel>().npcName;
            sentence = panel.GetComponent<Panel>().sentence;
            Clear();
            ResetChosenMarks();
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
        else if (dialogues.Count > 0)
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
        var nodeLinkData = currentDialogue.NodeLinks;
        foreach (DialogueNodeData d in dialogueNodeData)
        {
            if (targetNodes[choice] == d.NodeGUID)
            {
                quitNode = d.QuitNode;
                if (!quitNode)
                {
                    nodeGUID = d.NodeGUID;
                    npcName.text = nameOfNPC;

                    bool checking = true;
                    // Ustawienie dialogu głównego lub alternatywnego zależnie czy gracz wrócił do opcji
                    foreach (NodeLinkData n in nodeLinkData)
                    {
                        if (n.BaseNodeGUID == nodeGUID && checking)
                        {
                            if(n.WasChosen && !string.IsNullOrEmpty(d.AlternateText))
                            {
                                dialogueText = d.AlternateText;
                                checking = false;
                            }
                            else
                                dialogueText = d.DialogueText;
                        }
                    }

                    // Przekształcenie opcji w zależności czy jest wypowiadana przez gracza czy nie
                    if (!d.PlayerText)
                        sentence.text = dialogueText;
                    else
                        sentence.text = $"[TY:] {dialogueText}";
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
                bool wasChosen;

                if (n.GrayOut) wasChosen = n.WasChosen;
                else wasChosen = false;

                foreach (DialogueNodeData d in nodes)
                {
                    if (n.TargetNodeGUID == d.NodeGUID)
                        quitOption = d.QuitNode;
                }

                // Przekazanie zdania lub tytułu opcji na przycisk
                targetNodes.Add(n.TargetNodeGUID);
                options.Add(n.PortName);

                if (!quitOption)
                    WasChosen(CreateButton(n.PortName, n), wasChosen);
                else
                    WasChosen(CreateButton($"(Zakończ) {n.PortName}", n), wasChosen);
            }
        }
    }

    void WasChosen(GameObject buttonObject, bool chosen)
    {
        if (chosen)
            buttonObject.GetComponentInChildren<TMP_Text>().color = chosenOptionColor;
        else
            buttonObject.GetComponentInChildren<TMP_Text>().color = defaultColor;
    }

    void QuitButton(GameObject q)
    {
        //q.GetComponent<Button>().onClick.AddListener(delegate { GetComponent<DialogueInteraction>().Talk(false); });
    }

    GameObject CreateButton(string text, NodeLinkData nodelinks)
    {
        instantiatedButtons++;
        var option = Instantiate(buttonPrefab, panel.transform.GetChild(0));

        option.transform.localPosition = new Vector2(0, (instantiatedButtons - 1) * (-40) - 100);
        option.transform.GetChild(0).GetComponent<TMP_Text>().text = text;
        option.GetComponent<ButtonID>().buttonID = instantiatedButtons - 1;

        buttons.Add(option);
        option.GetComponent<Button>().onClick.AddListener(delegate { OnClick(option.GetComponent<ButtonID>().buttonID, nodelinks); });
        return option;
    }

    void OnClick(int n, NodeLinkData link)
    {
        choice = n;
        link.WasChosen = true;

        DestroyButtons();
        GetNode();
        GetOptions();
    }

    void DestroyButtons()
    {
        foreach (GameObject o in buttons)
            Destroy(o);

        buttons?.Clear();
    }

    public void ResetChosenMarks()
    {
        var nodeLinks = currentDialogue.NodeLinks;
        foreach (NodeLinkData links in nodeLinks)
            links.WasChosen = false;
    }
}