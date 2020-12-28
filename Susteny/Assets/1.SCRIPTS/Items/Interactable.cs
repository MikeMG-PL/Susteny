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

    [HideInInspector] public GameObject player;
    [HideInInspector] public Player playerScript;
    [HideInInspector] public PlayerActions playerActions;

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
        else interaction.Invoke();
    }

    public void Interact()
    {
        player.GetComponent<PlayerActions>().WatchObject(gameObject);
    }

    /// <summary>
    /// Jeżeli gracz jest wystarczająco blisko, oraz nie jest zablokowane wykonywanie akcji, akcja zostanie wykonana.
    /// </summary>
    public void TryToTriggerAction()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance > interactionDistance && playerActions.canInteract) return;

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
