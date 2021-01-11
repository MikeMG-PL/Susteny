using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animate : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void AnimateCharacter()
    {
        if (GetComponent<WalkAround>() != null)
            animator.SetFloat("Forward", agent.speed / 2);
        else
            animator.SetFloat("Forward", agent.velocity.magnitude / 2);
    }

    void ReachDestination()
    {
        if (GetComponent<WalkAround>() == null)
        {
            if ((agent.destination.magnitude - transform.position.magnitude) <= 0.2f)
                StopAgent(true);
            else
                StopAgent(false);
        }
    }

    void StopAgent(bool b)
    {
        agent.isStopped = b;

        if (b)
        {
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            animator.applyRootMotion = false;
            if(agent.velocity.magnitude <= 0.01f)
                animator.applyRootMotion = true;
        }
        else
        {
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            animator.applyRootMotion = true;
        }
            
    }

    void Update()
    {
        AnimateCharacter();
        ReachDestination();
    }
}
