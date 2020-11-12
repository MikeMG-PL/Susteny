using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Interactable : MonoBehaviour
{
    public GameObject player;
    public bool grabbable;
    public float interactionDistance = 2f;

    Vector3 startPosition;
    Quaternion startRotation;
    bool grabbed;
    bool grabbing;
    bool ungrabbing;

    private void Update()
    {
        if (grabbing)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.GetComponent<PlayerComponents>().viewObject.transform.position, 0.03f);
            if (transform.position == player.GetComponent<PlayerComponents>().viewObject.transform.position) grabbing = false;
        }

        else if (ungrabbing)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, startRotation, 0.05f);
            transform.position = Vector3.MoveTowards(transform.position, startPosition, 0.03f);

            if (transform.position == startPosition && transform.rotation == startRotation) ungrabbing = false;
        }

        if (Input.GetKey(KeyCode.Mouse1) && grabbed) Ungrab();
    }

    private void OnMouseDown()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (grabbable && distance <= interactionDistance && !grabbed)
        {
            Grab();
        }
    }

    void Grab()
    {
        grabbed = true;
        grabbing = true;
        ungrabbing = false;
        GetComponent<ViewMode>().ViewModeOn();
        SetAllCollidersStatus(false);
    }

    void Ungrab()
    {
        grabbed = false;
        grabbing = false;
        ungrabbing = true;
        GetComponent<ViewMode>().ViewModeOff();
        SetAllCollidersStatus(true);
    }

    void SetAllCollidersStatus(bool enable)
    {
        foreach (Collider c in GetComponents<Collider>())
        {
            c.enabled = enable;
        }
    }
}
