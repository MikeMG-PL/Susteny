using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<ItemInventory> inventory;

    public Inventory()
    {
        inventory = new List<ItemInventory>();
    }

    public void Add(Item item, int amount)
    {
        // Spróbuj znaleźć identyczny item w inventory, jeśli znaleziony, dodaj do niego odpowiednią ilość amount, jeśli nie, dodaj nowy item
        ItemInventory newItem = new ItemInventory(item, amount);
        ItemInventory itemInInventory = GetItemByReference(newItem);
        if (itemInInventory != null) itemInInventory.AddAmount(amount);
        else inventory.Add(newItem);
    }

    public List<ItemInventory> GetInventory()
    {
        return inventory;
    }

    ItemInventory GetItemByReference(ItemInventory itemInventory)
    {
        ItemInventory searchedItem = null;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].item == itemInventory.item)
            {
                searchedItem = inventory[i];
                break;
            }
        }
            return searchedItem;
    }
}
