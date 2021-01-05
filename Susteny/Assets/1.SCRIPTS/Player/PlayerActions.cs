using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] GameObject InventoryUI;

    [HideInInspector] public ItemWorld grabbedItem;
    [HideInInspector] public ViewMode viewMode;
    [HideInInspector] public bool inventoryAllowed = true;
    [HideInInspector] public bool canInteract = true;
    [HideInInspector] public bool finishedGoingAndRotatingTowardsObject = true;
    [HideInInspector] public bool showCursorOnPosition;

    SC_FPSController fpsController;

    public static event Action<bool> BrowsingInventory;

    void Awake()
    {
        Subscribe();
        viewMode = GetComponent<ViewMode>();
        fpsController = GetComponent<SC_FPSController>();
    }

    void DisallowInventorySwitching(bool b)
    {
        inventoryAllowed = !b;
    }

    void Update()
    {
        if (!finishedGoingAndRotatingTowardsObject && !fpsController.lookingAt && !fpsController.goingTo)
        {
            if (showCursorOnPosition) fpsController.LockControlsCursorOn(true);
            finishedGoingAndRotatingTowardsObject = true;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && viewMode.interactingWithItem && finishedGoingAndRotatingTowardsObject) StopFocusOnObject(true);

        else if (Input.GetKeyDown(KeyCode.Mouse1) && grabbedItem != null) grabbedItem.Ungrab();

        else if (Input.GetKeyDown(KeyCode.E) && grabbedItem != null) TakeToInventory(grabbedItem);

        else if (Input.GetKeyDown(KeyCode.Mouse1) && viewMode.viewingFromInventory) UngrabFromInventory();

        else if (Input.GetKeyDown(KeyCode.E) && inventoryAllowed) SwitchInventoryUI();
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
        GetComponent<Inventory>().Add(itemWorld.item, itemWorld.amount);
        if (grabbedItem != null)
        {
            grabbedItem = null;
            viewMode.ToggleViewMode(null, false);
        }
        Destroy(itemWorld.gameObject);
    }

    public void FocusOnObject(GameObject item, bool interact, bool switchLockControlsCursorOn)
    {
        viewMode.ToggleViewMode(item, true, interact, switchLockControlsCursorOn);
    }

    void StopFocusOnObject(bool enableMovemenetAndCursorOff)
    {
        viewMode.ToggleViewMode(null, false, interact: false, enableMovemenetAndCursorOff);
    }

    public void LookAt(Vector3 posToLook, bool interacting = false, float rotatingSpeed = 50f, float angleTolerance = 1f)
    {
        if (interacting) finishedGoingAndRotatingTowardsObject = false;
        GetComponent<SC_FPSController>().LookAt(posToLook, rotatingSpeed, angleTolerance);
    }

    public void GoToPosition(Vector3 posToGo, bool interacting = false, float goingSpeed = 2f, float positionTolerance = 0.1f)
    {
        if (interacting) finishedGoingAndRotatingTowardsObject = false;
        GetComponent<SC_FPSController>().GoTo(posToGo, goingSpeed, positionTolerance);
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
        ViewMode.ViewingItem += DisallowInventorySwitching;
        ViewMode.ViewingItem += LockInteracting;
    }

    void Unsubscribe()
    {
        BrowsingInventory -= LockInteracting;
        DialogueInteraction.Talking -= LockInteracting;
        Prototype.LevelStart -= DisallowInventorySwitching;
        ViewMode.ViewingItem -= DisallowInventorySwitching;
        ViewMode.ViewingItem -= LockInteracting;
    }

    void OnDisable()
    {
        Unsubscribe();
    }
}
