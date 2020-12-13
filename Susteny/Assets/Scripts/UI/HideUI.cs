using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideUI : MonoBehaviour
{
    public Image crosshair;
    Color currentColor;

    void Start()
    {
        DialogueInteraction.Talking += HideOverlayingUI;
        ViewMode.Viewing += HideOverlayingUI;
        Prototype.LevelStart += HideOverlayingUI;
        PlayerActions.BrowsingInventory += HideOverlayingUI;

        currentColor = crosshair.color;
    }

    void OnDisable()
    {
        DialogueInteraction.Talking -= HideOverlayingUI;
        ViewMode.Viewing -= HideOverlayingUI;
        Prototype.LevelStart -= HideOverlayingUI;
        PlayerActions.BrowsingInventory -= HideOverlayingUI;
    }

    void HideOverlayingUI(bool b)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(!b);
        }
    }

    RaycastHit _hit = new RaycastHit();
    Ray ray;

    void Update()
    {
        LookRay();
    }

    void LookRay()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out _hit, 20))
        {
            if (_hit.transform.GetComponent<ItemWorld>() != null || (_hit.transform.GetComponent<LoadDialogue>() != null && _hit.collider.isTrigger == false))
                crosshair.color = Color.green;
            else
                crosshair.color = currentColor;
        }
        else
            crosshair.color = currentColor;
    }
}
