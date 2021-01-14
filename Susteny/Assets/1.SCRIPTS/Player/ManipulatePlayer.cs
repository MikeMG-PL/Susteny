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

    private void Awake()
    {
        playerActions = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActions>();
    }

    public void Manipulate()
    {
        if (enableLookAt)
            if (objectToLookAt == null) playerActions.LookAt(transform.position, lookSpeed);
            else playerActions.LookAt(objectToLookAt.position, lookSpeed);

        if (enableGoTo)
            if (positionToGo == null) playerActions.GoToPosition(transform.position + transform.TransformDirection(Vector3.forward * distance), moveSpeed);
            else playerActions.GoToPosition(positionToGo.position * distance, moveSpeed);

        if (cursorOnWhenOnPosition && (enableGoTo || enableLookAt)) playerActions.showCursorOnPosition = true;
    }

    public void StopManipulating()
    {
        playerActions.GetComponent<SC_FPSController>().StopGoingTo();
        playerActions.GetComponent<SC_FPSController>().StopLookingAt();
        playerActions.GetComponent<SC_FPSController>().LockControlsCursorOn(false);
    }
}
