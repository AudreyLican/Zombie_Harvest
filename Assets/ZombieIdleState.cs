using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This code will give all the logic needed to the Zombie to take action
 * Make transition between chase, patroling and idle state
 */
public class ZombieIdleState : StateMachineBehaviour
{
    float timer;
    public float idleTime = 0f;

    Transform player;

    // Detecte player
    public float detectionArearadius = 18f;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        // --- Transition to Patrol state --- //

        timer += Time.deltaTime;
        if (timer > idleTime)
        {
            animator.SetBool("isPatroling", true);
        }

        // --- Transition to Chase State --- //
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionArearadius)
        {
            animator.SetBool("isChasing", true);
        }
    }
}
