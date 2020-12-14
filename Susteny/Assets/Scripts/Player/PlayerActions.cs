using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] GameObject InventoryUI;

    [HideInInspector] public bool canUngrab;// Zmienna, która zapobiega jednoczesnemu podniesieniu i upuszczeniu przedmiotu
    // (ponieważ odpowiada za nie ten sam przycisk myszki)

    public bool inventoryAllowed = true;

    ItemWorld grabbedInteractable;
    ViewMode viewMode;

    public static event Action<bool> BrowsingInventory;

    private void Awake()
    {
        viewMode = GetComponent<ViewMode>();
        Prototype.LevelStart += DisallowInventorySwitching;
    }

    void DisallowInventorySwitching(bool b)
    {
        inventoryAllowed = !b;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && grabbedInteractable != null && canUngrab) Ungrab();

        else if (Input.GetKeyDown(KeyCode.E) && grabbedInteractable != null)
            TakeToInventory(grabbedInteractable);

        else if (Input.GetKeyDown(KeyCode.Mouse1) && viewMode.viewingFromInventory)
            UngrabFromInventory();

        else if (Input.GetKeyDown(KeyCode.E) && !viewMode.viewingItem && inventoryAllowed) SwitchInventoryUI();

        canUngrab = true;
    }

    public bool canGrab()
    {
        if (grabbedInteractable != null || isInventoryOpened()) return false;
        else return true;
    }

    public void ToggleInventoryUI(bool enable)
    {
        BrowsingInventory.Invoke(enable);
        InventoryUI.SetActive(enable);
    }

    public void SwitchInventoryUI()
    {
        bool b = !InventoryUI.activeSelf;
        BrowsingInventory.Invoke(b);
        InventoryUI.SetActive(b);
    }

    public bool isInventoryOpened()
    {
        return InventoryUI.activeSelf;
    }

    public void GrabFromInventory(GameObject item)
    {
        viewMode.ViewItemFromInventory(item);
    }

    public void UngrabFromInventory()
    {
        viewMode.StopViewingItemFromInventory();
    }

    public void Grab(ItemWorld interactable)
    {
        grabbedInteractable = interactable;
        grabbedInteractable.grabbed = true;
        grabbedInteractable.grabbing = true;
        grabbedInteractable.ungrabbing = false;
        grabbedInteractable.SetAllCollidersStatus(false);
        viewMode.ToggleViewMode(grabbedInteractable.gameObject, true);
    }

    void Ungrab()
    {
        grabbedInteractable.grabbed = false;
        grabbedInteractable.grabbing = false;
        grabbedInteractable.ungrabbing = true;
        grabbedInteractable.SetAllCollidersStatus(true);
        grabbedInteractable = null;
        viewMode.ToggleViewMode(null, false);
    }

    public void TakeToInventory(ItemWorld interactable)
    {
        GetComponent<Player>().inventory.Add(interactable.item, interactable.amount);
        /* Debbuging */ //GetComponent<Player>().inventory.ShowInventory();
        if (grabbedInteractable != null) Ungrab();
        Destroy(interactable.gameObject);
    }

    void OnDestroy()
    {
        Prototype.LevelStart -= DisallowInventorySwitching;
    }
}
