using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Car : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject navigationPointsContainer;

    int point;      // Do którego punktu zmierzać będzie pojazd

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        navigationPointsContainer = GameObject.FindGameObjectWithTag("VehiclePoints");
        Move();
    }

    void Move()
    {
        agent.SetDestination(navigationPointsContainer.transform.GetChild(point).position);
    }

    void Update()
    {
        if (Vector3.Distance(agent.pathEndPosition, agent.transform.position) < 3)
        {
            if (point + 1 < navigationPointsContainer.transform.childCount)
            {
                point++;
                Move();
            }
        }
    }
}
