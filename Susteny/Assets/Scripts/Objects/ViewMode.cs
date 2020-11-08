using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ViewMode : MonoBehaviour
{
    public float rotationSpeed = 100f;

    GameObject player;
    Interactable interactable;
    bool disablingFocus;
    bool enablingFocus;
    bool viewMode;
    float focusSpeed = 20f;

    void Start()
    {
        interactable = GetComponent<Interactable>();
        player = interactable.player;
    }

    public void ViewModeOn()
    {
        viewMode = true;
        disablingFocus = false;
        enablingFocus = true;
        player.GetComponent<SC_FPSController>().canMove = false;
        player.GetComponent<PlayerComponents>().focusCamera.SetActive(true);
    }

    public void ViewModeOff()
    {
        viewMode = false;
        disablingFocus = true;
        enablingFocus = false;
        player.GetComponent<SC_FPSController>().canMove = true;
    }

    void Update()
    {
        if (viewMode)
        {
            transform.Rotate(Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime, Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime, 0, Space.World);
        }

        if (disablingFocus)
        {
            FloatParameter focalLength = player.GetComponent<PlayerComponents>().focusCamera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>().focalLength;
            if (focalLength.value <= 25)
            {
                disablingFocus = false;
                focalLength.value = 25f;
                player.GetComponent<PlayerComponents>().focusCamera.SetActive(false);
            }

            else focalLength.value -= Time.deltaTime * focusSpeed;
        }

        if (enablingFocus)
        {
            FloatParameter focalLength = player.GetComponent<PlayerComponents>().focusCamera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>().focalLength;
            if (focalLength.value >= 35)
            {
                enablingFocus = false;
                focalLength.value = 35f;
            }

            else focalLength.value += Time.deltaTime * focusSpeed;
        }
    }
}
