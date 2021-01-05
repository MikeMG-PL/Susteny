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
        {

            if (agent.remainingDistance <= 0.1f)
            {
                animator.SetFloat("Forward", 0);
            }
            else
                animator.SetFloat("Forward", agent.velocity.magnitude / 2);
        }
    }

    void Update()
    {
        AnimateCharacter();
    }
}
