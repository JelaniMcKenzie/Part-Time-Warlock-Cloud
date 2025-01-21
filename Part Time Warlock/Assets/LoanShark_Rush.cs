using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoanShark_Rush : StateMachineBehaviour
{
    Transform player;
    Rigidbody2D rb;
    LoanShark loanSharkBoss;

    [SerializeField] private float rushSpeed = 20f;
    [SerializeField] private float rushDuration = 0.5f;
    [SerializeField] private float attackRange = 1f;
    Vector2 playerPos;

    private Vector2 rushDirection;
    private float elapsedTime;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        loanSharkBoss = animator.GetComponent<LoanShark>();

        attackRange = animator.GetFloat("attackRange");

        playerPos = new Vector2(player.position.x, player.position.y);

        // Initialize rush direction toward the player's current position
        rushDirection = (playerPos - rb.position).normalized;

        elapsedTime = 0f; // Reset elapsed time
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
        // Apply rush movement
        rb.velocity = rushDirection * rushSpeed;
        elapsedTime += Time.deltaTime;

        if (rb.velocity.magnitude > rushSpeed)
        {
            rb.velocity = rb.velocity.normalized * rushSpeed;
        }

        // Transition to "Swing" state if the enemy is within attack range or the rush duration ends
        if (Vector2.Distance(player.position, rb.position) <= attackRange || elapsedTime >= rushDuration)
        {
            animator.SetTrigger("Swing");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Rush");
        rb.velocity = Vector2.zero; // Stop the enemy's movement
    }
}
