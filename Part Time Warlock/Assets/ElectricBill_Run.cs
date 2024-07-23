using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBill_Run : StateMachineBehaviour
{
    WizardPlayer player;
    Rigidbody2D rb;
    ElectricBill eBill;

    public float attackRange = 3f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = FindAnyObjectByType<WizardPlayer>();
        rb = animator.GetComponent<Rigidbody2D>();
        eBill = animator.GetComponent<ElectricBill>();

        //move towards player. have another state that teleports it back to the center and does the slam
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        eBill.LookAtPlayer();

        //if the boss isn't moving or slamming, it just stands still
        if (eBill.canMove == true)
        {
            Vector2 target = new Vector2(player.transform.position.x, player.transform.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, eBill.moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
        else if (eBill.isSlamming == true)
        {
            animator.SetTrigger("Slam");
        }

        //^the above implementation just pushes the transform. be wary of this if the player gets pushed into walls

        if (Vector2.Distance(player.transform.position, rb.position) <= attackRange)
        {
            // Attack
            animator.SetTrigger("Punch");
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Punch");
        animator.ResetTrigger("Slam");    
    }



    
}
