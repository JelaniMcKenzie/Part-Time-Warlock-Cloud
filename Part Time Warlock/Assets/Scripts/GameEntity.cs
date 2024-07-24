using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntity : MonoBehaviour, IDamageable
{
    [SerializeField] protected float health = 100f;
    public float moveSpeed = 5f; //must be a high value for things like the player (e.g. 500f)
    public bool isFrozen = false;
    public bool isBurning = false;
    protected bool isHit = false;
    public bool canMove = true;
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public GameManager gameManager;

    public GameObject onDeath;

    //a base burnSeconds float to be manipulated by other objects
    public float burnSeconds = 3f;

    //a base freezeSeconds float to be manipulated by other objects
    public float freezeSeconds;

    public float knockbackForce = 100f; //Set the default knocback force (if there is knockback for damage)
    public bool canTurnInvincible;

    protected enum State { Idle, Moving, Hit }
    [SerializeField] protected State currentState = State.Idle;
    #region IDamageable properties
    // IDamageable properties
    public float Health
    {
        get { return health; }
        set { health = value; }
    }

    public bool Targetable { get; set; }
    public bool Invincible { get; set; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Initialize Targetable and Invincible if necessary
        Targetable = true;
        Invincible = false;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    // IDamageable methods
    public void OnHit(float damage, Vector2 knockback)
    {
        if (!Invincible)
        {
            isHit = true;
            TakeDamage(damage);
            ApplyKnockback(knockback);

            /*if (canTurnInvincible)
            {
                //Activate invincibility and timer
                Invincible = true;
            }*/
        }
    }

    

    public void OnHit(float damage)
    {
        if (!Invincible)
        {
            isHit = true;
            TakeDamage(damage);
            isHit = false;
            /*if (canTurnInvincible)
            {
                //Activate invincibility and timer
                Invincible = true;
            }
            }*/
        }
    }

    public void ApplyKnockback(Vector2 knockback)
    {
        currentState = State.Hit;
        rb.AddForce(knockback, ForceMode2D.Impulse);
        StartCoroutine(RecoverFromHit());

    }

    private IEnumerator RecoverFromHit()
    {
        yield return new WaitForSeconds(0.5f);
        isHit = false;
        currentState = State.Idle;
    }

    public void OnObjectDestroyed()
    {
        Die();
    }

    public virtual void TakeDamage(float amount)
    {
        Health -= amount;
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

    /*public virtual void AddKnockBack(Vector2 knockback)
    {
        // Implement knockback logic here
        if (this.gameObject.CompareTag("Player"))
        {
            // Player-specific knockback logic
        }
        else if (this.gameObject.CompareTag("Enemy"))
        {
            // Enemy-specific knockback logic
        }
    }*/

    public virtual void Burn()
    {
        FireQuartzLogic fql = FindAnyObjectByType<FireQuartzLogic>();

        if (fql != null)
        {
            burnSeconds *= 2f;
        }

        isBurning = true;
        //Method to be overridden by derived classes.
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
        //Method to be overridden by derived classes.
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
