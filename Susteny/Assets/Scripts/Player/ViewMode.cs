using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ViewMode : MonoBehaviour
{
    public float rotationSpeed = 100f;

    ItemWorld interactable;
    bool disablingFocus;
    bool enablingFocus;
    bool viewMode;
    float focusSpeed = 30f;

    public void ViewModeOn(ItemWorld item)
    {
        viewMode = true;
        disablingFocus = false;
        enablingFocus = true;
        GetComponent<SC_FPSController>().canMove = false;
        GetComponent<PlayerComponents>().focusCamera.SetActive(true);
        interactable = item;
    }

    public void ViewModeOff()
    {
        viewMode = false;
        disablingFocus = true;
        enablingFocus = false;
        GetComponent<SC_FPSController>().canMove = true;
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
