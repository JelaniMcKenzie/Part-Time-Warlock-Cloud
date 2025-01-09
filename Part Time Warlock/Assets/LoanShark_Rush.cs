using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoanShark_Rush : StateMachineBehaviour
{
    Transform player;
    Rigidbody2D rb;
    LoanShark loanSharkBoss;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private Vector2 rushDirection;

    [SerializeField] private float rushSpeed = 10f;
    [SerializeField] private float rushDistance = 5f;
    [SerializeField] private float attackRange = 1f;

    private bool hasReachedEndPoint = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        loanSharkBoss = animator.GetComponent<LoanShark>();

        attackRange = animator.GetFloat("attackRange");

        // Determine the dash direction
        rushDirection = (new Vector2(player.position.x, player.position.y) - rb.position).normalized;

        // Calculate the start and end positions for the dash
        startPosition = rb.position;
        endPosition = startPosition + rushDirection * rushDistance;

        hasReachedEndPoint = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Move the enemy toward the end position
        if (!hasReachedEndPoint)
        {
            rb.velocity = rushDirection * rushSpeed;

            // Check if the enemy has reached the end position
            if (Vector2.Distance(rb.position, endPosition) <= 0.1f)
            {
                hasReachedEndPoint = true;
            }
        }

        // Transition to "Swing" state if within attack range or has reached the end position
        if (Vector2.Distance(player.position, rb.position) <= attackRange || hasReachedEndPoint)
        {
            animator.SetTrigger("Swing");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Rush");
        rb.velocity = Vector2.zero;
    }
}
