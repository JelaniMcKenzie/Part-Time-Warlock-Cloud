using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntity : MonoBehaviour
{
    public float health = 100;
    public float moveSpeed = 5f; //must be a high value for things like the player (e.g. 500f)
    public bool isFrozen = false;
    public bool isBurning = false;
    public bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // add death animation or sound effect here
        Destroy(gameObject);
    }

    public virtual void AddKnockBack()
    {
        if (this.gameObject.CompareTag("Player"))
        {

        }
        else if (this.gameObject.CompareTag("Enemy"))
        {

        }
    }

    /*public virtual void Burn()
    {
        //Method to be overidden by derived classes.
        //Some enemies may have a positive effect when
        //this method is called
    }*/

    /*public virtual void Freeze()
    {
        //Method to be overidden by derived classes.
        //Some enemies may have a positive effect when
        //this method is called
    }*/



}
