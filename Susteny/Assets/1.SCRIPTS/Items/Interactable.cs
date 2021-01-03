using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent interaction;
    public CrosshairColor crosshairColor;
    public float interactionDistance = 3.5f;
    public bool canInteractDespiteJumping = false;
    [Space(5)]
    public bool cursorOnWhenOnPosition = true;

    [HideInInspector] public GameObject player;
    [HideInInspector] public Player playerScript;
    [HideInInspector] public PlayerActions playerActions;
    [HideInInspector] public bool enableLookAt = false;
    [HideInInspector] public Transform objectToLookAt;
    [HideInInspector] public bool enableGoingTo = false;
    [HideInInspector] public Transform positionToGo;

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
        if (changeLayer && player.GetComponent<ViewMode>().DisablingFocusEnded())
        {
            ChangeLayerAfterViewing();
        }
    }

    void OnMouseDown()
    {
        TryToTriggerAction();
    }

    void TriggerAction()
    {
        if (interaction.GetPersistentEventCount() == 0 || interaction.GetPersistentTarget(0) == null) Debug.LogWarning("Action not specified");
        else
        {
            if (enableLookAt) playerActions.LookAt(objectToLookAt.position, interacting: true);
            if (enableGoingTo) playerActions.GoToPosition(positionToGo.position, interacting: true);
            if (cursorOnWhenOnPosition && (enableGoingTo || enableLookAt)) playerActions.showCursorOnPosition = true;
            interaction.Invoke();
        }
    }

    public void Interact()
    {
        player.GetComponent<PlayerActions>().FocusOnObject(gameObject, interact: true, !cursorOnWhenOnPosition);
    }

    /// <summary>
    /// Jeżeli gracz jest wystarczająco blisko, oraz nie jest zablokowane wykonywanie akcji, akcja zostanie wykonana.
    /// </summary>
    public void TryToTriggerAction()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance > interactionDistance && playerActions.canInteract) return;
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

[CustomEditor(typeof(Interactable))]
public class InteractableEditor : Editor
{
    bool setCustomPosition;
    bool setCustomRotation;

    public override void OnInspectorGUI()
    {
        Interactable interactable = (Interactable)target;
        base.OnInspectorGUI();

        EditorGUILayout.Space(5f);
        setCustomRotation = EditorGUILayout.Toggle("Set custom object to look at", setCustomRotation);
        if (setCustomRotation)
        {
            interactable.enableLookAt = true;
            EditorGUILayout.LabelField("Player will look at the center of this object during interaction");
            interactable.objectToLookAt = EditorGUILayout.ObjectField("Object transform", interactable.objectToLookAt, typeof(Transform), true) as Transform;

            EditorGUILayout.Space(5f);
        }
        else interactable.enableLookAt = false;

        EditorGUILayout.Space(5f);
        setCustomPosition = EditorGUILayout.Toggle("Set position the player will stand", setCustomPosition);
        if (setCustomPosition)
        {
            interactable.enableGoingTo = true;
            EditorGUILayout.LabelField("Player will stand at this position during interaction");
            interactable.positionToGo = EditorGUILayout.ObjectField("Object transform", interactable.positionToGo, typeof(Transform), true) as Transform;

            EditorGUILayout.Space(5f);
        }
        else interactable.enableGoingTo = false;
    }
}