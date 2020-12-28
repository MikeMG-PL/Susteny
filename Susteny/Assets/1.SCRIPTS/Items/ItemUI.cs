using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector] public Item item;
    [HideInInspector] public PlayerActions playerActions;

    public void OnPointerDown(PointerEventData eventData)
    {
        playerActions.GrabFromInventory(item.model);
    }

    void Start()
    {
        gameObject.layer = 9;    
    }
}
