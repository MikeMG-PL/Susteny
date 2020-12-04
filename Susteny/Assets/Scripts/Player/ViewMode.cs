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
    float focusSpeed = 30f;

    public static event Action<bool> Viewing;

    public void ToggleViewMode(ItemWorld item, bool b)
    {
        Viewing.Invoke(b);
        viewMode = b;
        disablingFocus = !b;
        enablingFocus = b;
        GetComponent<SC_FPSController>().canMove = !b;
        GetComponent<SC_FPSController>().canLook = !b;
        GetComponent<PlayerComponents>().focusCamera.SetActive(b);
        if (b)
            interactable = item;
        else
            interactable = null;
    }

    void Update()
    {
        if (viewMode)
        {
            interactable.transform.Rotate(Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime, Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime, 0, Space.World);
        }

        if (disablingFocus)
        {
            FloatParameter focalLength = GetComponent<PlayerComponents>().focusCamera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>().focalLength;
            if (focalLength.value <= 25)
            {
                disablingFocus = false;
                focalLength.value = 25f;
                GetComponent<PlayerComponents>().focusCamera.SetActive(false);
            }

            else focalLength.value -= Time.deltaTime * focusSpeed;
        }

        if (enablingFocus)
        {
            FloatParameter focalLength = GetComponent<PlayerComponents>().focusCamera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>().focalLength;
            if (focalLength.value >= 35)
            {
                enablingFocus = false;
                focalLength.value = 35f;
            }

            else focalLength.value += Time.deltaTime * focusSpeed;
        }
    }
}
