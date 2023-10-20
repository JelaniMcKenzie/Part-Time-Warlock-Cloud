using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAnimations : MonoBehaviour
{
    public float xAxis;
    public float yAxis;
    public Animator Anim = null;
    public Player P = null;
    public string currentAnim;
    public string direction;
    AnimatorStateInfo animState;
    public Scene activeScene;

    public GameObject hand = null;
    public GameObject staff = null;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        P = FindAnyObjectByType<Player>();
        activeScene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        Anim.GetCurrentAnimatorClipInfo(0); //0 is the default animation layer
                                            //currentAnim = "Idle";
        if (P.canMove == true)
        {
            if (activeScene.name != "Apartment")
            {
                CombatAnim();
            } else
            {
                ApartmentAnim();

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
                /*direction = "Left";
                currentAnim = "IdleLeft";*/
                P.GetComponent<SpriteRenderer>().flipX = true;
                

            }
            if (dir.x > 10 /*player is looking right*/)
            {
                /*direction = "Right";
                currentAnim = "IdleRight";*/
                P.GetComponent<SpriteRenderer>().flipX = false;
                
            }
        }

        //TEMPROARY CHANGE JUST SEEING HOW THE LEFT/RIGHT MECHANIC FELT

        /* else
        {
        //player is looking down
        if (dir.y < -10 )
            {
                direction = "Down";
                currentAnim = "Idle";
               
                
            }
        //player is looking up
        if (dir.y > 10 )
            {
                direction = "Up";
                currentAnim = "IdleUp";
                
                
            } 
        } */
    }

    private void CombatAnim() {
        MouseAim();
        //--------------------NORMAL WASD----------------
        /*if (Input.GetKeyDown(KeyCode.D) || xAxis >= 1)
        {
            currentAnim = direction;
        }
        if (Input.GetKeyDown(KeyCode.A) || xAxis <= -1)
        {
            currentAnim = direction;
        }
        if (Input.GetKeyDown(KeyCode.W) || yAxis >= 1)
        {
            currentAnim = direction;
        }
        if (Input.GetKeyDown(KeyCode.S) || yAxis <= -1)
        {
            currentAnim = direction;
        }*/

        float moveInputX = Input.GetAxisRaw("Horizontal");
        float moveInputY = Input.GetAxisRaw("Vertical");
         
        if (moveInputX == 0 && moveInputY == 0)
        {
            Anim.SetBool("running", false);
        } 
        else
        {
            Anim.SetBool("running", true);
        }



        if (direction == "Down") //change for if the mouse is on the southern hemisphere of the player
        {
            hand.GetComponent<SpriteRenderer>().sortingOrder = 5;
            staff.GetComponent<SpriteRenderer>().sortingOrder = 4;
        }
        else
        {
            hand.GetComponent<SpriteRenderer>().sortingOrder = 1;
            staff.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
        
        /*if (!animState.IsName(currentAnim))
        {   //checks what the name of the animation is (e.g right)
            //if the given animation ISN'T playing when the key is pressed, it immediately plays it
            Anim.Play(currentAnim, 0);
        }*/

    }

    private void ApartmentAnim()
    {
        // Determine the direction of movement based on the x and y axis values
        if (xAxis > 0)
        {
            currentAnim = "Right";
        }
        else if (xAxis < 0)
        {
            currentAnim = "Left";
        }
        else if (yAxis > 0)
        {
            currentAnim = "Up";
        }
        else if (yAxis < 0)
        {
            currentAnim = "Down";
        }
        else
        {
            // If there is no movement, play the idle animation in the current direction
            switch (currentAnim)
            {
                case "Right":
                    currentAnim = "IdleRight";
                    break;
                case "Left":
                    currentAnim = "IdleLeft";
                    break;
                case "Up":
                    currentAnim = "IdleUp";
                    break;
                case "Down":
                    currentAnim = "Idle";
                    break;
            }
        }

        // Check if the current animation is already playing to prevent unnecessary animation restarts
        if (!animState.IsName(currentAnim))
        {
            Anim.Play(currentAnim, 0);
        }
    }
}
