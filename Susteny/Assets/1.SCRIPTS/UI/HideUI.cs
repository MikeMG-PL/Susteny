using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideUI : MonoBehaviour
{
    void OnEnable()
    {
        DialogueInteraction.Talking += HideOverlayingUI;
        ViewMode.ViewingItem += HideOverlayingUI;
        PlayerActions.BrowsingInventory += HideOverlayingUI;
    }

    void OnDisable()
    {
        DialogueInteraction.Talking -= HideOverlayingUI;
        ViewMode.ViewingItem -= HideOverlayingUI;
        PlayerActions.BrowsingInventory -= HideOverlayingUI;
    }

    void HideOverlayingUI(bool b)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(!b);
        }
    }
}
