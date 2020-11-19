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
                Say();
            }
            else
                MarkerLoop();
            // rekurencja bitch B)
        }
        else
        {
            switch (speakerMarker)
            {
                case "player":
                    while (marker != "npc" && marker != "player")
                    {
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
                    break;

                case "npc":
                    Debug.Log("dupa");
                    break;
            }
        }
    }

    void Say()
    {
        ShowAsSpeaker();
    }

    void NPCAnswer()
    {

    }

    void PlayerDecide()
    {

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
