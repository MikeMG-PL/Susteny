using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    ItemWorld grabbedInteractable;
    ViewMode viewMode;

    private void Start()
    {
        viewMode = GetComponent<ViewMode>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1) && grabbedInteractable != null) Ungrab();

        if (Input.GetKey(KeyCode.E) && grabbedInteractable != null)
            TakeToInventory(grabbedInteractable);
    }

    public void Grab(ItemWorld interactable)
    {
        grabbedInteractable = interactable;
        grabbedInteractable.grabbed = true;
        grabbedInteractable.grabbing = true;
        grabbedInteractable.ungrabbing = false;
        grabbedInteractable.SetAllCollidersStatus(false);
        viewMode.ToggleViewMode(grabbedInteractable, true);
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
