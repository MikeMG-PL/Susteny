using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Crosshair : MonoBehaviour
{
    public Image crosshair;
    public UIHints hints;

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

    List<TMP_Text> allHints = new List<TMP_Text>();
    string interactionHint = string.Empty;

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
                if (NotTooFar()) return;
                else HitNothing();
            }

            // Jeśli *nadal* patrzymy na ten sam obiekt, ale nie jest on interactable
            else if (ReferenceEquals(lastHit, _hit.transform.gameObject) && !hitInteractable) return;

            // Jeśli patrzymy na inny obiekt
            else
            {
                lastHit = _hit.transform.gameObject;

                if (InteractableWasHit(_hit.transform))
                {
                    if (NotTooFar())
                    {
                        HitInteractable();
                    }
                    else HitNothing();
                }

                else
                    HitNothing();
            }
        }

        else
        {
            lastHit = null;
            HitNothing();
        }

    }

    // Sprawdzanie czy natknięto się na przedmiot, lub osobę, z którą można wejść w interakcję
    bool InteractableWasHit(Transform _transform)
    {
        hitInteractable = true;

        if (_transform.GetComponent<Interactable>() != null && _transform.GetComponent<Interactable>().crosshairColor != CrosshairColor.nonInteractive)
        {
            Interactable interactable = _transform.GetComponent<Interactable>();
            interactionDistance = interactable.interactionDistance;
            interactablePos = _transform.position;
            interactionHint = interactable.interactionHint;
        }

        else if (_transform.GetComponent<ChildTriggerItemAction>() != null && _transform.GetComponentInParent<Interactable>().crosshairColor != CrosshairColor.nonInteractive)
        {
            Interactable interactable = _transform.GetComponentInParent<Interactable>();
            interactionDistance = interactable.interactionDistance;
            interactablePos = interactable.transform.position;
            interactionHint = interactable.interactionHint;
        }

        else if (_transform.GetComponent<LoadDialogue>() != null)
        {
            DialogueInteraction npc = _transform.GetComponent<DialogueInteraction>();
            interactionDistance = npc.interactionDistance;
            interactablePos = _transform.position;
            interactionHint = npc.interactionHint;
        }

        else
            hitInteractable = false;

        return hitInteractable;
    }

    void HitInteractable()
    {
        crosshair.color = interactColor;
        if (string.IsNullOrEmpty(interactionHint)) interactionHint = hints.defaultInteractionHint;
        hints.interactionHint.text = interactionHint;
        hints.interactionHint.gameObject.SetActive(true);
    }

    void HitNothing()
    {
        crosshair.color = defaultColor;
        hints.interactionHint.gameObject.SetActive(false);
        hints.interactionHint.text = string.Empty;
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
