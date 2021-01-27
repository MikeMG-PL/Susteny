using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulatePlayer : MonoBehaviour
{
    [HideInInspector] public bool enableLookAt = false;
    [HideInInspector] public Transform objectToLookAt;
    [HideInInspector] public bool enableGoTo = false;
    [HideInInspector] public Transform positionToGo;
    [HideInInspector] public float lookSpeed = 50;
    [HideInInspector] public float moveSpeed = 4;
    [HideInInspector] public float distance = 1;
    [HideInInspector] public bool cursorOnWhenOnPosition = true;

    PlayerActions playerActions;
    SC_FPSController fpsController;

    private void Awake()
    {
        fpsController = GameObject.FindGameObjectWithTag("Player").GetComponent<SC_FPSController>();
        playerActions = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActions>();
    }

    public void Manipulate()
    {
        if (enableLookAt)
            if (objectToLookAt == null) fpsController.LookAt(transform.position, lookSpeed);
            else fpsController.LookAt(objectToLookAt.position, lookSpeed);

        if (enableGoTo)
            if (positionToGo == null) fpsController.GoTo(transform.position + transform.TransformDirection(Vector3.forward * distance), moveSpeed);
            else fpsController.GoTo(positionToGo.position * distance, moveSpeed);

        if (cursorOnWhenOnPosition && (enableGoTo || enableLookAt)) playerActions.showCursorOnPosition = true;
    }

    public void StopManipulating()
    {
        fpsController.ForceStopGoingRotating();
        fpsController.LockControlsCursorOn(false);
    }
}
