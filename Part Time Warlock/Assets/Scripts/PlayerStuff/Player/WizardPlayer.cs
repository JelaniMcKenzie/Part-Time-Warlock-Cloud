using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;
using System;
using UnityEngine.InputSystem;
using InventoryPlus;
using UnityEngine.Tilemaps;
using Nevelson.Topdown2DPitfall.Assets.Scripts.Utils;

public class WizardPlayer : GameEntity, IPitfallCheck, IPitfallObject
{
    public GameObject staffTip = null;
    public GameObject pitfallCollider = null;
    public Inventory inventory;
    public InventorySaver inventorySaver;
    public ScriptableObject armor;
    [SerializeField] private InputReader inputReader;

    [Space(30)]

    //--------------------Script comm fields--------------------
    public UIManager uiManager = null;

    [Space(30)]

    [Header("bool variables")]

    public bool controlsReversed = false;

    public bool canCast = true;
    public string scene;
    public bool canHit = true;
    public bool isGamePaused; //move this to a gamemanager script later
    public int coinNum = 0;
    public bool moveSpeedMultiplied = false;

    public Scene activeScene;
    public GameObject handParent;

    public GameObject inventoryObj = null;

    public bool isInventoryOpen = false;
    public Vector3 moveInput;

    private Vector3 defaultScale;
    public Vector3 dashStart;

    [Space(30)]

    [Header("Dash Settings")]
    public float dashMoveSpeed;
    public float dashDuration;
    public float dashCooldown;
    public bool canDash = true;
    public bool isDashing = false;

    [Space(15)]

    [Header("Coin Damage Spawn Settings")]
    [SerializeField] private GameObject coinSpawnRef;
    public float maxForce = 5f; // Maximum distance for the offset (how far the coins spread on hit)


    [Space(15)]

    [Header("Misc")]
    public Material material;
    public Vector3 pitRespawnPos;
    DamageVignette damageVignette;
    CameraShake camShake;




    // Start is called before the first frame update

    void Start()
    {
        defaultScale = transform.localScale;
        inventorySaver = FindAnyObjectByType<InventorySaver>();
        //rb = GetComponent<Rigidbody2D>();
        canDash = true;
        canHit = true;
        uiManager = FindAnyObjectByType<UIManager>();
        SpriteRenderer s = GetComponent<SpriteRenderer>();
        material = s.material;
        damageVignette = FindAnyObjectByType<DamageVignette>();
        camShake = FindAnyObjectByType<CameraShake>();
        //inventorySaver.LoadSavedInventory(inventoryObj.GetComponent<Inventory>());
    }


    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }

        //transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        /*if (inputReader.inventoryOn == true )
        {
            canMove = false;
            
        }
        else
        {
            canMove = true;
        }*/


        if (canMove == true)
        {
            Movement();
            //AimStaff();
            //Sprint();

            
            #region SpellCastingLogic


            //Use a spell or an item
            if (Input.GetMouseButtonDown(0) && canCast == true)
            {
                inventory.UseItem(inventory.hotbarUISlots[0]);

            }

            if (Input.GetMouseButtonDown(1) && canCast == true)
            {
                inventory.UseItem(inventory.hotbarUISlots[1]);
            }

            if (Input.GetKeyDown(KeyCode.E) && canCast == true)
            {
                inventory.UseItem(inventory.hotbarUISlots[2]);
            }

            if (Input.GetKeyDown(KeyCode.Space) && canDash)
            {
                StartCoroutine(Dash(moveInput, rb));
                if (inventory.GetInventorySlot(inventory.hotbarUISlots[3]).GetItemType() != null)
                {
                    SpellClass dashSpell = (SpellClass)inventory.GetInventorySlot(inventory.hotbarUISlots[3]).GetItemType();
                    if (Mathf.Abs(moveInput.x) != 0 || Mathf.Abs(moveInput.y) != 0)
                    {
                        if (!isDashing && dashSpell.currentCooldown > 0)
                        {
                            StartCoroutine(Dash(moveInput, rb));
                        }
                        else
                        {
                            if (canCast == true)
                            {
                                inventory.UseItem(inventory.hotbarUISlots[3]);
                                StartCoroutine(Dash(moveInput, rb));
                            }

                        }
                    }
                }   
            }
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                inventory.UseItem(inventory.hotbarUISlots[4]);
                inventory.DropItem(inventory.hotbarUISlots[4]);
            }
            
            

            
            for (int i = 0; i < inventory.inventoryItems.Count; i++)
            {
                if (inventory.inventoryItems[i].GetItemType() is SpellClass)
                {
                    //Downcast from ItemSlot to SpellClass to access SpellClass methods
                    SpellClass s = (SpellClass) inventory.inventoryItems[i].GetItemType();
                    s.UpdateCooldown();
                   
                }
            }
            #endregion

        }

        if (coinNum < 0)
        {
            coinNum = 0;
            uiManager.UpdateCoinText();
        }

        if (isHit == true)
        {
            FlashTimer();
        }

        //Pause Game
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0f)
        {
            uiManager.ResumeGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 1f)
        {
            uiManager.PauseGame();
        }
    }

    private void Movement()
    {

        if (controlsReversed == true)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal") * -1f;
            moveInput.y = Input.GetAxisRaw("Vertical") * -1f;

            moveInput.Normalize();
            rb.velocity = moveInput * moveSpeed;
        }
        else
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            moveInput.Normalize();
            rb.velocity = moveInput * moveSpeed;
        }

    }

    public void EnableMovement(bool _enable)
    {
        if (!_enable) 
        {
            rb.velocity = Vector2.zero;
            GetComponent<Animator>().SetBool("running", false);
        }
           
        canMove = _enable;
        
    }

    public void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = 7.5f;
            Debug.Log(moveSpeed);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 5.0f;
        }
    }


    public override void TakeDamage(float damage)
    {
        if (canHit == true)
        {
            StartCoroutine(damageVignette.TakeDamageEffect());
            camShake.ShakeCamera(0.5f, 0.5f);
            canMove = true;
            isHit = true;

            if (coinNum > 0) 
            {
                // Calculate the number of coins to spawn based on the damage
                int coinsToSpawn = Mathf.Max(Mathf.FloorToInt(damage / 2), 1); // Adjust the formula based on your preference

                // Ensure coinNum doesn't go below zero
                coinNum = Mathf.Max(coinNum - coinsToSpawn, 0);

                uiManager.UpdateCoinText();

                // Spawn coins
                for (int i = 0; i < coinsToSpawn; i++)
                {
                    SpawnCoin();
                }
            }
            else
            {
                uiManager.timer -= 30f;
                StartCoroutine(uiManager.ScaleTimerText());
            }
           

            StartCoroutine(Invulnerable());

            if (uiManager.timer <= 0)
            {
                StartCoroutine(TimeToDie());
            }
        }
    }


    private void SpawnCoin()
    {
        // Ensure there are coins to spawn
        if (coinNum <= 0)
            return;

        // Calculate the maximum number of coins to spawn based on available coins
        int maxSpawnedCoins = Mathf.Min(coinNum, 10); // Adjust the maximum spawn based on your preference

        // Get the player's position
        Vector2 playerPosition = transform.position;

        // Find all GameObjects with the "Wall" tag
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Border");

        // Iterate through a loop to attempt spawning each coin
        for (int i = 0; i < maxSpawnedCoins; i++)
        {
            // Calculate a random offset within a range to spawn further away from the player
            float xOffset = UnityEngine.Random.Range(-2f, 2f);
            float yOffset = UnityEngine.Random.Range(-2f, 2f);

            // Create a Vector2 with the random offset
            Vector2 offset = new Vector2(xOffset, yOffset);

            // Calculate the spawn position further away from the player
            Vector2 spawnPosition = playerPosition + offset;

            // Check if the spawn position is within any GameObjects with the "Wall" tag
            bool isCollidingWithWall = false;
            foreach (GameObject wall in walls)
            {
                Collider2D wallCollider = wall.GetComponent<Collider2D>();
                if (wallCollider != null && wallCollider.bounds.Contains(spawnPosition))
                {
                    isCollidingWithWall = true;
                    break;
                }
            }

            // If the spawn position is within a wall, skip this spawn attempt
            if (isCollidingWithWall)
                continue;

            // Instantiate a coin at the calculated spawn position
            GameObject newCoin = Instantiate(coinSpawnRef, spawnPosition, Quaternion.identity);

            // Get the Rigidbody2D component of the newly spawned coin
            Rigidbody2D coinRigidbody = newCoin.GetComponent<Rigidbody2D>();

            // Calculate the direction from the player to the spawned coin
            Vector2 impulseDirection = (spawnPosition - playerPosition).normalized;

            // Apply a circular impulse to the coin to make them spread out with physics
            coinRigidbody.AddForce(impulseDirection * UnityEngine.Random.Range(1f, 3f), ForceMode2D.Impulse);

            // Reduce the number of available coins after spawning each coin
            coinNum--;
        }
    }



    public IEnumerator Dash(Vector2 dashDirection, Rigidbody2D rb)
    {
        canDash = false;
        isDashing = true;

        Vector3 dashStart = transform.position;
        // Disable collision with the enemy layer
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
        pitfallCollider.SetActive(false);

        // Perform dash logic here, for example, calculate velocity
        Vector2 dashVelocity = dashDirection.normalized * dashMoveSpeed;
        rb.velocity = dashVelocity;

        float startTime = Time.time;
        float endTime = startTime + dashDuration;

        while (Time.time < endTime)
        {
            yield return null;
        }

        // Enable collision back with the enemy layer
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        pitfallCollider.SetActive(true);
        


        rb.velocity = Vector2.zero; // Stop the player after the dash
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }


    public IEnumerator Invulnerable()
    {
        canHit = false;
        // Disable collision with the enemy layer
        //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
        yield return new WaitForSeconds(2f);
        // Enable collision with the enemy layer
        //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        canHit = true;
        isHit = false;
    }

    public IEnumerator DamageFlash()
    {
        sprite.color = new Color32 (255, 255, 255, 0);
        yield return new WaitForSeconds(0.25f);
        sprite.color = new Color32(255, 255, 255, 255);
    }

    public IEnumerator TimeToDie()
    {
        Playeranimations pA = GetComponent<Playeranimations>();
        pA.anim.Play("Death");
        yield return new WaitForSeconds(1.2f);
        Destroy(this.gameObject);
        SceneManager.LoadScene(scene);
    }

    public void FlashTimer()
    {
        float timer = 2f;
        bool timerActive = true;
        if (timerActive == true)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                StartCoroutine(DamageFlash());
            }
        }

        else
        {
            timer = 0f;
            timerActive = false;
        }
    }

    public void PitfallActionsBefore()
    {
        canMove = false;
    }

    public void PitfallResultingAfter()
    {
        canMove = true;
        TakeDamage(2f);
    }

    public bool PitfallConditionCheck()
    {
        return true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (canHit == true)
            {
                TakeDamage(9f);
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (inventory.isActiveAndEnabled)
            {
                inputReader.inventoryOn = false;
                inventory.ShowInventory(inputReader.inventoryOn);
                EnableMovement(true);
            }
            if (canHit == true)
            {
                TakeDamage(2f);
            }
        }
    }

    public void SetCollisionStateDuringGeneration(bool enableCollisions)
    {
        // Get all Collider components attached to the GameObject
        Collider2D[] colliders = GetComponents<Collider2D>();

        // Loop through each collider and set its enabled state
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = enableCollisions;
        }
    }

    





    //Upon exiting the floor collider, you fall in the pit and take damage
    //record the point at which you fell, and set the vector -1 to place yourself
    //at the edge of the pit
    //or just check the tag of the pit on collision
    //in post processing, set everything on the collideable tilemap to isTrigger
    //then on trigger, fall in the pit
    //outline the first tile square of each floor on the "other" layer so shadow archers
    //can't teleport to them
}
