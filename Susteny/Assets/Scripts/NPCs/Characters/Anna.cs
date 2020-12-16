using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Anna : MonoBehaviour
{
    public GameObject photo;
    SC_FPSController player;
    GameManager.Level Level;
    NavMeshAgent agent;
    public Transform[] destinations;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<SC_FPSController>();
        Level = GameManager.Level.Prototype;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destinations[0].position);
        DialogueInteraction.Conversation += ConversationEvent;
    }

    void OnTriggerStay(Collider other)
    {

    }

    void OnTriggerEnter(Collider other)
    {
        LookAtAnna(other);
    }

    void ConversationEvent(bool b, string n, int i)
    {
        if(b == false && n == "Anna" && i == 0)
        {
            player.canLook = true;
            var p = Instantiate(photo);
            p.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            player.GetComponent<PlayerActions>().TakeToInventory(p.GetComponent<ItemWorld>());
            player.GetComponent<PlayerActions>().GrabFromInventory(player.GetComponent<Inventory>().GetInventory()[0].item.model); // here
        }
    }

    void LookAtAnna(Collider c)
    {
        if (c.gameObject.GetComponent<Destination>() != null && c.gameObject.GetComponent<Destination>().ID == 0)
        {
            player.canLook = false;
            Camera.main.transform.LookAt(destinations[0]);
            GetComponent<DialogueInteraction>().Talk(true);
            Destroy(destinations[0].gameObject);
        }
    }

    void OnDestroy()
    {
        DialogueInteraction.Conversation -= ConversationEvent;
    }
}
