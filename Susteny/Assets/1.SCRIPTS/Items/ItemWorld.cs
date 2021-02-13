using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public Item item;
    public ItemAction action;
    public int amount = 1;

    [HideInInspector] public bool grabbed;

    Vector3 startPosition;
    Quaternion startRotation;
    Interactable interactable;
    Player playerScript;
    PlayerActions playerActions;
    bool grabbing;
    bool ungrabbing;
    float moveToViewModeRotSpeed = 5; // 3.5f - MIN, 10 - MAX
    float moveToViewModePosSpeed = 0.05f; //0.03f - MIN

    void Start()
    {
        interactable = GetComponent<Interactable>();
        playerScript = interactable.playerScript;
        playerActions = interactable.playerActions;

        // Pozycja do której wracać będą przedmioty, które można podnieść, obejrzeć i z powrotem odstawić
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        if (grabbing)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, playerScript.viewModePosition.transform.position, moveToViewModePosSpeed);

            if (transform.position == playerScript.viewModePosition.transform.position) grabbing = false;
        }

        else if (ungrabbing)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, startRotation, moveToViewModeRotSpeed);
            transform.position = Vector3.MoveTowards(transform.position, startPosition, moveToViewModePosSpeed);

            if (transform.position == startPosition && transform.rotation == startRotation)
            {
                ungrabbing = false;
            }
        }
    }

    public void Interact()
    {
        if (action == ItemAction.grabbable) Grab();
        else if (action == ItemAction.takeable) Take();
    }

    // If grabbable 
    public void Grab()
    {
        if (grabbed) return;

        grabbed = true;
        grabbing = true;
        ungrabbing = false;

        interactable.SetAllCollidersStatus(false);
        playerActions.grabbedItem = this;
        playerActions.viewMode.ToggleViewMode(gameObject, true);
    }

    public void Ungrab()
    {
        grabbed = false;
        grabbing = false;
        ungrabbing = true;

        interactable.StoppedInteracting();
        interactable.SetAllCollidersStatus(true);
        playerActions.grabbedItem = null;
        playerActions.viewMode.ToggleViewMode(null, false);
    }

    // If takeable
    public void Take()
    {
        interactable.playerActions.TakeToInventory(this);
    }
}

public enum ItemAction
{
    grabbable,
    takeable
}