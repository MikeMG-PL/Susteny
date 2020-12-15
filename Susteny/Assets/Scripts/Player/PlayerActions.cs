using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] GameObject InventoryUI;

    public bool inventoryAllowed = true;
    public bool canGrab = true;

    ItemWorld grabbedInteractable;
    ViewMode viewMode;

    public static event Action<bool> BrowsingInventory;

    private void Awake()
    {
        Subscribe();
        viewMode = GetComponent<ViewMode>();
    }

    void DisallowInventorySwitching(bool b)
    {
        inventoryAllowed = !b;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && viewMode.interactingWithItem) StopInteracting();

        if (Input.GetKeyDown(KeyCode.Mouse1) && grabbedInteractable != null) Ungrab();

        else if (Input.GetKeyDown(KeyCode.E) && grabbedInteractable != null)
            TakeToInventory(grabbedInteractable);

        else if (Input.GetKeyDown(KeyCode.Mouse1) && viewMode.viewingFromInventory)
            UngrabFromInventory();

        else if (Input.GetKeyDown(KeyCode.E) && !viewMode.viewingItem && inventoryAllowed) SwitchInventoryUI();
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
        if (grabbedInteractable != null)
        {
            grabbedInteractable = null;
            viewMode.ToggleViewMode(null, false);
        }
        Destroy(interactable.gameObject);
    }

    public void Interact(GameObject item)
    {
        viewMode.ToggleViewMode(item, true, true);
    }

    void StopInteracting()
    {
        viewMode.ToggleViewMode(null, false, false);
    }

    void LockGrabbingItems(bool b)
    {
        canGrab = !b;
    }

    void Subscribe()
    {
        BrowsingInventory += LockGrabbingItems;
        DialogueInteraction.Talking += LockGrabbingItems;
        Prototype.LevelStart += DisallowInventorySwitching;
        ViewMode.ViewingItem += LockGrabbingItems;
    }

    void Unsubscribe()
    {
        BrowsingInventory += LockGrabbingItems;
        DialogueInteraction.Talking -= LockGrabbingItems;
        Prototype.LevelStart -= DisallowInventorySwitching;
        ViewMode.ViewingItem += LockGrabbingItems;
    }

    void OnDestroy()
    {
        Unsubscribe();
    }
}
