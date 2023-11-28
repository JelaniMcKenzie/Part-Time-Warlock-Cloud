using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimations : MonoBehaviour
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
        Slime slime = GetComponent<Slime>();
        if (slime != null)
        {
            if (slime.canMove == true)
            {
                if (slime.isFrozen == true) { anim.speed = 0; }
                anim.SetBool("isChasing", true);
            }
            else
            {
                if (slime.isFrozen == true) { anim.speed = 0; }
                anim.SetBool("isChasing", false);

            }
        }
        
    }

    //if attack range, then attack anim
}
