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

    [HideInInspector] public bool viewingItem;
    [HideInInspector] public bool viewingFromInventory;

    public Shader viewShader;
    public Shader[] shadersCopyX; Shader parentShaderX;
    public MeshRenderer[] renderers; MeshRenderer parentRenderer;

    Vector3 mousePos;
    FloatParameter focalLength;
    GameObject viewedItem;
    GameObject lastItem;
    GameObject focusCamera;
    bool disablingFocus;
    bool enablingFocus;

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
        if (b)
        {
            focusCamera.SetActive(b);
            viewedItem = item.gameObject;

            parentRenderer = viewedItem.GetComponent<MeshRenderer>();
            renderers = viewedItem.GetComponentsInChildren<MeshRenderer>();

            Shader parentShader = parentRenderer.material.shader;
            Shader[] shadersCopy = new Shader[renderers.Length];

            for (int i = 0; i < renderers.Length; i++)
            {
                shadersCopy[i] = renderers[i].material.shader;
            }

            parentRenderer.material.shader = viewShader;
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.shader = viewShader;
            }
            lastItem = null;

            parentShaderX = parentShader;
            shadersCopyX = shadersCopy;
        }
        else
        {
            lastItem = viewedItem;
            viewedItem = null;
        }
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
        if (lastItem != null && lastItem.GetComponent<ItemWorld>() != null && lastItem.GetComponent<ItemWorld>().ungrabbing == false)
        {
            lastItem.GetComponent<MeshRenderer>().material.shader = parentShaderX;

            for (int i = 0; i < lastItem.transform.childCount; i++)
            {
                lastItem.transform.GetChild(i).GetComponent<MeshRenderer>().material.shader = shadersCopyX[i];
            }
        }

        if (viewingItem)
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
