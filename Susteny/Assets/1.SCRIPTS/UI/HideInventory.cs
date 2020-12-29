using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInventory : MonoBehaviour
{
    void OnEnable()
    {
        DialogueInteraction.Talking += NoEQduringDialogue;
    }

    void OnDisable()
    {
        DialogueInteraction.Talking -= NoEQduringDialogue;
    }

    void HideOverlayingUI(bool b)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(!b);
        }
    }

    void NoEQduringDialogue(bool b)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActions>().inventoryAllowed = !b;
        }
    }
}
