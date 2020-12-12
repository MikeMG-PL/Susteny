using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkAround : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject navigationPointsContainer;

    int point;      // Do którego punktu zmierzać będzie NPC
    float time;     // Ile ma trwać jego podróż, zanim ją przerwie i zmieni punkt
    float speed;    // Szybkość chodu NPC;
    int priority; // Priorytet omijania przeszkód wzgl. innych NPC
    /////////////////////////////////////////////////////////////////////////
    float timer;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        navigationPointsContainer = GameObject.FindGameObjectWithTag("NavigationPoints");
        Destination();
    }

    void Destination()
    {
        point = Random.Range(0, navigationPointsContainer.transform.childCount);
        time = Random.Range(30, 60);
        speed = Random.Range(3, 4);
        priority = Random.Range(0, 99);
        Move();
    }

    void Move()
    {
        agent.SetDestination(navigationPointsContainer.transform.GetChild(point).position);
        agent.speed = speed;
        agent.avoidancePriority = priority;
        timer = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<CapsuleCollider>().radius = GetComponent<CapsuleCollider>().radius * 0.1f;
            agent.radius = agent.radius * 0.0001f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<CapsuleCollider>().radius = GetComponent<CapsuleCollider>().radius / 0.1f;
            agent.radius = agent.radius / 0.0001f;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (Vector3.Distance(agent.pathEndPosition, agent.transform.position) < 4 || timer >= time)
            Destination();
    }
}
