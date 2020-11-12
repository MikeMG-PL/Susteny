using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public Dialogue dialogueSO;

    string workspace; int cursor1, cursor2;
    string marker, talkMarker, optionMarker;
    string sentence;
    bool dialogueRunning;

    void Start()
    {
        StartDialogue(0);
    }

    void StartDialogue(int ID)
    {
        dialogueRunning = true;
        workspace = dialogueSO.texts[ID];
        GetMarker();
    }

    void Next()
    {
        if (Input.GetKeyDown(KeyCode.Return) && dialogueRunning)
        {
            switch (MarkerFound())
            {
                case true:
                    GetMarker();
                    break;

                case false:
                    GetSentence();
                    break;
            }
        }
    }

    void GetMarker()
    {
        cursor1 = workspace.IndexOf("#");
        workspace = workspace.Remove(cursor1, 1);
        cursor2 = workspace.IndexOf("#");
        marker = workspace.Substring(0, cursor2);

        switch (marker)
        {
            case "npc":
            case "player":
                talkMarker = marker;
                workspace = workspace.Remove(0, cursor2 + 2);
                break;

            case "end":
                dialogueRunning = false;
                break;

            default:
                optionMarker = marker;
                workspace = workspace.Remove(0, cursor2 + 2);
                break;

        }
    }

    void GetSentence()
    {
        cursor1 = workspace.IndexOf("#");
        sentence = workspace.Substring(0, cursor1 - 1);
        workspace = workspace.Remove(0, cursor1);

        if (talkMarker == "player")
            Debug.Log("GRACZ: " + sentence);
        else
            Debug.Log(dialogueSO.nameInDialogue.ToUpper() + ": " + sentence);
    }

    bool MarkerFound()
    {
        if (workspace[0] == '#')
            return true;
        else return false;
    }

    void Update()
    {
        Next();
    }








    /* 
        Znaczniki:

        #end# - Oznacza koniec dialogu
        #npc# - Ta kwestia jest wypowiadana przez NPC
        #player# - Ta kwestia jest wypowiadana przez gracza
        #save# - Poniższy wybór zostanie zapisany do pliku
        #X.Y# (po znaczniku #player) - X jest wachlarzem opcji do wyboru, Y - konkretną opcją
        #X.Y# (po znaczniku #npc) - jest odpowiedzią na opcję X.Y wybraną przez gracza

        Reguły:

        1) Po znaczniku #player# oraz #npc# nie może być pustki
        
        2) #player#
           #X.Y# musi być stale połączony z

           #npc#
           #X.Y#
        
           - każdy wybór gracza musi mieć przypisaną odpowiadającą sentencję NPC

        3) Po znaczniku #save# musi się znajdować znacznik #X.Y#

        4) Po każdym znaczniku musi być ENTER

        5) Syntax musi się kończyć znacznikiem #end#
        
        Przykład:

        #npc#
        W końcu jesteś. Ile można czekać?
        #player#
        #1.1#
        Musiałem załatwić coś pilnego.
        #1.2#
        Ciesz się, że w ogóle przyszedłem.
        #npc#
        #1.1#
        #1.2#
        Nie interesują mnie twoje wymówki.
        #end#
        */
}
