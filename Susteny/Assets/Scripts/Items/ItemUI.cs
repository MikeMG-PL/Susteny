using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
    public Item item;
    public ViewMode viewMode;

    public void EnableViewMode()
    {
        viewMode.ToggleViewInventoryItem(true, item);
    }
}
