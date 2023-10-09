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
        anim.speed = 1;

        if (canMove == true)
        {
            if (isFrozen == true) { anim.speed = 0; }
            anim.SetBool("isChasing", true);
        }
        else
        {
            if (isFrozen == true) { anim.speed = 0; }
            anim.SetBool("isChasing", false);

        }
    }

    //if attack range, then attack anim
}
