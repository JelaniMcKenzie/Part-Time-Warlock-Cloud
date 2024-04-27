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
    public Player player;
    public float fireInterval = 2f; 
    public float minDistance = 10f; //change this value to adjust the movement range

    private bool canFire = true;
    private bool canTP = true;
    public Animator animator;
    private BoxCollider2D boxCollider;

    //get component in parent?
    GungeonRoomManager roomManager;
    
    // Start is called before the first frame update
    void Start()
    {
       sprite = GetComponent<SpriteRenderer>();
       canMove = true;
       moveSpeed = 2f;
       player = FindAnyObjectByType<Player>();

       roomManager = GetComponentInParent<GungeonRoomManager>();
       animator = GetComponent<Animator>();
       boxCollider = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        AimAtPlayer();
        // Draw a line from the bow to the player
        Debug.DrawLine(bow.transform.position, player.transform.position, Color.red);

        // Check if it's time to fire
        if (canFire)
        {
            StartCoroutine(Fire());
            //get the specfic room with the gungeon room manager rather than the whole map
            //alternatively, set the marigin to a specific radius from the player
            
        }
        animator.GetCurrentAnimatorClipInfo(0);
    }

    public override void Die()
    {
        base.Die();
    }

    public void Shoot()
    {
        //yield return new WaitForSeconds(1f);
        bow.GetComponent<Animator>().SetTrigger("Shoot");
        GameObject a = Instantiate(arrow, firePos.transform.position, Quaternion.identity);
        a.transform.parent = null;
        a.transform.localScale = new Vector3(1, 1, 1);
        a.transform.position = transform.position;
        a.transform.LookAt(player.transform.position);
        //bow.transform.LookAt(player.transform.position);
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
        // Prevent firing multiple times simultaneously
        canFire = false;

        // Wait for the interval before firing
        yield return new WaitForSeconds(fireInterval);

        // Perform firing action
        // For example, instantiate a projectile at bow position and make it move towards the player
        Shoot();

        // Start the animation coroutine
        StartCoroutine(PlayAnimationAndWait());

        canFire = true;
    }

    private IEnumerator PlayAnimationAndWait()
    {
        // Play your animation here
        // For example:
        //shoot anim here (found with getcomponentinchild(animator)
        // Wait for the animation duration
        // Replace the placeholder value (2f) with the actual duration of your animation
        yield return new WaitForSeconds(1.375f);
        
        // After the animation is finished, start teleportation
        StartCoroutine(TeleportAfterDelay());

        // Set canTP to true if needed
        canTP = true;
    }

    private IEnumerator TeleportAfterDelay()
    {
        // Wait for a short delay after firing before teleporting
        boxCollider.enabled = false;
        sprite.color = Color.clear;
        animator.SetTrigger("Disappear");
        yield return new WaitForSeconds(0.75f); // Adjust the delay as needed, match the length of the animation

        // Perform teleportation action
        // For example, find a teleport spot and move there
        //animator.SetTrigger("Disappear");
        FindTeleportSpot(GetRandomTeleportPosition());

        // Set canTP to true if needed
        canTP = true;
    }

    public void FindTeleportSpot(Vector3 position)
    {
        if (roomManager != null)
        {
            // Check if the point is actually inside the collider as there may be holes in the floor, etc.
            // We also want to make sure that there is no other collider in the radius of 1

            if (!IsPointWithinCollider(roomManager.FloorCollider, position) ||
                    Physics2D.OverlapCircleAll(position, 0.5f).Any(x => !x.isTrigger))
            {
                Debug.LogError("Point is within another collider");
            }
            else
            {
                //set the new shadowarcher position
                //Make sure the position isn't getting z axis-ed, everything must have a z value of 0
                //also, make sure to put breakables and other environment objects in the "collideable" layer to make sure this doesn't happen
                //Debug.LogWarning("archer transform: " + transform.position);
                transform.position = position;
                boxCollider.enabled = true;
                sprite.color = Color.white;
                animator.SetTrigger("Appear");
                
                //Debug.LogWarning("new transform: " + position);
            }
        }
        else
        {
            Debug.LogError("Room Manager is null!");
        }
    }

    private Vector3 GetRandomTeleportPosition()
    {
        return RandomPointInBounds(roomManager.FloorCollider.bounds, 1f);
    }

    private void ShowBow()
    {
        bow.SetActive(true);
    }

    private void HideBow()
    {
        bow.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(9f);
        }
    }

    

    private static bool IsPointWithinCollider(Collider2D collider, Vector2 point)
    {
        return collider.OverlapPoint(point);
    }

    public static Vector3 RandomPointInBounds(Bounds bounds, float margin = 0)
    {
        return new Vector3(
            RandomRange(bounds.min.x + margin, bounds.max.x - margin),
            RandomRange(bounds.min.y + margin, bounds.max.y - margin),
            0//(bounds.min.z + margin, bounds.max.z - margin)
        );

    }

    private static float RandomRange(float min, float max)
    {
        return (UnityEngine.Random.Range(0.0f, 1.0f) * (max - min) + min);
    }


}
