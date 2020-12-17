using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastDoor : MonoBehaviour
{
    Inventory inv;
    public ItemInventory keys;
    bool unlocked;

    void Start()
    {
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
}
