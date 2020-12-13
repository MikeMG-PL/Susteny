using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Car : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject navigationPointsContainer, currentContainer;

    int pointID;      // Do którego punktu zmierzać będzie pojazd
    public int destinationType; // Gdzie zmierza pojazd docelowo

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        navigationPointsContainer = GameObject.FindGameObjectWithTag("VehiclePoints");

        agent.speed = Random.Range(15, 22.5f);
        agent.angularSpeed = agent.speed + 35;
        Move();
    }

    void Move()
    {
        currentContainer = navigationPointsContainer.transform.GetChild(destinationType).gameObject;
        agent.SetDestination(currentContainer.transform.GetChild(pointID).position);
    }

    void Update()
    {
        Move();

        if (Vector3.Distance(agent.pathEndPosition, agent.transform.position) < 3)
        {
            if (pointID < currentContainer.transform.childCount - 1)
            {
                pointID++;
                Move();
            }

            if (pointID == currentContainer.transform.childCount - 1)
                Destroy(gameObject);
        }
    }
}
