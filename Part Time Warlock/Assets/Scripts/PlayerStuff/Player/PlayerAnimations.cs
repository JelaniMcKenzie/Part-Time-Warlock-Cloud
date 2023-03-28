using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public float xAxis; 
    public float yAxis; 
    public Animator Anim = null;
    public Player P = null;
    public string currentAnim = "Idle";
    public string direction = "Right";
    AnimatorStateInfo animState;

    public GameObject hand = null;
    public GameObject staff = null;
    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        P = FindObjectOfType<Player>();
        
    }

    // Update is called once per frame
    void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        Anim.GetCurrentAnimatorStateInfo(0); //0 is the default animation layer
        //currentAnim = "Idle";
    
        if (P.canMove == true)
        {
            MouseAim();
            //--------------------NORMAL WASD----------------

            if (Input.GetKey(KeyCode.D) || xAxis >= 1)
            {
                currentAnim = direction;
            }
            if (Input.GetKey(KeyCode.A) || xAxis <= -1)
            {
                currentAnim = direction;
            }
            if (Input.GetKey(KeyCode.W) || yAxis >= 1)
            {
                currentAnim = direction;
            }
            if (Input.GetKey(KeyCode.S) || yAxis <= -1)
            {
                currentAnim = direction;
            }

            if (!animState.IsName(currentAnim))
            {   //checks what the name of the animation is (e.g right)
                //if the given animation ISN'T playing when the key is pressed, it immediately plays it
                Anim.Play(currentAnim, 0);
            }
            

            if (direction == "Down")
            {
                hand.GetComponent<SpriteRenderer>().sortingOrder = 5;
                staff.GetComponent<SpriteRenderer>().sortingOrder = 4;
            }
            else
            {
                hand.GetComponent<SpriteRenderer>().sortingOrder = 3;
                staff.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
        }
    }

    private void MouseAim()
    {
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        dir = Vector3.MoveTowards(new Vector3(), dir, 0.1f) * 1000;
        float xDistance = Mathf.Abs(dir.x);
        float yDistance = Mathf.Abs(dir.y);
        if (xDistance > yDistance)
        {
            if (dir.x < -10 /*player is looking left. the 0 references the player position*/)
            {
                direction = "Left";
                currentAnim = "IdleLeft";
                
                
            }
            if (dir.x > 10 /*player is looking right*/)
            {
                direction = "Right";
                currentAnim = "IdleRight";
                
                
            }
        }
        else
        {
            if (dir.y < -10 /*player is looking down*/)
            {
                direction = "Down";
                currentAnim = "Idle";
               
                
            }
            if (dir.y > 10 /*player is looking up*/)
            {
                direction = "Up";
                currentAnim = "IdleUp";
                
                
            }
        }








    }
}
