using PixelCrushers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Playeranimations : MonoBehaviour
{
    public float xAxis;
    public float yAxis;
    public Animator anim = null;
    public ArmorController armorController;
    public Player P = null;
    public string currentanim;
    public string direction;
    AnimatorStateInfo animState;
    public Scene activeScene;

    public GameObject hand = null;
    public GameObject staff = null;

    private SpriteRenderer playerSprite;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        P = this.gameObject.GetComponent<Player>();
        activeScene = SceneManager.GetActiveScene();
        armorController = GetComponent<ArmorController>();
        playerSprite = P.GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        anim.GetCurrentAnimatorClipInfo(0); //0 is the default animation layer
                                            //currentanim = "Idle";
        if (P.canMove == true)
        {
            CombatAnim();
        }
    }

    private void MouseAim()
    {
        if (P != null)
        {
            
            Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            dir = Vector3.MoveTowards(new Vector3(), dir, 0.1f) * 1000;
            float xDistance = Mathf.Abs(dir.x);
            float yDistance = Mathf.Abs(dir.y);
            if (xDistance > yDistance || yDistance > xDistance)
            {
                if (dir.x < -0.1 /*player is looking left. the 0 references the player position*/)
                {
                    /*direction = "Left";
                    currentanim = "IdleLeft";*/
                    P.GetComponent<SpriteRenderer>().flipX = true;
                    
                }
                if (dir.x > 0.1 /*player is looking right*/)
                {
                    /*direction = "Right";
                    currentanim = "IdleRight";*/
                    P.GetComponent<SpriteRenderer>().flipX = false;
                    

                }
            }

        }
        else
        {
            Debug.LogError("Player is null for some reason");
        }

        //TEMPROARY CHANGE JUST SEEING HOW THE LEFT/RIGHT MECHANIC FELT

        /* else
        {
        //player is looking down
        if (dir.y < -10 )
            {
                direction = "Down";
                currentanim = "Idle";
               
                
            }
        //player is looking up
        if (dir.y > 10 )
            {
                direction = "Up";
                currentanim = "IdleUp";
                
                
            } 
        } */
    }

    private void CombatAnim() {
        MouseAim();
        //--------------------NORMAL WASD----------------
        /*if (Input.GetKeyDown(KeyCode.D) || xAxis >= 1)
        {
            currentanim = direction;
        }
        if (Input.GetKeyDown(KeyCode.A) || xAxis <= -1)
        {
            currentanim = direction;
        }
        if (Input.GetKeyDown(KeyCode.W) || yAxis >= 1)
        {
            currentanim = direction;
        }
        if (Input.GetKeyDown(KeyCode.S) || yAxis <= -1)
        {
            currentanim = direction;
        }*/

        
         
        if (P.moveInput.x == 0 && P.moveInput.y == 0)
        {
            anim.SetBool("idle", true);
            anim.SetBool("running", false);
            anim.SetBool("dashing", false);

        } 
        else
        {
            anim.SetBool("idle", false);
            if (P.isDashing == true)
            {
                anim.SetBool("dashing", true);
                anim.SetBool("running", false);
            }
            else
            {
                anim.SetBool("running", true);
                anim.SetBool("dashing", false);
            }
        }

        /*if (direction == "Down") //change for if the mouse is on the southern hemisphere of the player
        {
            hand.GetComponent<SpriteRenderer>().sortingOrder = 5;
            staff.GetComponent<SpriteRenderer>().sortingOrder = 4;
        }
        else
        {
            hand.GetComponent<SpriteRenderer>().sortingOrder = 1;
            staff.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }*/

        /*if (!animState.IsName(currentanim))
        {   //checks what the name of the animation is (e.g right)
            //if the given animation ISN'T playing when the key is pressed, it immediately plays it
            anim.Play(currentanim, 0);
        }*/

    }

    private void Apartmentanim()
    {
        if (P.moveInput.x == 0 && P.moveInput.y == 0)
        {
            anim.SetBool("idle", true);
            anim.SetBool("running", false);
            anim.SetBool("dashing", false);
        }
        else
        {
            anim.SetBool("idle", false);
            if (P.isDashing == true)
            {
                anim.SetBool("dashing", true);
                anim.SetBool("running", false);
            }
            else
            {
                anim.SetBool("running", true);
                anim.SetBool("dashing", false);
            }

            if (P.moveInput.x < 0)
            {
                playerSprite.flipX = true;
            }
            else if (P.moveInput.x > 0)
            {
                playerSprite.flipX = false;
            }
        }
    }
}
