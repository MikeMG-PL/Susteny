using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public InventoryUI inventoryUI;

    void Start()
    {
        inventoryUI.Initialization();
    }
}
