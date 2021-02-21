using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject viewModePosition;
    public new Camera camera;
    public GameObject focusCamera;
    public GameObject postProccessCamera;

    Interactable interactableItem;
    DialogueInteraction person;
    PlayerActions playerActions;
    InputManager input;

    Ray ray;
    RaycastHit _hit = new RaycastHit();

    private void Awake()
    {
        playerActions = GetComponent<PlayerActions>();
        input = InputManager.instance;
    }

    void LateUpdate()
    {
        TryToInteract();
    }

    void TryToInteract()
    {

        if (input.GetKeybindDown(input.keybinds.interact) && playerActions.canInteract)
        {
            ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out _hit, 20))
            {
                if (HitInteractable(_hit.transform)) _hit.transform.GetComponent<Interactable>().TryToTriggerAction();
                else if (HitNPC(_hit.transform)) _hit.transform.GetComponent<DialogueInteraction>().TryToTalk();
                else if (HitInteractableChild(_hit.transform)) _hit.transform.GetComponent<ChildTriggerItemAction>().interactable.TryToTriggerAction();
            }
        }
    }

    bool HitNPC(Transform _transform)
    {
        if (_transform.GetComponent<DialogueInteraction>() != null) return true;
        else return false;
    }

    bool HitInteractableChild(Transform _transform)
    {
        if (_transform.GetComponent<ChildTriggerItemAction>() != null) return true;
        else return false;
    }

    bool HitInteractable(Transform _transform)
    {
        if (_transform.GetComponent<Interactable>() != null) return true;
        else return false;
    }
}
