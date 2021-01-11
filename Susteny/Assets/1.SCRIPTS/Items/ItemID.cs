using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemID : MonoBehaviour
{
    [Header("[OPTIONAL] - in a need of item model identification")]
    [Tooltip("If there is a need to eg. specify which item you take, use this identification script and assign Item Scriptable Object to this field")]
    public Item thisItem;
}
