using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class ViewMode : MonoBehaviour
{
    public Transform inventoryViewTransform;
    public float rotationSpeed = 100f;
    public float focusSpeed = 30f;
    public bool viewingItem;
    public bool viewingFromInventory;

    GameObject viewedItem;
    bool disablingFocus;
    bool enablingFocus;
    GameObject focusCamera;
    FloatParameter focalLength;

    public static event Action<bool> Viewing;

    private void Start()
    {
        focusCamera = GetComponent<Player>().focusCamera;
        focalLength = focusCamera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>().focalLength;
    }

    public void ToggleViewMode(GameObject item, bool b)
    {
        Viewing.Invoke(b);
        viewingItem = b;
        disablingFocus = !b;
        enablingFocus = b;
        GetComponent<SC_FPSController>().canMove = !b;
        GetComponent<SC_FPSController>().canLook = !b;
        if (b)
        {
            focusCamera.SetActive(b);
            viewedItem = item.gameObject;
        }
        else viewedItem = null;
    }

    public void ViewItemFromInventory(GameObject item)
    {
        GameObject obj = Instantiate(item);
        obj.transform.SetParent(inventoryViewTransform.parent);
        obj.transform.localPosition = inventoryViewTransform.localPosition;
        obj.transform.localRotation = inventoryViewTransform.localRotation;
        obj.transform.localScale = inventoryViewTransform.localScale;

        viewingFromInventory = true;
        GetComponent<PlayerActions>().ToggleInventoryUI(false, false);
        ToggleViewMode(obj, true);
    }

    public void StopViewingItemFromInventory()
    {
        viewingFromInventory = false;
        Destroy(viewedItem);
        ToggleViewMode(null, false);
    }

    void Update()
    {
        if (viewingItem)
        {
            viewedItem.transform.Rotate(Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime, Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime, 0, Space.World);
        }

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
}
