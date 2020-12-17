using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public MonoBehaviour test;

    public Action action;
    public GameObject player;
    public Item item;
    public float interactionDistance = 3.5f;
    public int amount = 1; 

    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public Quaternion startRotation;
    [HideInInspector] public bool grabbed;
    [HideInInspector] public bool grabbing;
    [HideInInspector] public bool ungrabbing;

    Player playerScript;
    bool changeLayer;
    float moveToViewModeRotSpeed = 3.5f;
    float moveToViewModePosSpeed = 0.05f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
    }

    void Start()
    {
        // Pozycja do której wracać będą przedmioty, które można podnieść, obejrzeć i z powrotem odstawić
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        if (grabbing)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, playerScript.viewModePosition.transform.position, moveToViewModePosSpeed);

            if (transform.position == playerScript.viewModePosition.transform.position) grabbing = false;
        }

        else if (ungrabbing)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, startRotation, moveToViewModeRotSpeed);
            transform.position = Vector3.MoveTowards(transform.position, startPosition, moveToViewModePosSpeed);

            if (transform.position == startPosition && transform.rotation == startRotation)
            {
                ungrabbing = false;
            }
        }

        // Sytuacja poniżej występuje gdy skończyliśmy oglądać ten przedmiot i focus już się wyłączył lub gdy zaczęliśmy oglądać inny przedmiot
        if (changeLayer && player.GetComponent<ViewMode>().DisablingFocusEnded())
        {
            ChangeLayerAfterViewing();
        }
    }

    private void OnMouseDown()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance > interactionDistance) return;

        TriggerAction();
    }

    public void TriggerAction()
    {
        if (action == Action.door) GetComponent<AnyDoor>().OpenDoor();

        if (action == Action.interactable)
        {
            player.GetComponent<PlayerActions>().Interact(gameObject);
        }

        if (!player.GetComponent<PlayerActions>().canGrab) return;

        if (action == Action.grabbable && !grabbed) player.GetComponent<PlayerActions>().Grab(this);

        else if (action == Action.takeable) player.GetComponent<PlayerActions>().TakeToInventory(this);
    }

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

public enum Action
{
    grabbable,
    takeable,
    interactable,
    door,
};
