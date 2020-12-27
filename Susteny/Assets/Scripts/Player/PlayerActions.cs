using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] GameObject InventoryUI;

    public bool inventoryAllowed = true;
    public bool canGrab = true;
    public bool canTalk = true;
    public bool canOpenDoor = true;
    public bool canInteract = true;

    public ItemWorld grabbedItem;
    public ViewMode viewMode;

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
        if (Input.GetKeyDown(KeyCode.Mouse1) && viewMode.interactingWithItem) StopWatchingObject();

        else if (Input.GetKeyDown(KeyCode.Mouse1) && grabbedItem != null) grabbedItem.Ungrab();

        else if (Input.GetKeyDown(KeyCode.E) && grabbedItem != null) TakeToInventory(grabbedItem);

        else if (Input.GetKeyDown(KeyCode.Mouse1) && viewMode.viewingFromInventory) UngrabFromInventory();

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

    public void TakeToInventory(ItemWorld itemWorld)
    {
        GetComponent<Player>().inventory.Add(itemWorld.item, itemWorld.amount);
        if (grabbedItem != null)
        {
            grabbedItem = null;
            viewMode.ToggleViewMode(null, false);
        }
        Destroy(itemWorld.gameObject);
    }

    public void WatchObject(GameObject item)
    {
        viewMode.ToggleViewMode(item, true, true);
    }

    void StopWatchingObject()
    {
        viewMode.ToggleViewMode(null, false, false);
    }

    void LockInteracting(bool b)
    {
        canInteract = !b;
    }

    void Subscribe()
    {
        BrowsingInventory += LockInteracting;
        DialogueInteraction.Talking += LockInteracting;
        Prototype.LevelStart += DisallowInventorySwitching;
        ViewMode.ViewingItem += LockInteracting;
    }

    void Unsubscribe()
    {
        BrowsingInventory -= LockInteracting;
        DialogueInteraction.Talking -= LockInteracting;
        Prototype.LevelStart -= DisallowInventorySwitching;
        ViewMode.ViewingItem -= LockInteracting;
    }

    void OnDisable()
    {
        Unsubscribe();
    }
}
