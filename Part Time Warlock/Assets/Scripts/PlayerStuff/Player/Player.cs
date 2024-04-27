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

public class Player : GameEntity
{
    public GameObject staffTip = null;
    public Inventory inventory;
    public InventorySaver inventorySaver;
    public ScriptableObject armor;
    [SerializeField] private InputReader inputReader;
    private Coroutine shrinkingCoroutine;

    [Space(30)]

    //--------------------Script comm fields--------------------
    private UIManager uiManager = null;

    [Space(30)]

    [Header("bool variables")]

    public bool controlsReversed = false;

    public bool canCast = true;
    public string scene;
    public bool canHit = true;
    private bool isHit = false;
    public bool isGamePaused; //move this to a gamemanager script later
    public int coinNum = 0;
    public bool moveSpeedMultiplied = false;

    public Scene activeScene;
    public GameObject handParent;

    public GameObject inventoryObj = null;

    public bool isInventoryOpen = false;
    public Rigidbody2D rb;
    public Vector2 moveInput;

    [Space(30)]

    [Header("Dash Settings")]
    public float dashMoveSpeed;
    public float dashDuration;
    public float dashCooldown;
    public bool canDash = true;
    public bool isDashing = false;

    [Space(15)]

    [Header("Coin Damage Spawn Settings")]
    private int minCoinSpawn = 1;
    private int maxCoinSpawn = 5;
    [SerializeField] private GameObject coinSpawnRef;
    public float maxForce = 5f; // Maximum distance for the offset (how far the coins spread on hit)

    public Material material;



    // Start is called before the first frame update

    void Start()
    {
        inventorySaver = FindAnyObjectByType<InventorySaver>();
        rb = GetComponent<Rigidbody2D>();
        canDash = true;
        canHit = true;
        uiManager = FindAnyObjectByType<UIManager>();
        SpriteRenderer s = GetComponent<SpriteRenderer>();
        material = s.material;


        inventorySaver.LoadSavedInventory(inventoryObj.GetComponent<Inventory>());
        
    }

    private void Awake()
    {


        
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
                SpellClass dashSpell = (SpellClass) inventory.GetInventorySlot(inventory.hotbarUISlots[3]).GetItemType();

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
        if (Input.GetKeyDown(KeyCode.Escape) && canMove == false)
        {
            isGamePaused = false;
            canMove = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && canMove == true)
        {
            isGamePaused = true;
            canMove = false;
        }

        
        // Detect if player is over the pit and call the FallPlayer method
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
        if (hit.collider != null && hit.collider.CompareTag("Pit"))
        {
            Debug.LogWarning("ON PIT");
            Pit pit = hit.collider.GetComponent<Pit>();
            if (pit != null && isDashing == false)
            {
                pit.FallPlayer();
            }
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


    public void Damage()
    {
        if (canHit == true)
        {
            canMove = true;
            isHit = true;

            // Ensure coinNum doesn't go below zero
            if (coinNum > 0)
            {
                int maxSpawnedCoins = Mathf.Min(coinNum, 10); // Adjust the maximum spawn based on your preference

                int subtractedCoins = Mathf.Max(coinNum / 5, 1); // 1:1 spawn for lower numbers, proportional for higher

                coinNum = Mathf.Max(coinNum - subtractedCoins, 0);
                uiManager.UpdateCoinText();

                // Spawn coins
                for (int i = 0; i < maxSpawnedCoins; i++)
                {
                    SpawnCoin();
                }
            }
            else
            {
                uiManager.timer -= 30;
                uiManager.CoinText.text = ": 0";
            }

            StartCoroutine(Invulnerable());

            if (uiManager.timer <= 0)
            {
                Destroy(this.gameObject);
                SceneManager.LoadScene(scene);
            }
        }
    }


    private void SpawnCoin()
    {
        // Calculate a random offset within the specified range
        float xOffset = UnityEngine.Random.Range(-1f, 1f);
        float yOffset = UnityEngine.Random.Range(-1f, 1f);

        // Create a Vector2 with the random offset
        Vector2 offset = new Vector2(xOffset, yOffset);

        // Instantiate a coin at the player's position with the random offset
        Instantiate(coinSpawnRef, (Vector2)transform.position + offset, Quaternion.identity);

        // Get the Rigidbody2D component
        
        // Apply a random force to the coin
        if (coinSpawnRef.TryGetComponent<Rigidbody2D>(out var coinRb))
        {
            Vector2 force = new Vector2(UnityEngine.Random.Range(-maxForce, maxForce), UnityEngine.Random.Range(-maxForce, maxForce));
            coinRb.AddForce(force, ForceMode2D.Impulse);
        }
    }


    public IEnumerator Dash(Vector2 dashDirection, Rigidbody2D rb)
    {
        canDash = false;
        isDashing = true;

        // Create a layer mask excluding the enemy layer
        LayerMask originalMask = ~LayerMask.GetMask("Player");
        LayerMask noEnemyMask = ~LayerMask.GetMask("Enemy");

        // Disable collision with the enemy layer
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

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

        rb.velocity = Vector2.zero; // Stop the player after the dash
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }


    public IEnumerator Invulnerable()
    {
        canHit = false;
        yield return new WaitForSeconds(2f);
        canHit = true;
        isHit = false;
    }

    public IEnumerator DamageFlash()
    {
        GetComponent<SpriteRenderer>().color = new Color32 (255, 255, 255, 0);
        yield return new WaitForSeconds(0.25f);
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
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

    // Function to start the shrinking coroutine
    public void StartShrinking(float duration)
    {
        if (shrinkingCoroutine == null)
        {
            shrinkingCoroutine = StartCoroutine(ShrinkPlayer(duration));
        }
    }

    // Coroutine to gradually decrease the player's scale
    private IEnumerator ShrinkPlayer(float duration)
    {
        Vector3 initialScale = transform.localScale;
        float timer = 0f;

        while (timer < duration)
        {
            float scaleFactor = Mathf.Lerp(1f, 0f, timer / duration);
            transform.localScale = initialScale * scaleFactor;
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure the player's scale is set to zero
        transform.localScale = new Vector3 (0.001f, 0.001f, 0.001f);

        // Reset the shrinking coroutine reference
        shrinkingCoroutine = null;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (canHit == true)
            {
                Damage();
            }
            
        }

        if (collision.gameObject.tag == "Pit")
        {
            if (isDashing == false)
            {
                //falling anim, maybe lerp the player's scale down to 0
            }
        }

    }

    public void OnCollisionStay2D(Collision2D collision)
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
                Damage();
            }
        }

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pit"))
        {
            Debug.LogWarning("ON PIT");
            Pit pit = collision.GetComponent<Pit>();
            if (pit != null && isDashing == false)
            {
                pit.FallPlayer();
            }
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
