using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public GameObject InventoryUI;
    public Transform inventoryViewTransform;
    public UIHints UIHints;

    public bool viewingItemFromInventory;

    [HideInInspector] public GameObject interactingObject;
    [HideInInspector] public ItemWorld grabbedItem;
    [HideInInspector] public ViewMode viewMode;
    [HideInInspector] public bool inventoryAllowed = true;
    [HideInInspector] public bool quittingViewModeAllowed = true;
    [HideInInspector] public bool canInteract = true;
    /// Gdy gracz stanie w wyznaczonym przez GoToPosition miejscu oraz gdy będzie patrzył na wyznaczony przez LookAt obiekt, jeśli true, włączy się kursor i wyłączy movement
    [HideInInspector] public bool showCursorOnPosition;

    SC_FPSController fpsController;
    InputManager input;

    public static event Action<bool> BrowsingInventory;

    void Awake()
    {
        Subscribe();
        viewMode = GetComponent<ViewMode>();
        fpsController = GetComponent<SC_FPSController>();
        input = InputManager.instance;
    }

    public void DisallowInventorySwitching(bool b)
    {
        inventoryAllowed = !b;
    }

    void Update()
    {
        if (input.GetKeybindDown(input.keybinds.cancel) && viewMode.interactingWithItem) StopFocusingOnObject(true);

        else if (input.GetKeybindDown(input.keybinds.cancel) && grabbedItem != null && quittingViewModeAllowed) grabbedItem.Ungrab();

        else if (input.GetKeybindDown(input.keybinds.inventory) && grabbedItem != null) TakeToInventory(grabbedItem);

        else if (input.GetKeybindDown(input.keybinds.cancel) && viewingItemFromInventory && quittingViewModeAllowed) StopViewingItemFromInventory();

        else if (input.GetKeybindDown(input.keybinds.inventory) && inventoryAllowed) SwitchInventoryUI();
    }

    public void EnableInventoryUI(bool enable)
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

    public void ViewItemFromInventory(GameObject item)
    {
        GameObject obj = CreateItemFromInventory(item);
        viewingItemFromInventory = true;
        EnableInventoryUI(false);
        viewMode.ToggleViewMode(obj, true);
    }

    GameObject CreateItemFromInventory(GameObject item)
    {
        GameObject obj = Instantiate(item);
        obj.transform.SetParent(inventoryViewTransform.parent);
        obj.transform.localPosition = inventoryViewTransform.localPosition;
        obj.transform.localScale = item.transform.localScale * 3500;
        obj.transform.localEulerAngles = item.transform.localEulerAngles;
        return obj;
    }

    public void StopViewingItemFromInventory()
    {
        viewingItemFromInventory = false;
        Destroy(viewMode.viewedItem);
        viewMode.ToggleViewMode(null, false);
    }

    public void TakeToInventory(ItemWorld itemWorld)
    {
        GetComponent<Inventory>().Add(itemWorld.item, itemWorld.amount);
        if (grabbedItem != null)
        {
            grabbedItem.GetComponent<Interactable>().StoppedInteracting();
            grabbedItem = null;
            viewMode.ToggleViewMode(null, false);
        }
        Destroy(itemWorld.gameObject);
    }

    public void FocusOnObject(GameObject item, bool disableRotating, bool switchLockControlsCursorOn)
    {
        viewMode.ToggleViewMode(item, true, disableRotating, switchLockControlsCursorOn);
    }

    public void StopFocusingOnObject(bool enableMovemenetAndCursorOff)
    {
        // Jeżeli gracz nie skończył jeszcze iść / obracać się w określonym kierunku, trzeba ręcznie to przerwać
        fpsController.ForceStopGoingRotating();

        if (interactingObject != null) interactingObject.GetComponent<Interactable>().StoppedInteracting();
        viewMode.ToggleViewMode(null, false, disableRotating: false, enableMovemenetAndCursorOff);
    }

    public void LockInteracting(bool b)
    {
        canInteract = !b;
    }

    void Subscribe()
    {
        BrowsingInventory += LockInteracting;
        DialogueInteraction.Talking += LockInteracting;
        ViewMode.ViewingItem += DisallowInventorySwitching;
        ViewMode.ViewingItem += LockInteracting;
    }

    void Unsubscribe()
    {
        BrowsingInventory -= LockInteracting;
        DialogueInteraction.Talking -= LockInteracting;
        ViewMode.ViewingItem -= DisallowInventorySwitching;
        ViewMode.ViewingItem -= LockInteracting;
    }

    void OnDisable()
    {
        Unsubscribe();
    }
}
