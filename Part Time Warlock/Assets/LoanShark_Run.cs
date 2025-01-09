using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoanShark_Run : StateMachineBehaviour
{
    public float moveSpeed = 2.5f;
    public float attackRange = 2f;
    public float rushRange = 5f;
    Transform player;
    Rigidbody2D rb;
    LoanShark loanSharkBoss;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        if (loanSharkBoss == null)
        {
            loanSharkBoss = animator.GetComponent<LoanShark>();
        }
        animator.SetFloat("attackRange", attackRange);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        loanSharkBoss.LookAtPlayer();

        Vector2 target = new Vector2(player.position.x, player.position.y);
        // Calculate the direction towards the player
        Vector2 direction = (target - rb.position).normalized;

        // Move the enemy towards the player
        rb.velocity = moveSpeed * direction;

        // Check if within attack range
        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            animator.SetTrigger("Slam");
        }

        if (Vector2.Distance(player.position, rb.position) <= rushRange)
        {
            // Decide whether to Slam or Start Rush
            float randomChance = Random.Range(0f, 1f); // Random chance between 0 and 1
            if (randomChance <= 0.2f) // 20% chance to rush
            {
                animator.SetTrigger("StartRush");
            }
        }
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Slam");
        rb.velocity = Vector2.zero;
        animator.ResetTrigger("LoanShark");
    }

    
}
