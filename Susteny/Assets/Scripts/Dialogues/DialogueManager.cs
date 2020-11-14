using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public Dialogue dialogueSO;

    string workspace; int cursor1, cursor2;
    string marker;          // Pobrany znacznik
    string speakerMarker;   // Znacznik postaci
    string optionMarker;    // X.Y
    string sentence;        // Zdanie
    string choice;          // Wybór
    string quitMarker;      // Znacznik wyjścia
    bool dialogueRunning;

    void Start()
    {
        choice = "1.1";
        Syntax(0);
    }

    void Syntax(int ID)
    {
        Initialize(ID);
        Analyze();
    }

    void Initialize(int element)
    {
        dialogueRunning = true;
        workspace = dialogueSO.texts[element];
        workspace += "#end#";
    }

    void Analyze()
    {
        GetMarker();
        if (dialogueRunning)
        {
            switch (marker)
            {
                case "npc":
                case "player":
                    speakerMarker = marker;
                    DeeperAnalyze();
                    break;

                case "end":
                    dialogueRunning = false;
                    break;

                default:
                    optionMarker = marker;
                    DeeperAnalyze();
                    break;
            }
        }
    }

    void DeeperAnalyze()
    {
        if (dialogueRunning)
        {
            switch (GetMarkerOrSentence())
            {
                case "end":
                    dialogueRunning = false;
                    break;

                case "npc":
                case "player":
                    speakerMarker = marker;
                    Analyze();
                    break;

                case "quit":
                    Error();
                    break;

                case null:
                    GetSentence();
                    ShowAsSpeaker(sentence);
                    Analyze();
                    break;

                default:
                    optionMarker = marker;
                    PlayerOrNPCOption();
                    break;
            }
        }

    }

    void PlayerOrNPCOption()
    {
        switch (speakerMarker)
        {
            case "player":
                RecognizeQuitMarker();
                break;

            case "npc":
                if (choice == optionMarker)
                {
                    while (GetMarkerOrSentence() != null)
                    {
                        switch (GetMarkerOrSentence())
                        {
                            case "npc":
                            case "player":
                            case "quit":
                                Error();
                                break;

                            case null:
                                GetSentence();
                                ShowAsSpeaker(sentence);
                                DeleteRestOfOptions();
                                Analyze();
                                break;

                            default:
                                GetMarker();
                                break;
                        }
                    }

                }
                else
                {
                    switch (GetMarkerOrSentence())
                    {
                        case null:
                            GetSentence();
                            DeeperAnalyze();
                            break;

                        case "npc":
                        case "player":
                        case "quit":
                            Error();
                            break;

                        default:
                            GetMarker();
                            optionMarker = marker;
                            DeeperAnalyze();
                            break;
                    }
                }
                break;

            default:
                Error();
                break;
        }
    }

    void RecognizeQuitMarker()
    {
        switch (GetMarkerOrSentence())
        {
            case "quit":
                quitMarker = optionMarker;
                break;

            case null:
                break;
        }

        GetSentence();
        ShowAsOption(sentence);
        Analyze();
    }

    /////////////////////////////////////////////////

    void DeleteRestOfOptions()
    {
        while (marker != "npc" || marker != "player" || marker != "quit" || marker != "end")
        {
            GetMarker();
        }
    }

    void ShowAsOption(string sentenceToShow)
    {
        Debug.Log(">>> " + sentenceToShow);
    }

    void ShowAsSpeaker(string sentenceToShow)
    {
        switch (speakerMarker)
        {
            case "npc":
                Debug.Log(dialogueSO.nameInDialogue.ToUpper() + ": " + sentenceToShow);
                break;

            case "player":
                Debug.Log("GRACZ: " + sentenceToShow);
                break;

            default:
                Error();
                break;
        }
    }

    void Error()
    {
        Debug.LogError("Dialogue syntax error!");
        Debug.Log("MARKER: " + marker + ", SPEAKERMARKER: "
            + speakerMarker + ", OPTIONMARKER: "
            + optionMarker + ", WORKSPACE: "
            + workspace);
    }

    string GetMarkerOrSentence()
    {
        if (MarkerFound())
        {
            GetMarker();
            return marker;
        }
        else
            return null;
    }

    void GetMarker()
    {
        cursor1 = workspace.IndexOf("#");
        workspace = workspace.Remove(cursor1, 1);
        cursor2 = workspace.IndexOf("#");
        marker = workspace.Substring(0, cursor2);
        workspace = workspace.Remove(0, cursor2 + 2);
    }

    void GetSentence()
    {
        cursor1 = workspace.IndexOf("#");
        sentence = workspace.Substring(0, cursor1 - 1);
        workspace = workspace.Remove(0, cursor1);
    }

    bool MarkerFound()
    {
        if (workspace[0] == '#')
            return true;
        else return false;
    }

    void Update()
    {
        /*Debug.Log("MARKER: " + marker + ", SPEAKERMARKER: "
            + speakerMarker + ", OPTIONMARKER: "
            + optionMarker + ", WORKSPACE: "
            + workspace);*/
    }
}
