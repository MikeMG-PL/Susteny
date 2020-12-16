using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
    public Item item;
    public PlayerActions playerActions;

    public void PointerDown()
    {
        playerActions.GrabFromInventory(item.model);
    }

    void Start()
    {
        gameObject.layer = 9;    
    }
}
