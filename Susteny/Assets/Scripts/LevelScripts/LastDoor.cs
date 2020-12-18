using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastDoor : MonoBehaviour
{
    public GameObject Vlad;
    Inventory inv;
    public ItemInventory keys;
    bool unlocked;
    public AudioClip Sustain;
    public Material newSkybox;
    public Material nightMaterial;
    public List<MeshRenderer> objectsToChangeMaterial;
    public List<GameObject> objectsToDisable;

    void Start()
    {
        DialogueInteraction.Conversation += EventConversation;
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
        DialogueInteraction.Conversation -= EventConversation;
    }

    void EventConversation(bool b, string n, int i)
    {
        if (b == false && n == "Vlad" && i == 0)
        {
            var t = GameObject.FindGameObjectWithTag("TaskSystem").GetComponent<TaskSystem>();
            Vlad.GetComponent<LoadDialogue>().currentDialogueID = 1;
            t.hideTask("3");
            t.addTask(t.availableTasks[4]);
        }
    }
}
