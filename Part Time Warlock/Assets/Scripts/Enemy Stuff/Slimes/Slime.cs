using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Slime : GameEntity
{
    private WizardPlayer P = null;

    [SerializeField] public GameObject EnemyDeathAnim;

    //public Animator Anim = null;
    public bool frozen = false;
    public UIManager UI = null;
    public bool isOnFire = false;
    private bool hasPlayedSound = false;


    public float chaseDistance = 100f; // Set the distance at which the enemy starts chasing
    public float chaseDuration = 3f; // Set the duration the enemy chases after the player is out of range

    private bool isChasing = false;
    private float chaseTimer = 0f;
    public SpriteRenderer detectJump;
    public Sprite[] groundSprites;

    [SerializeField] private AudioClip slimeMove;

    // Start is called before the first frame update
    void Start()
    {

        health = 143;
        canMove = false;
        P = FindAnyObjectByType<WizardPlayer>();
        UI = FindAnyObjectByType<UIManager>();
        //rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        gameManager = FindAnyObjectByType<GameManager>();
        //SM.EnemyCount++;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, P.transform.position);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (currentState != State.Hit)
        {
            if (distanceToPlayer <= chaseDistance)
            {
                canMove = true;
                if (canMove == true)
                {
                    if (!isChasing)
                    {
                        isChasing = true;
                        chaseTimer = chaseDuration;
                    }

                    // Call EnemyMovement() only when chasing
                    EnemyMovement();
                }
            }
            else
            {
                // Continue chasing for a certain duration even if the player is out of range
                chaseTimer -= Time.deltaTime;

                if (chaseTimer <= 0f)
                {
                    // Stop chasing
                    isChasing = false;
                    canMove = false;
                    //Debug.Log("Chase stopped!");
                }
            }

            if (canMove == false)
            {
                rb.velocity = Vector3.zero;
            }
        }  
    }

    public void EnemyMovement()
    {
        if (currentState == State.Hit)
        {
            return;
        }

        bool onGround = false;

        foreach (Sprite s in groundSprites)
        {
            //if the slime animation isn't in its "hopping" phase,
            //then set onGround to true and stop it from moving
            if (detectJump.sprite == s)
            {
                onGround = true;
                currentState = State.Idle;
                return;
            }
        }

        if (!onGround)
        {
            currentState = State.Moving;
            if (!hasPlayedSound)
            {
                StartCoroutine(PlaySoundAndWait());
            }
            // Calculate the direction towards the player
            Vector2 direction = (P.transform.position - transform.position).normalized;

            // Move the enemy towards the player
            rb.velocity = moveSpeed * direction;

        }
    }

    public void ResetSoundFlag()
    {
        hasPlayedSound = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (TryGetComponent<IDamageable>(out var damageable))
        {
            Debug.Log("HasDamageable");
            

            if (other.TryGetComponent<DamageSpell>(out var damageSpell))
            {
                Vector2 knockbackDirection;

                if (damageSpell.TryGetComponent<PlayerProjectiles>(out var projectile))
                {
                    //grab the velocity of the projectile to ensure knockback is straight back relative to the projectile's incoming direction
                    knockbackDirection = projectile.GetComponent<Rigidbody2D>().velocity.normalized;
                }
                else
                {
                    //offset for collision detection changes the direction where the force comes from
                    knockbackDirection = (transform.position - damageSpell.transform.position).normalized;
                }

                Vector2 knockback = knockbackDirection * damageSpell.knockbackForce; //reverse knockback force to send in the other direction

                //After making sure that the collider has a script that implements IDamagable, we can run the OnHit implementation and pass our Vector2 force
                damageable.OnHit(damageSpell.damage, knockback);

                if (isFrozen == false)
                {
                    StartCoroutine(DamageFlash());
                }


            }
        }
        else
        {
            Debug.LogWarning("IDamageable is null!");
        }
        

        if (other.CompareTag("Ice"))
        {
            //Destroy GameObject
            //Instantiate "EnemyFreeze" object that's basically a mannequin with an ice block
            //Or instantaiate an opaque ice block on top of it that breaks after five seconds

            StartCoroutine(FreezeTime());

        }

        if (other.CompareTag("FireWall"))
        {
            base.Burn();
        }

    }


    public IEnumerator PlaySoundAndWait()
    {
        hasPlayedSound = true;

        AudioSource.PlayClipAtPoint(slimeMove, transform.position);

        // Wait for the length of the audio clip
        yield return new WaitForSeconds(slimeMove.length);

        hasPlayedSound = false;
    }

    public IEnumerator ChaseTimer()
    {
        yield return new WaitForSeconds(3f);
        canMove = false;
    }

    public override void Die()
    {
        base.Die();
    }

    public IEnumerator FreezeTime()
    {
        moveSpeed = 0f;
        isFrozen = true;
        sprite.color = new Color32(0, 63, 255, 255);
        yield return new WaitForSeconds(2.5f);
        moveSpeed = 4f;
        isFrozen = false;
        sprite.color = new Color32(255, 255, 255, 255);
    }

    public IEnumerator DamageFlash()
    {
        sprite.color = new Color32(100, 0, 0, 255);
        yield return new WaitForSeconds(0.25f);
        sprite.color = new Color32(255, 255, 255, 255);
    }

    
}
