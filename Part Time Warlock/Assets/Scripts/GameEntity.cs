using System.Collections;
using UnityEngine;
using static IDamageable;

public class GameEntity : MonoBehaviour, IDamageable
{
    [SerializeField] protected float health = 100f;
    [SerializeField] protected SimpleFlash flashEffect;
    public float moveSpeed = 5f; //must be a high value for things like the player (e.g. 500f)
    public float attackingDamage = 0f; //varies for each attacking entity. pass this value into OnHit
    public bool isFrozen = false;
    public bool isBurning = false;
    protected bool isHit = false;
    public bool canMove = true;
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public Color flashColor = Color.white;
    public GameManager gameManager;

    public GameObject onDeath;
    public GameObject iceBlockPrefab;
    [SerializeField] protected GameObject activeIceBlock;

    //a base burnSeconds float to be manipulated by other objects
    public float burnSeconds = 3f;

    //a base freezeSeconds float to be manipulated by other objects
    public float freezeSeconds = 5f;

    public float knockbackForce = 100f; //Set the default knocback force (if there is knockback for damage)
    public bool canTurnInvincible;

    protected enum State { Idle, Moving, Hit, Stun, Stagger }
    
    [SerializeField] protected State currentState = State.Idle;

    protected DamageType damageType = DamageType.Melee;
    #region IDamageable properties
    // IDamageable properties
    public float Health
    {
        get { return health; }
        set { health = value; }
    }

    public bool Targetable { get; set; }
    public bool Invincible { get; set; }
    public SimpleFlash FlashEffect
    {
        get { return flashEffect; }
        set { flashEffect = value; }

    }
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
    public virtual void OnHit(float damage, KnockbackData knockback, DamageType _attackType)
    {
        if (!Invincible)
        {
            switch (_attackType)
            {
                case DamageType.Projectile:

                    break;
                case DamageType.Melee:
                    break;
                case DamageType.AOE: 
                    break;
                
            }
            isHit = true;
            // Disable collision with the PitBorder layer
            Physics2D.IgnoreLayerCollision(this.gameObject.layer, LayerMask.NameToLayer("PitBorder"), true);

            FlashEffect.Flash(flashColor);
            //Stun state logic here. How long are enemies/players stunned for?
            TakeDamage(damage);
            ApplyKnockback(knockback);

            // Enable collision with the PitBorder layer
            Physics2D.IgnoreLayerCollision(this.gameObject.layer, LayerMask.NameToLayer("PitBorder"), false);

            /*if (canTurnInvincible)
            {
                //Activate invincibility and timer
                Invincible = true;
            }*/
        }
    }



    public virtual void OnHit(float damage)
    {
        if (!Invincible)
        {
            isHit = true;
            // Disable collision with the PitBorder layer
            Physics2D.IgnoreLayerCollision(this.gameObject.layer, LayerMask.NameToLayer("PitBorder"), true);

            FlashEffect.Flash(flashColor);
            TakeDamage(damage);
            isHit = false;

            // Enable collision with the PitBorder layer
            Physics2D.IgnoreLayerCollision(this.gameObject.layer, LayerMask.NameToLayer("PitBorder"), false);
            /*if (canTurnInvincible)
            {
                //Activate invincibility and timer
                Invincible = true;
            }
            }*/
        }
    }

    public virtual void ApplyKnockback(KnockbackData knockbackData)
    {
        if (currentState == State.Hit) return; // Prevent overlapping knockbacks

        currentState = State.Hit;
        isHit = true;

        // Apply knockback force
        rb.velocity = Vector2.zero; // Reset velocity to avoid stacking forces
        rb.AddForce(knockbackData.Direction * knockbackData.Force, ForceMode2D.Impulse);

        // Start knockback handling coroutine
        StartCoroutine(HandleKnockback(knockbackData.Duration));
    }


    private IEnumerator HandleKnockback(float duration)
    {
        // Ignore collision with the PitBorder layer
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("PitBorder"), true);

        yield return new WaitForSeconds(duration);
        isHit = false;
        currentState = State.Idle;

        // Re-enable the collision with the PitBorder layer
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("PitBorder"), false);
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


        //Method to be overridden by derived classes.
        //Some enemies may have a positive effect when
        //this method is called
    }

    public virtual IEnumerator Frozen()
    {
        moveSpeed = 0f;
        if (activeIceBlock == null)
        {
            activeIceBlock = Instantiate(iceBlockPrefab, transform.position, Quaternion.identity);
            activeIceBlock.transform.SetParent(transform);
            activeIceBlock.transform.localPosition = Vector3.zero;
            activeIceBlock.transform.localScale = transform.localScale * 1.125f;
        }


        if (TryGetComponent<Animator>(out var Anim))
        {
            Anim.Play(Anim.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, 0f);
            Anim.Rebind();
        }
        yield return new WaitForSeconds(freezeSeconds);
        moveSpeed = 4f;
        isFrozen = false;
        if (activeIceBlock != null)
        {
            Destroy(activeIceBlock);
            activeIceBlock = null;
        }
    }
}
