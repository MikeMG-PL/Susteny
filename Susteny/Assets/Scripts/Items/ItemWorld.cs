using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public Action action;
    GameObject player;
    public Item item;
    public float interactionDistance = 3.5f;
    public int amount = 1; 

    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public Quaternion startRotation;
    [HideInInspector] public bool grabbed;
    [HideInInspector] public bool grabbing;
    [HideInInspector] public bool ungrabbing;

    Player playerScript;
    float moveToViewModeRotSpeed = 5f;
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

            if (transform.position == startPosition && transform.rotation == startRotation) ungrabbing = false;
        }
    }

    private void OnMouseDown()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance > interactionDistance || !player.GetComponent<PlayerActions>().canGrab()) return;

        if (action == Action.grabbable && !grabbed)
        {
            player.GetComponent<PlayerActions>().canUngrab = false;
            player.GetComponent<PlayerActions>().Grab(this);
        }

        else if (action == Action.takeable) player.GetComponent<PlayerActions>().TakeToInventory(this);
    }

    public void SetAllCollidersStatus(bool enable)
    {
        foreach (Collider c in GetComponents<Collider>())
        {
            c.enabled = enable;
        }
    }
}

public enum Action
{
    grabbable,
    takeable,
};
