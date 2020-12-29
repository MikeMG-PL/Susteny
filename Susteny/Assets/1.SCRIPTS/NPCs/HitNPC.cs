using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HitNPC : MonoBehaviour
{
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.CompareTag("Player"))
        {
            GetComponent<CapsuleCollider>().radius = GetComponent<CapsuleCollider>().radius * 0.1f;
            agent.radius = agent.radius * 0.0001f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.CompareTag("Player"))
        {
            GetComponent<CapsuleCollider>().radius = GetComponent<CapsuleCollider>().radius / 0.1f;
            agent.radius = agent.radius / 0.0001f;
        }
    }

}
