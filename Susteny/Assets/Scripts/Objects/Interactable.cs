using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Interactable : MonoBehaviour
{
    public GameObject player;
    public float interactionDistance = 2f;
    public bool grabbable;
    public float RotationSpeed = 5;

    bool viewMode;
    bool disablingFocus;
    bool enablingFocus;
    bool grabbed;

    float focusSpeed = 20f;

    void Update()
    {
        if (viewMode)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.GetComponent<PlayerComponents>().viewObject.transform.position, 0.03f);
            transform.Rotate(Input.GetAxis("Mouse Y") * RotationSpeed * Time.deltaTime, Input.GetAxis("Mouse X") * RotationSpeed * Time.deltaTime, 0, Space.World);

            if (Input.GetKey(KeyCode.Mouse1)) Ungrab();
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

    void ViewModeOn()
    {
        viewMode = true;
        disablingFocus = false;
        enablingFocus = true;
        player.GetComponent<SC_FPSController>().canMove = false;
        player.GetComponent<PlayerComponents>().focusCamera.SetActive(true);
    }

    void ViewModeOff()
    {
        viewMode = false;
        disablingFocus = true;
        enablingFocus = false;
        player.GetComponent<SC_FPSController>().canMove = true;
    }

    private void OnMouseDown()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (grabbable && distance <= interactionDistance && !grabbed)
        {
            Grab();
        }
    }

    void Grab()
    {
        grabbed = true;
        ViewModeOn();
    }

    void Ungrab()
    {
        grabbed = false;
        ViewModeOff();
    }
}
