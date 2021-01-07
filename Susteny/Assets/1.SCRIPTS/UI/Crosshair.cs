using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public Image crosshair;

    new Camera camera;
    GameObject player;
    GameObject lastHit;
    Color defaultColor;
    Color interactColor = Color.green;
    RaycastHit _hit = new RaycastHit();
    Ray ray;
    Vector3 interactablePos;
    bool hitInteractable;
    float interactionDistance;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camera = player.GetComponent<Player>().camera;

        defaultColor = crosshair.color;
    }

    void Update()
    {
        LookRay();
    }

    void LookRay()
    {
        ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out _hit, 20))
        {
            // Jeśli *nadal* patrzymy na coś z czym możemy przeprowadzić interakcję, ale odsunęliśmy się za daleko lub przybliżyliśmy się
            if (ReferenceEquals(lastHit, _hit.transform.gameObject) && hitInteractable)
            {
                interactablePos = _hit.transform.position; // Musimy zaktualizować pozycję przedmiotu (być może został przez gracza podniesiony, odłożony, itp.)
                if (NotTooFar()) crosshair.color = interactColor;
                else crosshair.color = defaultColor;
            }

            // Jeśli *nadal* patrzymy na ten sam obiekt, ale nie jest on interactable
            else if (ReferenceEquals(lastHit, _hit.transform.gameObject) && !hitInteractable) crosshair.color = defaultColor;

            // Jeśli patrzymy na inny obiekt
            else
            {
                lastHit = _hit.transform.gameObject;

                if (HitInteractable(_hit.transform))
                {
                    if (NotTooFar()) crosshair.color = interactColor;
                    else crosshair.color = defaultColor;
                }

                else
                    crosshair.color = defaultColor;
            }
        }

        else
        {
            lastHit = null;
            crosshair.color = defaultColor;
        }

    }

    // Sprawdzanie czy natknięto się na przedmiot, lub osobę, z którą można wejść w interakcję
    bool HitInteractable(Transform _transform)
    {
        hitInteractable = true;

        if (_transform.GetComponent<Interactable>() != null && _transform.GetComponent<Interactable>().crosshairColor != CrosshairColor.nonInteractive)
        {
            interactionDistance = _transform.GetComponent<Interactable>().interactionDistance;
            interactablePos = _transform.position;
        }

        else if (_transform.GetComponent<ChildTriggerItemAction>() != null && _transform.GetComponentInParent<Interactable>().crosshairColor != CrosshairColor.nonInteractive)
        {
            interactionDistance = _transform.GetComponentInParent<Interactable>().interactionDistance;
            interactablePos = _transform.GetComponentInParent<Interactable>().transform.position;
        }

        else if (_transform.GetComponent<LoadDialogue>() != null)
        {
            interactionDistance = _transform.GetComponent<DialogueInteraction>().interactionDistance;
            interactablePos = _transform.position;
        }

        else
            hitInteractable = false;

        return hitInteractable;
    }

    bool NotTooFar()
    {
        float distance = Vector3.Distance(interactablePos, player.transform.position);
        if (distance <= interactionDistance) return true;
        else return false;
    }
}

public enum CrosshairColor
{
    defaultColor,
    interactive,
    nonInteractive,
}
