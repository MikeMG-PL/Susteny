using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUI : MonoBehaviour
{
    void Start()
    {
        DialogueInteraction.Talking += HideOverlayingUI;
    }

    void OnDisable()
    {
        DialogueInteraction.Talking -= HideOverlayingUI;
    }

    void HideOverlayingUI(bool b)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(!b);
        }
        
    }
}
