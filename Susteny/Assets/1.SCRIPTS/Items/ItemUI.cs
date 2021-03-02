using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public Item item;
    [HideInInspector] public InventoryUI inventoryUI;
    [HideInInspector] public PlayerActions playerActions;
    [HideInInspector] public GameObject description;
    [HideInInspector] public bool descriptionVisible;

    public void OnPointerDown(PointerEventData eventData)
    {
        playerActions.ViewItemFromInventory(item.model);
    }

    void Start()
    {
        gameObject.layer = 9;    
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (descriptionVisible) return;
        descriptionVisible = true;
        // TODO: lepsze ustawianie opisu względem przedmiotu, aktualnie jest mocno "na czuja"
        inventoryUI.itemDescParent.transform.position = new Vector3(transform.position.x + inventoryUI.itemDescPosActualOffset, transform.position.y - 20f, transform.position.z);
        description = Instantiate(inventoryUI.itemDescPrefab, inventoryUI.itemDescParent.transform);
        description.GetComponent<ItemDescriptionUI>().name.text = item.name;
        description.GetComponent<ItemDescriptionUI>().desc.text = item.description;
        inventoryUI.itemThatDescrptionIsVisible = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!description) return;
        DestoryDescription();
    }

    public void DestoryDescription()
    {
        Destroy(description);
        descriptionVisible = false;
        inventoryUI.itemThatDescrptionIsVisible = null;
    }
}
