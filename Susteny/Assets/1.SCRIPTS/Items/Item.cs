using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New item", menuName = "Inventory system/Item")]
public class Item : ScriptableObject
{
    public ItemType itemType = ItemType.Default;
    public GameObject model;
    public GameObject icon;
    public new string name = "Brak nazwy";
    [TextArea(15, 20)] public string description;
}

public enum ItemType
{
    Cube,
    Default,
}