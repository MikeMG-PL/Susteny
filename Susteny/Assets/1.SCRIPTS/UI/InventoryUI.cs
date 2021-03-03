using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject player;
    public GameObject blankSlotPrefab;
    public GameObject itemDescPrefab;
    public GameObject itemDescParent;
    public int maxInventorySlots = 28;
    public float itemDescPosOriginalOffset = 180f;

    [HideInInspector] public float itemDescPosActualOffset;
    [HideInInspector] public GameObject itemThatDescrptionIsVisible;

    Inventory inventory;
    PlayerActions playerActions;

    Dictionary<ItemInventory, GameObject> itemsDisplayed = new Dictionary<ItemInventory, GameObject>();
    List<GameObject> blankSlots = new List<GameObject>();

    public void Initialization()
    { 
        inventory = player.GetComponent<Inventory>();
        playerActions = player.GetComponent<PlayerActions>();
        itemDescPosActualOffset = itemDescPosOriginalOffset;
        CreateDisplay();
    }

    // Odległość opisu od przedmiotu zależy od wielkości ekranu, ciężko zrobić to inaczej bo opis nie może być dzieckiem 
    float originalWidth = 1124f; // Wielkość okna, którą miałem podczas ustawiania elementu UI
    float oldWidth = 1124f;
    void Update()
    {
        if (oldWidth != Screen.width)
        {
            itemDescPosActualOffset = itemDescPosOriginalOffset * (Screen.width / originalWidth);
            oldWidth = Screen.width;
        }
    }

    public void OpenInventory(bool b)
    {
        if (!b && itemThatDescrptionIsVisible != null) itemThatDescrptionIsVisible.GetComponent<ItemUI>().DestoryDescription();
        gameObject.SetActive(b);
        UpdateDisplay();
    }

    // TODO: Usuwanie przedmiotów z eq, jeśli potrzebne
    void UpdateDisplay()
    {
        Text amount;
        for (int i = 0; i < inventory.GetInventory().Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.GetInventory()[i]))
            {
                amount = itemsDisplayed[inventory.GetInventory()[i]].GetComponentInChildren<Text>();
                if (amount != null)
                    amount.text = inventory.GetInventory()[i].amount.ToString("n0");
            }

            else
                AddUIItem(i);
        }
    }

    void CreateDisplay()
    {
        for (int i = 0; i < inventory.GetInventory().Count; i++)
        {
            AddUIItem(i, true);
        }

        for (int i = 0; i < maxInventorySlots - inventory.GetInventory().Count; i++)
        {
            AddBlankSlot();
        }
    }

    void AddBlankSlot()
    {
        var obj = Instantiate(blankSlotPrefab, transform);
        blankSlots.Add(obj);
    }

    void AddUIItem(int index, bool init = false)
    {
        var obj = Instantiate(inventory.GetInventory()[index].item.icon, Vector3.zero, Quaternion.identity, transform);
        if (!init) // Remove one blank slot
        {
            Destroy(blankSlots[blankSlots.Count - 1]);
            blankSlots.RemoveAt(blankSlots.Count - 1);
            obj.transform.SetSiblingIndex(itemsDisplayed.Count);
        }
        if (obj.GetComponentInChildren<Text>() != null) obj.GetComponentInChildren<Text>().text = inventory.GetInventory()[index].amount.ToString("n0");

        ItemUI itemUI = obj.GetComponent<ItemUI>();
        itemUI.item = inventory.GetInventory()[index].item;
        itemUI.inventoryUI = this;
        itemUI.playerActions = playerActions;

        itemsDisplayed.Add(inventory.GetInventory()[index], obj);
    }
}
