using Edgar.Unity;
using Edgar.Unity.Examples.Gungeon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShadowArcher : GameEntity
{
    /// <summary>
    /// What this code needs to do:
    /// Locate the player, draw a line from the bow to the player
    /// wait an interval of time before firing, then fire
    /// dissapear
    /// move in a random direction towards the player
    /// reappear
    /// rinse repeat
    /// 
    /// If the enemy is a certain distance away from the player, it doesn't move towards it
    /// </summary>
    /// 
    /***
     * 
     * TODO: Fix movement logic. it sucks.
     * Get random position within floor collider to disappear and reappear
     * 
     * similar to enemy spawning logic
     * get current room you're in
     * get gungeon game manager
     * get floor tilemap
     * get floor collider
     * choose random pos in floor collider
     * Do this the same way the spawner handles spawning enemies
     */

    public GameObject arrow;
    public GameObject bow;
    public GameObject firePos;
    public SpriteRenderer bowSprite;
    public WizardPlayer player;
    public float fireInterval = 2f;
    public float minDistance = 10f; //change this value to adjust the movement range

    private bool canFire = true;
    public Animator animator;
    public Animator bowAnim;
    private BoxCollider2D boxCollider;

    //get component in parent?
    GungeonRoomManager roomManager;

    private Coroutine fireCoroutine;
    private List<Coroutine> activeCoroutines = new List<Coroutine>();

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        canMove = true;
        moveSpeed = 2f;
        player = FindAnyObjectByType<WizardPlayer>();

        roomManager = GetComponentInParent<GungeonRoomManager>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        bowAnim = bow.GetComponent<Animator>();
        gameManager = FindAnyObjectByType<GameManager>();
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        if (!isFrozen)
        {
            if (canMove)
            {
                AimAtPlayer();
            }

            // Draw a line from the bow to the player
            Debug.DrawLine(bow.transform.position, player.transform.position, Color.red);

            // Check if it's time to fire
            if (canFire && canMove)
            {
                fireCoroutine = StartCoroutine(Fire());
            }

            animator.GetCurrentAnimatorClipInfo(0);
        }
    }

    public override void Die()
    {
        base.Die();
    }

    private IEnumerator Shoot()
    {
        bow.GetComponent<Animator>().SetTrigger("Shoot");
        yield return new WaitForSeconds(1.375f);
        GameObject a = Instantiate(arrow, firePos.transform.position, Quaternion.identity);
        a.transform.parent = null;
        a.transform.localScale = new Vector3(1, 1, 1);
        a.transform.position = transform.position;
        a.transform.LookAt(player.transform.position);
        Destroy(a, 2.5f);
    }

    private void AimAtPlayer()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;

        // Flip the sprite based on player position
        if (directionToPlayer.x < 0)
        {
            sprite.flipX = false;
            bowSprite.flipY = true;
        }
        else
        {
            sprite.flipX = true;
            bowSprite.flipY = false;
        }

        // Rotate the bow toward the player
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        bow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private IEnumerator Fire()
    {
        canFire = false;
        yield return new WaitForSeconds(0.833f);

        if (isFrozen) yield break; // Check if frozen before firing

        yield return new WaitForSeconds(fireInterval);

        if (isFrozen) yield break; // Check if frozen before shooting

        StartCoroutine(Shoot());
        yield return new WaitForSeconds(2f);

        if (isFrozen) yield break; // Check if frozen before disappearing

        animator.SetTrigger("Disappear");
        yield return new WaitForSeconds(0.75f);

        if (isFrozen) yield break; // Check if frozen before teleporting

        FindTeleportSpot(transform.position);
        canFire = true;
    }

    public void FindTeleportSpot(Vector2 position)
    {
        if (roomManager != null)
        {
            do
            {
                position = GetRandomTeleportPosition();
            } while (!IsPointWithinCollider(roomManager.FloorCollider, position) ||
                     Physics2D.OverlapCircleAll(position, 0.5f).Any(x => !x.isTrigger));

            transform.position = position;
            boxCollider.enabled = true;
            sprite.color = Color.white;
            animator.SetTrigger("Appear");
        }
        else
        {
            Debug.LogError("Room Manager is null!");
        }
    }

    private Vector2 GetRandomTeleportPosition()
    {
        return RandomPointInBounds(roomManager.FloorCollider.bounds, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TryGetComponent<IDamageable>(out var damageable))
        {
            Debug.Log("HasDamageable");


            if (collision.TryGetComponent<DamageSpell>(out var damageSpell))
            {
                Vector2 knockbackDirection;

                if (damageSpell.TryGetComponent<PlayerProjectiles>(out var projectile))
                {
                    Debug.LogWarning("PROJECTILE!");
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


        if (collision.CompareTag("FireWall"))
        {
            base.Burn();
        }

        if (collision.CompareTag("Ice"))
        {
            if (!isFrozen)
            {
                StartCoroutine(FreezeTime());
            }
        }
    }

    public IEnumerator FreezeTime()
    {
        isFrozen = true;
        moveSpeed = 0f;
        sprite.color = new Color32(0, 186, 192, 255);
        canFire = false;

        // Stop and clear all active coroutines
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }
        foreach (var coroutine in activeCoroutines)
        {
            StopCoroutine(coroutine);
        }
        activeCoroutines.Clear();

        // Pause the animator
        animator.speed = 0;
        bowAnim.speed = 0;

        yield return new WaitForSeconds(2.5f);

        
        moveSpeed = 2f; // Or your desired move speed
        isFrozen = false;
        canFire = true;

        // Resume the animator
        animator.speed = 1;
        bowAnim.speed = 1;
        sprite.color = new Color32(255, 255, 255, 255);
    }

    private static bool IsPointWithinCollider(Collider2D collider, Vector2 point)
    {
        return collider.OverlapPoint(point);
    }

    public static Vector2 RandomPointInBounds(Bounds bounds, float margin = 0)
    {
        return new Vector2(
            RandomRange(bounds.min.x + margin, bounds.max.x - margin),
            RandomRange(bounds.min.y + margin, bounds.max.y - margin)
        );
    }

    private static float RandomRange(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public IEnumerator DamageFlash()
    {
        sprite.color = new Color32(100, 0, 0, 255);
        yield return new WaitForSeconds(0.25f);
        sprite.color = new Color32(255, 255, 255, 255);
    }

    private void ShowBow()
    {
        bow.SetActive(true);
    }

    private void HideBow()
    {
        bow.SetActive(false);
    }

}
