﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CharacterController))]

public class SC_FPSController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public bool isRunning;

    [HideInInspector] public bool finishedGoingAndRotatingTowardsObject = true;
    ViewMode viewMode;
    PlayerActions playerActions;
    UIHints UIHints;
    InputManager input;

    CharacterController characterController;
    public Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    float rotationY = 0;

    [HideInInspector]
    public bool canMove = true;
    public bool canLook = true;

    Transform cameraTransform;

    Vector3 posToLook;
    public bool lookingAt;
    float angleTolerance; // Look at don't need to be perfect
    float lookAtSpeed;

    Vector3 posToGo;
    public bool goingTo;
    float positionTolerance;
    float goingSpeed;

    void Awake()
    {
        Init();
        Subscribe();
        input = InputManager.instance;
    }

    void OnDisable()
    {
        Unsubscribe();
    }

    //////////////////////////////

    void Subscribe()
    {
        DialogueInteraction.Talking += LockControlsCursorOn;
        PlayerActions.BrowsingInventory += LockControlsCursorOn;
    }

    void Unsubscribe()
    {
        DialogueInteraction.Talking -= LockControlsCursorOn;
        PlayerActions.BrowsingInventory -= LockControlsCursorOn;
    }

    public void UnfreezeLook(bool b)
    {
        canLook = b;
    }

    public void LockControlsCursorOn(bool b)
    {
        canMove = !b;
        canLook = !b;
        Cursor.visible = b;

        if (b)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    public void LockControlsCursorOff(bool b)
    {
        canMove = !b;
        canLook = !b;
    }

    public void AllEnabled(bool b)
    {
        GetComponent<CharacterController>().enabled = b;
        GetComponent<PlayerActions>().DisallowInventorySwitching(!b);
    }

    public void QuittingViewModeAllowed(bool b)
    {
        GetComponent<PlayerActions>().quittingViewModeAllowed = b;
    }

    public void ToggleCursor(bool b)
    {
        Cursor.visible = b;

        if (b) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;
    }

    void Init()
    {
        viewMode = GetComponent<ViewMode>();
        playerActions = GetComponent<PlayerActions>();
        UIHints = viewMode.UIHints;
        characterController = GetComponent<CharacterController>();
        cameraTransform = playerCamera.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(PreventRotatingFPSController());

        rotationX += playerCamera.transform.localEulerAngles.x;
        rotationY += playerCamera.transform.localEulerAngles.y;
    }

    void Update()
    {
        CursorEnableAfterOnPosOrRot();

        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        Vector3 right = cameraTransform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        isRunning = input.GetKeybind(input.keybinds.run);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        if (characterController.isGrounded) moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (input.GetKeybindDown(input.keybinds.jump) && canMove && characterController.isGrounded)
            moveDirection.y = jumpSpeed;
        else
            moveDirection.y = movementDirectionY;

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        if (characterController.enabled) characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canLook)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            rotationY += Input.GetAxis("Mouse X") * lookSpeed;
            cameraTransform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
        }

        if (lookingAt) RotateToLookAt();
        if (goingTo) GoToPosition();
    }

    public void LookAt(Vector3 posToLook, float rotatingSpeed = 50f, float angleTolerance = 0.2f)
    {
        finishedGoingAndRotatingTowardsObject = false;
        lookingAt = true;
        canLook = false;
        this.angleTolerance = angleTolerance;
        this.lookAtSpeed = rotatingSpeed;
        this.posToLook = posToLook;
    }

    public void GoTo(Vector3 posToGo, float goingSpeed = 4f, float positionTolerance = 0.1f)
    {
        finishedGoingAndRotatingTowardsObject = false;
        canMove = false;
        goingTo = true;
        this.positionTolerance = positionTolerance;
        this.goingSpeed = goingSpeed;

        if (Physics.Raycast(posToGo, Vector3.down, out RaycastHit hit, 5f)) posToGo.y = hit.point.y;
        else Debug.LogWarning("No floor found near the player interact position");
        this.posToGo = posToGo;
    }

    // !!! Jeśli chcesz zatrzymać gracza i *od razu* nadać mu nowy cel, musisz użyć funkcji ForceStopGoingRotating
    // Używanie ForceStop jest ogólnie bezpieczniejsze.

    void StopGoingTo(bool enableMove = false)
    {
        goingTo = false;
        canMove = enableMove;
    }

    void StopLookingAt(bool enableLooking = false)
    {
        rotationX = EulerDistance(cameraTransform.localEulerAngles.x);
        rotationY = EulerDistance(cameraTransform.localEulerAngles.y);

        lookingAt = false;
        canLook = enableLooking;
    }

    void RotateToLookAt()
    {
        Quaternion lookAtQuaternion = Quaternion.LookRotation(posToLook - cameraTransform.position);

        // !goingTo bo gracz zawsze musi najpierw być w odpowiednim miejscu, aby kamera przestała się obracać
        if (!goingTo
            && cameraTransform.localEulerAngles.y <= lookAtQuaternion.eulerAngles.y + angleTolerance
            && cameraTransform.localEulerAngles.y >= lookAtQuaternion.eulerAngles.y - angleTolerance
            && cameraTransform.localEulerAngles.x <= lookAtQuaternion.eulerAngles.x + angleTolerance
            && cameraTransform.localEulerAngles.x >= lookAtQuaternion.eulerAngles.x - angleTolerance)
        {
            StopLookingAt();
        }

        cameraTransform.rotation = Quaternion.RotateTowards(cameraTransform.rotation, lookAtQuaternion, Time.deltaTime * lookAtSpeed);
        cameraTransform.rotation = Quaternion.Euler(new Vector3(cameraTransform.localEulerAngles.x, cameraTransform.localEulerAngles.y, 0));
    }

    void GoToPosition()
    {
        if (transform.position.x <= posToGo.x + positionTolerance && transform.position.x >= posToGo.x - positionTolerance
            && transform.position.z <= posToGo.z + positionTolerance && transform.position.z >= posToGo.z - positionTolerance
            && characterController.isGrounded)
        {
            StopGoingTo();
        }

        Vector3 positionToGo = posToGo - transform.position;
        if (positionToGo.magnitude > positionTolerance)
        {
            positionToGo = positionToGo.normalized * goingSpeed;
            characterController.Move(positionToGo * Time.deltaTime);
        }
    }

    public void ForceStopGoingRotating(bool enableMove = false, bool enableLooking = false)
    {
        finishedGoingAndRotatingTowardsObject = true;
        playerActions.showCursorOnPosition = false;

        goingTo = false;
        canMove = enableMove;

        rotationX = EulerDistance(cameraTransform.localEulerAngles.x);
        rotationY = EulerDistance(cameraTransform.localEulerAngles.y);

        lookingAt = false;
        canLook = enableLooking;
    }

    void CursorEnableAfterOnPosOrRot()
    {
        if (!finishedGoingAndRotatingTowardsObject && !lookingAt && !goingTo)
        {
            if (playerActions.showCursorOnPosition)
            {
                playerActions.showCursorOnPosition = false;
                if (viewMode.viewingItem)
                {
                    ToggleCursor(true);
                    if (viewMode.viewedItem.GetComponent<Hints>() != null) UIHints.ShowCornerHints(viewMode.viewedItem.GetComponent<Hints>().cornerHints);
                }
            }
            finishedGoingAndRotatingTowardsObject = true;
        }
    }

    float EulerDistance(float euler)
    {
        if (euler > 180) return euler - 360;
        else return euler;
    }

    IEnumerator PreventRotatingFPSController()
    {
        while (true)
        {
            transform.localEulerAngles = Vector3.zero;
            yield return new WaitForSecondsRealtime(1);
            transform.localEulerAngles = Vector3.zero;
        }
    }
}