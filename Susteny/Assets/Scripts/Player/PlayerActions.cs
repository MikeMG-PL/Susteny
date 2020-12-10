using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] GameObject InventoryUI;

    [HideInInspector] public bool canUngrab; // Zmienna, która zapobiega jednoczesnemu podniesieniu i upuszczeniu przedmiotu
    // (ponieważ odpowiada za nie ten sam przycisk myszki)

    ItemWorld grabbedInteractable;
    ViewMode viewMode;

    public static event Action<bool> BrowsingInventory;

    private void Start()
    {
        viewMode = GetComponent<ViewMode>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && grabbedInteractable != null && canUngrab) Ungrab();

        if (Input.GetKeyDown(KeyCode.Mouse1) && grabbedInteractable != null)
            TakeToInventory(grabbedInteractable);

        if (Input.GetKeyDown(KeyCode.Mouse1) && viewMode.viewingFromInventory)
            UngrabFromInventory();

        if (Input.GetKeyDown(KeyCode.E) && !viewMode.viewingItem) SwitchInventoryUI();

        canUngrab = true;
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
        if (grabbedInteractable != null) Ungrab();
        Destroy(interactable.gameObject);
    }
}
