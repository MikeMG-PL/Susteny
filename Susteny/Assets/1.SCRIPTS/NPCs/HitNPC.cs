using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HitNPC : MonoBehaviour
{
    NavMeshAgent agent;
    Vector3 destination;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void OnTriggerEnter(Collider other)
    {
        destination = agent.destination;
        if (other.CompareTag("Player"))
        {
            GetComponent<CapsuleCollider>().radius = GetComponent<CapsuleCollider>().radius * 0.1f;
            agent.radius = agent.radius * 0.0001f;
        }
        agent.destination = destination;
    }

    void OnTriggerExit(Collider other)
    {
        destination = agent.destination;
        if (other.CompareTag("Player"))
        {
            GetComponent<CapsuleCollider>().radius = GetComponent<CapsuleCollider>().radius / 0.1f;
            agent.radius = agent.radius / 0.0001f;
        }
        agent.destination = destination;
    }
}
