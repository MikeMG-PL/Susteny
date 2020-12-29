using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkAround : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject navigationPointsContainer;
    Animator animator;

    int point;      // Do którego punktu zmierzać będzie NPC
    float time;     // Ile ma trwać jego podróż, zanim ją przerwie i zmieni punkt
    float speed;    // Szybkość chodu NPC;
    /////////////////////////////////////////////////////////////////////////////
    float timer;
    int ID;

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        navigationPointsContainer = GameObject.FindGameObjectWithTag("NavigationPoints");

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i) == transform)
                ID = i;
        }

        Destination();
    }

    void Destination()
    {
        int newPoint = Random.Range(0, navigationPointsContainer.transform.childCount);
        while (newPoint == point)
        {
            newPoint = Random.Range(0, navigationPointsContainer.transform.childCount);
        }
        point = newPoint;

        time = Random.Range(30, 60);
        speed = Random.Range(0.75f, 2);

        if (point + ID < navigationPointsContainer.transform.childCount)
            point += ID;
        else if (point + ID / 2 < navigationPointsContainer.transform.childCount)
            point += ID / 2;
        else if (point + ID - 3 < navigationPointsContainer.transform.childCount && point + ID - 3 > 0)
            point += ID - 3;
        else if (point + ID + 2 < navigationPointsContainer.transform.childCount)
            point += ID + 2;

        Move();
    }

    void Move()
    {
        agent.SetDestination(navigationPointsContainer.transform.GetChild(point).position);
        agent.speed = speed;
        timer = 0;

    }



    void Animate()
    {
        animator.SetFloat("Forward", agent.speed / 2);

        /*if (agent.angularSpeed < 0)
            animator.SetFloat("Turn", -agent.angularSpeed);
        else
            animator.SetFloat("Turn", agent.angularSpeed);*/
    }

    void Update()
    {
        timer += Time.deltaTime;

        Animate();

        if (Vector3.Distance(agent.destination, agent.transform.position) < 4 || timer >= time)
            Destination();
    }
}
