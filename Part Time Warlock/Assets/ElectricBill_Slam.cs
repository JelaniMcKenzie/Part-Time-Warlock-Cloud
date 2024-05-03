using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBill_Slam : StateMachineBehaviour
{
    Player player;
    Rigidbody2D rb;
    ElectricBill eBill;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = FindAnyObjectByType<Player>();
        rb = animator.GetComponent<Rigidbody2D>();
        eBill = animator.GetComponent<ElectricBill>();

        if (animator == null)
        {
            Debug.LogError("cannot find animator");
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (eBill.isSlamming)
        {
            animator.SetTrigger("Slam");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Slam");
    }

}
