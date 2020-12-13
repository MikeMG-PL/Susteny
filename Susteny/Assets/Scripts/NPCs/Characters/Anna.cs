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

    }

    void OnTriggerStay(Collider other)
    {

    }

    void OnTriggerEnter(Collider other)
    {
        LookAtAnna(other);
    }

    void LookAtAnna(Collider c)
    {
        if (c.gameObject.GetComponent<Destination>() != null && c.gameObject.GetComponent<Destination>().ID == 0)
        {
            player.canLook = false;
            Camera.main.transform.LookAt(destinations[0]);
            GetComponent<DialogueInteraction>().Talk(true);
            Destroy(destinations[0].gameObject);
            //player.GetComponent<PlayerActions>().TakeToInventory(photo.GetComponent<ItemWorld>());
        }
    }
}
