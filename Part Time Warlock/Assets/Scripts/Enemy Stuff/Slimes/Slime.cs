using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Slime : GameEntity
{
    private Player P = null;

    [SerializeField] public GameObject EnemyDeathAnim;

    //public Animator Anim = null;
    public bool frozen = false;
    public UIManager UI = null;
    public bool isOnFire = false;
    private bool hasPlayedSound = false;
    public Rigidbody2D rb;


    public float chaseDistance = 1000f; // Set the distance at which the enemy starts chasing
    public float chaseDuration = 3f; // Set the duration the enemy chases after the player is out of range

    private bool isChasing = false;
    private float chaseTimer = 0f;
    public SpriteRenderer detectJump;
    public Sprite[] groundSprites;

    [SerializeField] private AudioClip slimeMove;

    [SerializeField] public GameObject CoinPrefab = null;
    [SerializeField] public GameObject BigCoinPrefab = null;

    // Start is called before the first frame update
    void Start()
    {

        health = 143;
        canMove = false;
        P = FindAnyObjectByType<Player>();
        UI = FindAnyObjectByType<UIManager>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        //SM.EnemyCount++;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, P.transform.position);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

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
            rb.velocity = new Vector3(0, 0, 0);
        }

        if (health <= 0f)
        {
            Die();
            //SM.EnemyCount--;
        }
    }

    public void EnemyMovement()
    {

        bool onGround = false;

        foreach (Sprite s in groundSprites)
        {
            //if the slime animation isn't in its "hopping" phase,
            //then set onGround to true and stop it from moving
            if (detectJump.sprite == s)
            {
                onGround = true;
            }
        }

        if (onGround == false)
        {
            if (hasPlayedSound == false)
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
        
        if (other.TryGetComponent<DamageSpell>(out var damageSpell))
        {
            TakeDamage(damageSpell.damage);
            StartCoroutine(DamageFlash());

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
        Instantiate(EnemyDeathAnim, transform.position, Quaternion.identity);

        int spawnCoin = Random.Range(0, 2);
        int spawnBigCoin = Random.Range(0, 6);
        if (spawnCoin == 1)
        {
            GameObject K = Instantiate(CoinPrefab, transform.position, Quaternion.identity);
            K.transform.parent = null;
        }

        if (spawnBigCoin == 2)
        {
            Instantiate(BigCoinPrefab, transform.position, Quaternion.identity);
        }
        Destroy(this.gameObject);
    }

    public IEnumerator FreezeTime()
    {
        moveSpeed = 0f;
        isFrozen = true;
        sprite.color = new Color32(0, 210, 210, 80);
        yield return new WaitForSeconds(2.5f);
        sprite.color = new Color32(255, 255, 255, 255);
        moveSpeed = 4f;
        isFrozen = false;
    }

    public IEnumerator DamageFlash()
    {
        sprite.color = new Color32(100, 0, 0, 255);
        yield return new WaitForSeconds(0.25f);
        sprite.color = new Color32(255, 255, 255, 255);
    }

    
}
