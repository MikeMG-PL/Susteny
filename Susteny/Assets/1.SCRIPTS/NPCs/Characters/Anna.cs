using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.AI;

public class Anna : MonoBehaviour
{
    public List<Transform> destinations;
    NavMeshAgent agent;

    public PlayableDirector timeline2;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void FindPlayer()
    {
        agent.SetDestination(destinations[0].position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == destinations[0])
            timeline2.Play();
    }
}
