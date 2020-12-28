using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePadlockWheels : MonoBehaviour
{
    public Transform correctPos;
    public float angleTolerance = 10f;
    public float rotationSpeed = 1f;

    [HideInInspector] public bool correct;

    Vector3 mousePos;
    bool wasClickedOn;

    void OnMouseDown()
    {
        wasClickedOn = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0)) wasClickedOn = false;

        if (wasClickedOn && Input.GetMouseButtonDown(0))
        {
            mousePos = Input.mousePosition;
        }

        if (wasClickedOn && Input.GetMouseButton(0))
        {
            var delta = Input.mousePosition - mousePos;
            mousePos = Input.mousePosition;
            //transform.Rotate(transform.right, delta.y * rotationSpeed, Space.World);
            transform.Rotate(transform.up, -delta.x * rotationSpeed, Space.World);
        }

        if (Quaternion.Angle(transform.localRotation, correctPos.localRotation) <= angleTolerance) correct = true;
        else correct = false;
    }
}
