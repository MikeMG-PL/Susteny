using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject player;
    public RectTransform startPosition;
    public int x_space_between_items;
    public int y_space_between_items;
    public int number_of_colums;

    Inventory inventory;
    PlayerActions playerActions;

    Dictionary<ItemInventory, GameObject> itemsDisplayed = new Dictionary<ItemInventory, GameObject>();

    void Awake()
    {
        inventory = player.GetComponent<Inventory>();
        playerActions = player.GetComponent<PlayerActions>();
    }

    void Start()
    {
        CreateDisplay();
    }

    void Update()
    {
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        for (int i = 0; i < inventory.GetInventory().Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.GetInventory()[i]))
                itemsDisplayed[inventory.GetInventory()[i]].GetComponentInChildren<Text>().text = inventory.GetInventory()[i].amount.ToString("n0");

            else
                AddUIItem(i);
        }
    }

    void CreateDisplay()
    {
        for (int i = 0; i < inventory.GetInventory().Count; i++)
        {
            AddUIItem(i);
        }
    }

    void AddUIItem(int index)
    {
        var obj = Instantiate(inventory.GetInventory()[index].item.UI_Prefab, Vector3.zero, Quaternion.identity, transform);
        obj.GetComponent<RectTransform>().localPosition = GetPosition(index);
        obj.GetComponentInChildren<Text>().text = inventory.GetInventory()[index].amount.ToString("n0");

        ItemUI item = obj.GetComponent<ItemUI>();
        item.item = inventory.GetInventory()[index].item;
        item.playerActions = playerActions;

        itemsDisplayed.Add(inventory.GetInventory()[index], obj);
    }

    Vector3 GetPosition(int i)
    {
        return new Vector3(
            startPosition.localPosition.x + (x_space_between_items * (i % number_of_colums)),
            startPosition.localPosition.y + (-y_space_between_items * (i / number_of_colums)),
            0f);
    }
}
