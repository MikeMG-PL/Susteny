using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent interaction;
    public CrosshairColor crosshairColor;
    public float interactionDistance = 3.5f;
    public bool canInteractDespiteJumping = false;

    [HideInInspector] public GameObject player;
    [HideInInspector] public Player playerScript;
    [HideInInspector] public PlayerActions playerActions;
    [HideInInspector] public bool enableLookAt = false;
    [HideInInspector] public Transform objectToLookAt;
    [HideInInspector] public bool enableGoingTo = false;
    [HideInInspector] public Transform positionToGo;
    [HideInInspector] public bool cursorOnWhenOnPosition = true;

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
            if (enableLookAt)
                if (objectToLookAt == null) playerActions.LookAt(transform.position);
                else playerActions.LookAt(objectToLookAt.position);

            if (enableGoingTo)
                if (positionToGo == null) playerActions.GoToPosition(transform.position + transform.TransformDirection(Vector3.forward));
                else playerActions.GoToPosition(positionToGo.position);

            if (cursorOnWhenOnPosition && (enableGoingTo || enableLookAt)) playerActions.showCursorOnPosition = true;
            interaction.Invoke();
        }
    }

    public void Interact()
    {
        player.GetComponent<PlayerActions>().FocusOnObject(gameObject, interact: true, !playerActions.showCursorOnPosition);
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

[CustomEditor(typeof(Interactable))]
public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Interactable interactable = (Interactable)target;
        base.OnInspectorGUI();

        EditorGUILayout.Space(5f);

        if (interactable.enableGoingTo || interactable.enableLookAt)
        {
            interactable.cursorOnWhenOnPosition = EditorGUILayout.Toggle("Cursor on when player on position", interactable.cursorOnWhenOnPosition);
            EditorGUILayout.Space(5f);
        }
        else interactable.cursorOnWhenOnPosition = false;

        interactable.enableLookAt = EditorGUILayout.Toggle("Set custom object to look at", interactable.enableLookAt);
        if (interactable.enableLookAt)
        {
            interactable.objectToLookAt = (Transform)EditorGUILayout.ObjectField(
                new GUIContent("Transform", "Player will look at the center of this object during interaction"),
                interactable.objectToLookAt, typeof(Transform), true);

            EditorGUILayout.Space(5f);
        }

        EditorGUILayout.Space(5f);
        interactable.enableGoingTo = EditorGUILayout.Toggle("Set position the player will stand", interactable.enableGoingTo);
        if (interactable.enableGoingTo)
        {
            interactable.positionToGo = (Transform)EditorGUILayout.ObjectField(
                new GUIContent("Transform", "Player will stand at this position during interaction"),
                interactable.positionToGo, typeof(Transform), true);

            EditorGUILayout.Space(5f);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(interactable);
            EditorSceneManager.MarkSceneDirty(interactable.gameObject.scene);
        }
    }
}