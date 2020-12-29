using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Anna : MonoBehaviour
{
    public List<Transform> destinations;
    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void FindPlayer()
    {
        agent.SetDestination(destinations[0].position);
    }
}
