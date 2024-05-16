using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameEntity : MonoBehaviour
{
    [SerializeField] protected float health = 100f;
    public float moveSpeed = 5f; //must be a high value for things like the player (e.g. 500f)
    public bool isFrozen = false;
    public bool isBurning = false;
    public bool canMove = true;
    public SpriteRenderer sprite;
    public GameManager gameManager;

    public GameObject onDeath;

    //a base burnSeconds float to be manipulated by other objects
    public float burnSeconds = 5f;

    //a base freezeSeconds float to be manipulated by other objects
    public float freezeSeconds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Instantiate(onDeath, transform.position, Quaternion.identity);
        if (this.gameObject.CompareTag("Enemy"))
        {
            gameManager.enemiesKilled++;
        }
        // add death animation or sound effect here
        Destroy(this.gameObject);
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

    public virtual void Burn()
    {
        FireQuartzLogic fql = FindAnyObjectByType<FireQuartzLogic>();

        if (fql != null)
        {
            burnSeconds *= 2f;
        }

        isBurning = true;
        //Method to be overidden by derived classes.
        //Some enemies may have a positive effect when
        //this method is called
        StartCoroutine(Aflame());
        StartCoroutine(BurnTime());

        IEnumerator Aflame()
        {
            sprite.color = new Color32(222, 70, 97, 255);
            for (int s = 0; s <= burnSeconds; s++)
            {
                health -= 2;
                yield return new WaitForSeconds(1.5f);
            }
        }

        IEnumerator BurnTime()
        {
            yield return new WaitForSeconds(burnSeconds);
            isBurning = false;
            sprite.color = new Color32(255, 255, 255, 255);
        }
    }

    public virtual void Freeze()
    {
        isFrozen = true;
        StartCoroutine(Frozen());
        isFrozen = false;
        //Method to be overidden by derived classes.
        //Some enemies may have a positive effect when
        //this method is called
    }

    public IEnumerator Frozen()
    {
        moveSpeed = 0f;
        sprite.color = new Color32(0, 210, 210, 80);
        yield return new WaitForSeconds(freezeSeconds);
        sprite.color = new Color32(255, 255, 255, 255);
        moveSpeed = 4f;
    }





}
