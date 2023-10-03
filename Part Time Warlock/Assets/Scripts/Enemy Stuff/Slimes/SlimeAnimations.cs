using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimations : Slime
{

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            canMove = true;
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            canMove = false;
        }
        if (canMove == true)
        {
            anim.SetBool("isChasing", true);
        }
        else
        {
            anim.SetBool("isChasing", false);

        }
    }

    //if attack range, then attack anim
}
