﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animate : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;
    WalkAround walkAround;
    int forwardID;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        walkAround = GetComponent<WalkAround>();
        forwardID = Animator.StringToHash("Forward");
    }

    void AnimateCharacter()
    {
        if (walkAround != null && animator.GetFloat(forwardID) != agent.speed / 2)
            animator.SetFloat(forwardID, agent.speed / 2);

        else if (animator.GetFloat(forwardID) != agent.velocity.magnitude / 2)
            animator.SetFloat(forwardID, agent.velocity.magnitude / 2);
    }

    void ReachDestination()
    {
        if (walkAround == null)
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

    public enum Side { Left, Right };
    public void RotateCharacter(Side side, float turnSpeed, float range) // Side - left/right, turnSpeed - turning animation speed, range - how far the agent rotates
    {
        StartCoroutine(RotationCoroutine(side, turnSpeed, range));
    }

    IEnumerator RotationCoroutine(Side side, float turnSpeed, float range)
    {
        float turn = 0;
        while (turn < range)
        {
            turn += Time.deltaTime * 2 * turnSpeed;

            yield return new WaitForSeconds(Time.deltaTime);

            if(side == Side.Left)
                animator.SetFloat("Turn", -turn);
            else
                animator.SetFloat("Turn", turn);
        }
        while (turn > 0)
        {
            turn -= Time.deltaTime * 2 * turnSpeed;

            yield return new WaitForSeconds(Time.deltaTime);

            if (side == Side.Left)
                animator.SetFloat("Turn", -turn);
            else
                animator.SetFloat("Turn", turn);
        }
        animator.SetFloat("Turn", 0);
        StopCoroutine(RotationCoroutine(side, turnSpeed, range));
    }

    void Update()
    {
        AnimateCharacter();
        ReachDestination();
    }
}
