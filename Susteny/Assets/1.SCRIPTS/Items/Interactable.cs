﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent interaction;
    public CrosshairColor crosshairColor;
    public float interactionDistance = 3.5f;
    public bool canInteractDespiteJumping = false;
    public string interactionHint;

    [HideInInspector] public bool isInteractedWith;
    [HideInInspector] public GameObject player;
    [HideInInspector] public Player playerScript;
    [HideInInspector] public PlayerActions playerActions;
    [HideInInspector] public bool enableLookAt = false;
    [HideInInspector] public Transform objectToLookAt;
    [HideInInspector] public bool enableGoTo = false;
    [HideInInspector] public Transform positionToGo;
    [HideInInspector] public bool cursorOnWhenOnPosition = true;
    [HideInInspector] public float lookSpeed = 50;
    [HideInInspector] public float moveSpeed = 4;
    [HideInInspector] public float distance = 1;

    bool changeLayer;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
        playerActions = player.GetComponent<PlayerActions>();
    }

    void Update()
    {
        // Sytuacja poniżej występuje gdy skończyliśmy oglądać ten przedmiot i focus już się wyłączył lub gdy zaczęliśmy oglądać inny przedmiot
        if (changeLayer && playerActions.viewMode.viewedItem != gameObject)
        {
            ChangeLayerAfterViewing();
        }
    }

    void TriggerAction()
    {
        if (interaction.GetPersistentEventCount() == 0 || interaction.GetPersistentTarget(0) == null) Debug.LogWarning("Action not specified");
        else
        {
            if (enableLookAt)
                if (objectToLookAt == null) playerActions.LookAt(transform.position, lookSpeed);
                else playerActions.LookAt(objectToLookAt.position, lookSpeed);

            if (enableGoTo)
                if (positionToGo == null) playerActions.GoToPosition(transform.position + transform.TransformDirection(Vector3.forward * distance), moveSpeed);
                else playerActions.GoToPosition(positionToGo.position * distance, moveSpeed);

            if (cursorOnWhenOnPosition && (enableGoTo || enableLookAt)) playerActions.showCursorOnPosition = true;
            StartedInteracting();
            interaction.Invoke();
        }
    }

    public void FocusOnInteractable()
    {
        player.GetComponent<PlayerActions>().FocusOnObject(gameObject, interact: true, !playerActions.showCursorOnPosition);
    }

    void StartedInteracting()
    {
        isInteractedWith = true;
        playerActions.interactingObject = gameObject;
    }

    public void StoppedInteracting()
    {
        isInteractedWith = false;
        playerActions.interactingObject = null;
    }

    /// <summary>
    /// Jeżeli gracz jest wystarczająco blisko, oraz nie jest zablokowane wykonywanie akcji, akcja zostanie wykonana.
    /// </summary>
    public void TryToTriggerAction()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance > interactionDistance || !playerActions.canInteract) return;
        if (!canInteractDespiteJumping && !player.GetComponent<CharacterController>().isGrounded) return; // Gracz nie może skakać podczas interakcji

        TriggerAction();
    }

    // Służy do włączania i wyłączania wszystkich colliderów obiektu
    public void SetAllCollidersStatus(bool enable)
    {
        foreach (Collider c in GetComponents<Collider>())
        {
            c.enabled = enable;
        }
    }

    public void ViewingEnded()
    {
        changeLayer = true;
    }

    // Jeżeli obiekt zostanie zniszczony (wzięty do ekwipunku etc.) layer może nie zostać zmieniony na czas
    void ChangeLayerAfterViewing()
    {
        gameObject.layer = 0;
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>()) child.gameObject.layer = 0;
        changeLayer = false;
    }
}