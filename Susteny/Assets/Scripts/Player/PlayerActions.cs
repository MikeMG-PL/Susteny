using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] GameObject InventoryUI;
    ItemWorld grabbedInteractable;
    ViewMode viewMode;

    [HideInInspector] public bool canUngrab; // Zmienna, która zapobiega jednoczesnemu podniesieniu i upuszczeniu przedmiotu (ponieważ odpowiada za nie ten sam przycisk myszki)

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

        if (Input.GetKeyDown(KeyCode.Escape) && grabbedInteractable == null && GetComponent<ViewMode>().active)
            GetComponent<ViewMode>().ToggleViewInventoryItem(false);

        if (Input.GetKeyDown(KeyCode.E) && !viewMode.active) ToggleInventoryUI();

        canUngrab = true;
    }

    public void ToggleInventoryUI(bool toggle = true, bool b = false)
    {
        if (InventoryUI == null)
        {
            Debug.LogError("Nie ma przypisanego obiektu InventoryUI!");
            return;
        }

        // Jeżeli wywołano z (domyślnym) toggle = true, widoczność inventoryUI zostanie przełączona na przeciwną wartość do aktualnej
        // (jeśli jest włączona, zostanie wyłączona)
        // W przeciwnym wypadku, programista podaje pożądaną wartość w zmiennej b
        bool enable;
        if (toggle) enable = !InventoryUI.activeSelf;
        else enable = b;
        BrowsingInventory.Invoke(enable);
        InventoryUI.SetActive(enable);
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
