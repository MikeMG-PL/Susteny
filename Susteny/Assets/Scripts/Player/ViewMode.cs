using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class ViewMode : MonoBehaviour
{
    public Transform inventoryViewTransform;
    public float rotationSpeed = 0.5f;
    public float focusSpeed = 30f;

    [HideInInspector] public bool viewingItem;
    [HideInInspector] public bool viewingFromInventory;
    [HideInInspector] public bool interactingWithItem;

    Vector3 mousePos;
    FloatParameter focalLength;
    GameObject viewedItem;
    GameObject focusCamera;
    bool disablingFocus;
    bool enablingFocus;

    public static event Action<bool> ViewingItem;

    private void Awake()
    {
        focusCamera = GetComponent<Player>().focusCamera;
        focalLength = focusCamera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>().focalLength;
    }

    public void ToggleViewMode(GameObject item, bool b, bool interact = false)
    {
        ViewingItem.Invoke(b);
        viewingItem = b;
        interactingWithItem = interact;
        disablingFocus = !b;
        enablingFocus = b;

        if (b)
        {
            focusCamera.SetActive(b);
            viewedItem = item.gameObject;
            ChangeLayerToFocus();
        }

        else
        {
            // Zawsze po skończeniu oglądania, będziemy próbować ustawić layer itemu z powrotem na domyślny
            if (viewedItem != null && viewedItem.GetComponent<ItemWorld>() != null) viewedItem.GetComponent<ItemWorld>().ViewingEnded();
            viewedItem = null;
        }
    }

    void ChangeLayerToFocus()
    {
        viewedItem.layer = 9;
        foreach (Transform child in viewedItem.GetComponentsInChildren<Transform>())child.gameObject.layer = 9;
    }

    public void ViewItemFromInventory(GameObject item)
    {
        GameObject obj = CreateItemFromInventory(item);
        viewingFromInventory = true;
        GetComponent<PlayerActions>().ToggleInventoryUI(false);
        ToggleViewMode(obj, true);
    }

    GameObject CreateItemFromInventory(GameObject item)
    {
        GameObject obj = Instantiate(item);
        obj.transform.SetParent(inventoryViewTransform.parent);
        obj.transform.localPosition = inventoryViewTransform.localPosition;
        obj.transform.localEulerAngles = item.transform.localEulerAngles;
        return obj;
    }

    public void StopViewingItemFromInventory()
    {
        viewingFromInventory = false;
        Destroy(viewedItem);
        ToggleViewMode(null, false);
    }

    void Update()
    {
        if (viewingItem && !interactingWithItem)
        {
            InspectItem();
        }

        ManageFocus();
    }

    void InspectItem()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            var delta = Input.mousePosition - mousePos;
            mousePos = Input.mousePosition;
            viewedItem.transform.Rotate(transform.right, delta.y * rotationSpeed, Space.World);
            viewedItem.transform.Rotate(transform.up, -delta.x * rotationSpeed, Space.World);
        }
    }

    void ManageFocus()
    {
        if (disablingFocus)
        {
            if (focalLength.value <= 25)
            {
                focalLength.value = 25f;
                focusCamera.SetActive(false);
                disablingFocus = false;
            }

            else focalLength.value -= Time.deltaTime * focusSpeed;
        }

        if (enablingFocus)
        {
            if (focalLength.value >= 35)
            {
                focalLength.value = 35f;
                enablingFocus = false;
            }

            else focalLength.value += Time.deltaTime * focusSpeed;
        }
    }

    // disablingFocus = false - gdy nie oglądamy żadnego przedmiotu / gdy skończyliśmy oglądać przedmiot i focus już się wyłączył
    // gdy skończyliśmy oglądać przedmiot, ale zaczęliśmy szybko oglądać drugi
    public bool DisablingFocusEnded()
    {
        return !disablingFocus;
    }
}
