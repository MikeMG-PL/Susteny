using System.Collections;
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
        Prototype.LevelStart += LockControlsCursorOff;
        Prototype.MouseLookUnfreeze += UnfreezeLook;
    }
    void Unsubscribe()
    {
        DialogueInteraction.Talking -= LockControlsCursorOn;
        PlayerActions.BrowsingInventory -= LockControlsCursorOn;
        Prototype.LevelStart -= LockControlsCursorOff;
        Prototype.MouseLookUnfreeze -= UnfreezeLook;
    }

    void UnfreezeLook(bool b)
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

    void Init()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = playerCamera.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        Vector3 right = cameraTransform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            moveDirection.y = jumpSpeed;
        else
            moveDirection.y = movementDirectionY;

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        if (characterController.enabled && canMove) characterController.Move(moveDirection * Time.deltaTime);

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

    public void LookAt(Vector3 posToLook, float rotatingSpeed, float angleTolerance)
    {
        lookingAt = true;
        canLook = false;
        this.angleTolerance = angleTolerance;
        this.lookAtSpeed = rotatingSpeed;
        this.posToLook = posToLook;
    }

    public void GoTo(Vector3 posToGo, float goingSpeed, float positionTolerance)
    {
        canMove = false;
        goingTo = true;
        this.positionTolerance = positionTolerance;
        this.goingSpeed = goingSpeed;

        if (Physics.Raycast(posToGo, Vector3.down, out RaycastHit hit, 5f)) posToGo.y = hit.point.y;
        else Debug.LogWarning("No floor found near the player interact position");
        this.posToGo = posToGo;
    }

    public void StopGoingTo(bool enableMove = false)
    {
        goingTo = false;
        canMove = enableMove;
    }

    public void StopLookingAt(bool enableLooking = false)
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

    float EulerDistance(float euler)
    {
        if (euler > 180) return euler - 360;
        else return euler;
    }
}