using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject inventoryUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) SwitchInventoryUI();
    }

    void SwitchInventoryUI()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }
}
