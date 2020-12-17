using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastDoor : MonoBehaviour
{
    public GameObject Vlad;
    Inventory inv;
    public ItemInventory keys;
    bool unlocked;

    void Start()
    {
        DialogueInteraction.Conversation += ConversationEvent;
        inv = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    void Update()
    {
        if (inv.inventory.Count > 0 && !unlocked)
        {
            for (int i = 0; i < inv.inventory.Count; i++)
            {
                if (inv.inventory[i].item.name == "Klucz")
                {
                    GetComponent<AnyDoor>().UnlockDoor();
                    unlocked = true;
                }
            }
        }
    }

    void OnDestroy()
    {
        DialogueInteraction.Conversation -= ConversationEvent;
    }

    void ConversationEvent(bool b, string n, int i)
    {
        if (b == false && n == "Władysław Strzemiński" && i == 0)
        {
            Vlad.GetComponent<LoadDialogue>().currentDialogueID = 1;
            var t = GameObject.FindGameObjectWithTag("TaskSystem").GetComponent<TaskSystem>();
            t.hideTask("2");
            t.addTask(t.availableTasks[3]);
        }
    }
}
