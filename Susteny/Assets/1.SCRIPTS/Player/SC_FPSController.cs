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

    [HideInInspector]
    public bool canMove = true;
    public bool canLook = true;

    Transform cameraTransform;

    Vector3 posToLook;
    public bool lookingAt;
    float range = 0.3f; // Look at don't need to be perfect
    float horizontalRotationSpeed;
    float verticalRotationSpeed;

    Vector3 posToGo;
    public bool goingTo;
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
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

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
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canLook)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        if (lookingAt) RotateToLookAt();
        if (goingTo) GoToPosition();
    }

    public void LookAt(Vector3 posToLook, float horizontalRotationSpeed, float verticalRotationSpeed)
    {
        lookingAt = true;
        canLook = false;
        this.horizontalRotationSpeed = horizontalRotationSpeed;
        this.verticalRotationSpeed = verticalRotationSpeed;
        this.posToLook = posToLook;
    }

    public void GoTo(Vector3 posToGo, float goingSpeed)
    {
        GetComponent<CharacterController>().enabled = false;
        canMove = false;
        goingTo = true;
        this.goingSpeed = goingSpeed;

        float playerHeight = 1.06477f; // Wyznaczone doświadczalnie. Nie jest to całkowita wysokość kapsułki, ani wysokość od kamery do ziemi.
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hit, 5f)) posToGo.y = hit.point.y + playerHeight;
        else posToGo.y = transform.position.y;
        this.posToGo = posToGo;
    }

    void RotateToLookAt()
    {
        Quaternion lookAtQuaternion = Quaternion.LookRotation(cameraTransform.position - posToLook);

        // Z jakiegoś powodu gracz obraca się w przeciwną stronę, czyli o 180.
        lookAtQuaternion *= Quaternion.Euler(0, 180, 0);

        // !goTo bo gracz zawsze musi najpierw być w odpowiednim miejscu, aby kamera przestała się obracać
        if (!goingTo
            && transform.localRotation.eulerAngles.y <= lookAtQuaternion.eulerAngles.y + range
            && transform.localRotation.eulerAngles.y >= lookAtQuaternion.eulerAngles.y - range
            && cameraTransform.localRotation.eulerAngles.x <= lookAtQuaternion.eulerAngles.x + range
            && cameraTransform.localRotation.eulerAngles.x >= lookAtQuaternion.eulerAngles.x - range)
        {
            // Ze względu na różnice w wartości eulerAngles w inspektorze
            // https://forum.unity.com/threads/solved-how-to-get-rotation-value-that-is-in-the-inspector.460310/
            if (cameraTransform.localEulerAngles.x > 180) rotationX = -360 + cameraTransform.localEulerAngles.x;
            else rotationX = cameraTransform.localEulerAngles.x;

            lookingAt = false;
        }

        cameraTransform.localRotation = Quaternion.RotateTowards(cameraTransform.localRotation, lookAtQuaternion, Time.deltaTime * verticalRotationSpeed);
        cameraTransform.localRotation = Quaternion.Euler(new Vector3(cameraTransform.localRotation.eulerAngles.x, 0f, 0f));

        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, lookAtQuaternion, Time.deltaTime * horizontalRotationSpeed);
        transform.localRotation = Quaternion.Euler(new Vector3(0f, transform.localRotation.eulerAngles.y, 0f));
    }

    void GoToPosition()
    {
        if (transform.position == posToGo)
        {
            goingTo = false;
            GetComponent<CharacterController>().enabled = true;
        }

        float step = goingSpeed * Time.deltaTime;
        Vector3 positionToGo = Vector3.MoveTowards(transform.position, posToGo, step);

        transform.position = positionToGo;
    }
}