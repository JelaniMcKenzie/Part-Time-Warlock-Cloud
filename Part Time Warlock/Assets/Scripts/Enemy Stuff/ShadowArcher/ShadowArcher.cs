using System.Collections;
using System.Collections.Generic;
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
     * TODO: Fix movement logic. it sucks
     */

    public GameObject arrow;
    public GameObject bow;
    public GameObject firePos;
    public SpriteRenderer bowSprite;
    public Player player;
    public float fireInterval = 2f;
    public float detectionDistance = 10f;

    private bool isFiring;
    private bool isMovingTowardsPlayer = true;

    public LineRenderer lineRenderer;
    public float lineUpdateInterval = 0.1f; // Interval for updating the line renderer

    private float lineUpdateTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
       canMove = true;
       player = FindAnyObjectByType<Player>();
       StartCoroutine(EnemyBehaviorLoop());
    }

    // Update is called once per frame
    void Update()
    {
        DrawLineToPlayer();

        // Update line renderer position periodically
        lineUpdateTimer += Time.deltaTime;
        if (lineUpdateTimer >= lineUpdateInterval)
        {
            lineUpdateTimer = 0f;
            UpdateLineRenderer();
        }

        if (isFiring == false)
        {
            isMovingTowardsPlayer = true;
        }
        else
        {
            isMovingTowardsPlayer = false;
        }

        MoveTowardsPlayer();

    }

    public override void Die()
    {
        base.Die();
    }

    IEnumerator EnemyBehaviorLoop()
    {
        while (true)
        {
            // Locate the player
            LocatePlayer();

            // Draw a line from the bow to the player
            

            // Wait for interval before firing
            yield return new WaitForSeconds(fireInterval);

            // Fire
            Fire();

            // Disappear
            //gameObject.SetActive(false);
            //transform.localScale = Vector2.zero;

            // Move in a random direction towards the player

            // Reappear
            //gameObject.SetActive(true);
            //transform.localScale = Vector2.one;

            // Rinse and repeat
            //yield return null;
        }
    }

    void LocatePlayer()
    {
        if (player != null)
        {
            // Check the distance between enemy and player
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            // If the player is within detection distance, move towards it
            if (distanceToPlayer <= detectionDistance)
            {
                isMovingTowardsPlayer = true;
            }
            else
            {
                isMovingTowardsPlayer = false;
            }
        }
    }

    

    void DrawLineToPlayer()
    {
        if (player != null && bow != null)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;

            // Flip the sprite based on player position
            if (directionToPlayer.x < 0)
            {
                sprite.flipX = false;
                bowSprite.flipX = true;
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

            // Update Line Renderer
            UpdateLineRenderer();
        }
    }

    void MoveTowardsPlayer()
    {
        if (isMovingTowardsPlayer)
        {
            // Calculate the direction towards the player
            Vector3 direction = (player.transform.position - transform.position).normalized;

            // Move the enemy towards the player
            transform.position += moveSpeed * Time.deltaTime * direction;
        }
    }

    void UpdateLineRenderer()
    {
        if (player != null && bow != null && lineRenderer != null)
        {
            // Set positions for Line Renderer (bow position to player position)
            lineRenderer.SetPosition(0, bow.transform.position);
            lineRenderer.SetPosition(1, player.transform.position);
        }
    }

    public void Fire()
    {
        isFiring = true;
        //yield return new WaitForSeconds(1f);
        GameObject a = Instantiate(arrow, firePos.transform.position, Quaternion.identity);
        a.transform.parent = null;
        a.transform.localScale = new Vector3(1, 1, 1);
        a.transform.position = transform.position;
        a.transform.LookAt(player.transform.position);
        isFiring = false;
        //bow.transform.LookAt(player.transform.position);
        Destroy(a.gameObject, 2.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(9f);
        }
    }


}
