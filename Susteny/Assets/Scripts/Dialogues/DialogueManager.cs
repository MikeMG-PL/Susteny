using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public Dialogue dialogueSO;
    public int chosenDialogue;
    public string choice;          // Wybór

    string workspace; int cursor1, cursor2;
    string marker;          // Pobrany znacznik
    string speakerMarker;   // Znacznik postaci
    string optionMarker;    // X.Y
    string sentence;        // Zdanie
    string quitMarker;      // Znacznik wyjścia
    bool dialogueRunning;

    void Start()
    {
        Initialize(chosenDialogue);
    }

    void Initialize(int element)
    {
        dialogueRunning = true;
        workspace = dialogueSO.texts[element];
        workspace += "\n#end#";
    }

    void Forward()
    {
        if (Input.GetKeyDown(KeyCode.Return) && dialogueRunning)
            MarkerLoop();
    }

    void MarkerLoop()
    {
        GetMarker();
        if (marker == "npc" || marker == "player")
        {
            speakerMarker = marker;
            if (!MarkerFound())
            {
                GetSentence();
                ShowAsSpeaker();
            }
            else
                MarkerLoop();
        }
        else
        {
            switch (speakerMarker)
            {
                case "player":
                    PlayerDecide();
                    break;

                case "npc":
                    NPCAnswer();
                    break;
            }
        }
    }

    /*
     if (MarkerFound())
            {
                optionMarker = marker;
                GetMarker();
                if (marker == "npc" || marker == "player")
                {
                    speakerMarker = marker;
                    break;
                }
                if (optionMarker == choice)
                {
                    if (MarkerFound())
                        GetMarker();
                    else
                    {
                        GetSentence();
                        ShowAsSpeaker();
                    }
                }
                else
                {
                    if (MarkerFound())
                    {
                        GetMarker();
                        if (marker == choice)
                            optionMarker = marker;
                    }
                    else
                    {
                        GetSentence();
                    }
                }
            }
            else
            {
                GetSentence();
                ShowAsSpeaker();
            }
     */

    void NPCAnswer()
    {
        while (marker != "npc" && marker != "player")
        {
            CheckForEnd();
            if (!dialogueRunning)
                break;

            if (marker == choice)
                optionMarker = marker;

            while (marker != "npc" && marker != "player")
            {
                if (optionMarker == choice)
                {
                    if (MarkerFound())
                        GetMarker();
                    else
                    {
                        GetSentence();
                        ShowAsSpeaker();
                        while (marker != "npc" && marker != "player")
                        {
                            if (MarkerFound())
                                GetMarker();
                            else
                                GetSentence();
                        }
                    }

                    if (marker == "npc" || marker == "player")
                        speakerMarker = marker;
                }
                else
                {
                    while (marker != "npc" && marker != "player")
                    {
                        if (MarkerFound())
                        {
                            GetMarker();
                            if (marker == choice)
                            {
                                optionMarker = marker;
                                break;
                            }
                        }
                        else
                            GetSentence();
                    }
                    if (marker == "npc" || marker == "player")
                        speakerMarker = marker;
                }
            }

        }
    }

    void PlayerDecide()
    {
        while (marker != "npc" && marker != "player")
        {
            CheckForEnd();
            if (!dialogueRunning)
                break;

            if (MarkerFound())
            {
                GetMarker();
                if (marker == "npc" || marker == "player")
                {
                    speakerMarker = marker;
                    break;
                }
            }
            else
            {
                GetSentence();
                ShowAsOption();
            }
        }
    }

    void CheckForEnd()
    {
        if (marker == "end")
            dialogueRunning = false;
    }

    /////////////////////////////////////////////////

    void DeleteRestOfOptions()
    {
        marker = "X.Y";
        while (marker != "npc" && marker != "player" && marker != "quit" && marker != "end" && dialogueRunning)
        {
            switch (CheckMarker())
            {
                case null:
                    GetSentence();
                    break;

                case "end":
                    dialogueRunning = false;
                    break;

                case "npc":
                case "player":
                case "quit":
                    break;

                default:
                    marker = "X.Y";
                    DeleteMarker();
                    break;
            }
        }
        marker = null;
    }

    void ShowAsOption()
    {
        Debug.Log(">>> " + sentence);
    }

    void ShowAsSpeaker()
    {
        switch (speakerMarker)
        {
            case "npc":
                Debug.Log(dialogueSO.nameInDialogue.ToUpper() + ": " + sentence);
                break;

            case "player":
                Debug.Log("GRACZ: " + sentence);
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

    string GetMarker()
    {
        if (MarkerFound())
        {
            cursor1 = workspace.IndexOf("#");
            workspace = workspace.Remove(cursor1, 1);
            cursor2 = workspace.IndexOf("#");
            marker = workspace.Substring(0, cursor2);
            workspace = workspace.Remove(0, cursor2 + 2);
            return marker;
        }
        else
            return null;
    }

    string CheckMarker()
    {
        if (MarkerFound())
        {
            cursor1 = workspace.IndexOf("#");
            workspace = workspace.Remove(cursor1, 1);
            cursor2 = workspace.IndexOf("#");
            marker = workspace.Substring(0, cursor2);
            workspace.Insert(0, "#");
            return marker;
        }
        else
            return null;
    }

    void DeleteMarker()
    {
        if (MarkerFound())
        {
            cursor1 = workspace.IndexOf("#");
            workspace = workspace.Remove(cursor1, 1);
            cursor2 = workspace.IndexOf("#");
            workspace = workspace.Remove(0, cursor2 + 2);
        }
    }

    void GetSentence()
    {
        cursor1 = workspace.IndexOf("#");
        sentence = workspace.Substring(0, cursor1 - 1);
        workspace = workspace.Remove(0, cursor1);
    }

    bool MarkerFound()
    {
        if (workspace != null && workspace[0] == '#')
            return true;
        else return false;
    }

    void Update()
    {
        Forward();
    }
}
