using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public RectTransform startPosition;
    public int x_start = -200;
    public int y_start = 100;
    public int x_space_between_items;
    public int y_space_between_items;
    public int number_of_colums;

    Dictionary<ItemInventory, GameObject> itemsDisplayed = new Dictionary<ItemInventory, GameObject>();

    private void Start()
    {
        CreateDisplay();
    }

    private void Update()
    {
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        for (int i = 0; i < inventory.GetInventory().Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.GetInventory()[i]))
                itemsDisplayed[inventory.GetInventory()[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.GetInventory()[i].amount.ToString("n0");

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
        obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.GetInventory()[index].amount.ToString("n0");
        itemsDisplayed.Add(inventory.GetInventory()[index], obj);
    }

    Vector3 GetPosition(int i)
    {
        return new Vector3(
            startPosition.anchoredPosition.x + (x_space_between_items * (i % number_of_colums)),
            startPosition.anchoredPosition.y + (-y_space_between_items * (i / number_of_colums)),
            0f);
    }
}
