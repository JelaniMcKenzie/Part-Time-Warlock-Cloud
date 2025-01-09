using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoanShark_Anticipation : StateMachineBehaviour
{
    private Transform player;
    private Rigidbody2D rb;
    private Vector2 rushDirection;
    [SerializeField] private float pauseDuration = 5.0f;
    private float elapsedTime = 0f;
    public LoanShark loanSharkBoss;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        loanSharkBoss = animator.GetComponent<LoanShark>();


        Vector2 target = new Vector2(player.position.x, player.position.y);

        //lock the rush direction
        rushDirection = (target - rb.position).normalized;

        //store rush direction for rush state
        animator.SetFloat("RushDirectionX", rushDirection.x);
        animator.SetFloat("RushDirectionY", rushDirection.y);

        rb.velocity = Vector2.zero;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        loanSharkBoss.LookAtPlayer();
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= pauseDuration)
        {
            animator.SetTrigger("Rush");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("StartRush");
        elapsedTime = 0f;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
