using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class ViewMode : MonoBehaviour
{
    public float rotationSpeed = 100f;

    ItemWorld interactable;
    bool disablingFocus;
    bool enablingFocus;
    bool viewMode;
    public float focusSpeed = 30f;
    GameObject focusCamera;
    FloatParameter focalLength;

    public static event Action<bool> Viewing;

    private void Start()
    {
        focusCamera = GetComponent<Player>().focusCamera;
        focalLength = focusCamera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>().focalLength;
    }

    public void ToggleViewMode(ItemWorld item, bool b)
    {
        Viewing.Invoke(b);
        viewMode = b;
        disablingFocus = !b;
        enablingFocus = b;
        GetComponent<SC_FPSController>().canMove = !b;
        GetComponent<SC_FPSController>().canLook = !b;
        if (b)
        {
            focusCamera.SetActive(b);
            interactable = item;
        }
        else interactable = null;
    }

    void Update()
    {
        if (viewMode)
        {
            interactable.transform.Rotate(Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime, Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime, 0, Space.World);
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
