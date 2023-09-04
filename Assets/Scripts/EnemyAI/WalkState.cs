using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class WalkState : StateMachineBehaviour
{
    float timer = 0;
    List<Transform> wayPoints = new();
    private NavMeshAgent agent;
    private Transform player;
    float chaseRange = 10;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        Transform wayPointsObjects = GameObject.FindGameObjectWithTag("WayPoints").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        foreach (Transform wayPoint in wayPointsObjects)
            wayPoints.Add(wayPoint);

        agent = animator.GetComponent<NavMeshAgent>();
        agent.SetDestination(wayPoints[0].position);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);

        timer += Time.deltaTime;
        
        if (timer > 10)
            animator.SetBool("isPatrolling", false);

        float distance = Vector3.Distance(animator.transform.position, player.position);
        
        if (distance < chaseRange)
            animator.SetBool("isChasing", true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that processes and affects root motion
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that sets up animation IK (inverse kinematics)
    }
}