using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;
using System;
using UnityEngine.InputSystem;
using InventoryPlus;

public class Player : GameEntity
{
    public GameObject staffTip = null;
    public Inventory inventory;
    public ScriptableObject armor;
    [SerializeField] private InputReader inputReader;

    [Space(30)]

    //--------------------Script comm fields--------------------
    private UIManager uiManager = null;

    [Space(30)]

    [Header("bool variables")]

    public bool canCast = true;
    public float maxHealth = 1f;
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

        rb = GetComponent<Rigidbody2D>();
        canDash = true;
        activeScene = SceneManager.GetActiveScene();
        
        if (activeScene.name == "Apartment")
        {
            handParent.SetActive(false);
            canCast = false;
        } 
        else
        {
            handParent.SetActive(true);
            health = maxHealth;
            //healthBar.UpdateHealthBar();
        }
        canHit = true;
        uiManager = FindAnyObjectByType<UIManager>();
        SpriteRenderer s = GetComponent<SpriteRenderer>();
        material = s.material;

    }

    // Update is called once per frame
    void Update()
    {
        activeScene = SceneManager.GetActiveScene();
        
        if (activeScene.name == "RDG Test")
        {
            handParent.SetActive(true);
            canCast = true;
        }
        

        if (isDashing)
        {
            return;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

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
            Sprint();

            
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
                        StartCoroutine(Dash(moveInput, GetComponent<Rigidbody2D>()));
                    }
                    else
                    {
                        if (canCast == true) 
                        {
                            inventory.UseItem(inventory.hotbarUISlots[3]);
                            StartCoroutine(Dash(moveInput, GetComponent<Rigidbody2D>()));
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
    }

    private void FixedUpdate()
    {
        if (canMove == true)
        {
            moveInput.Normalize();
            //Using Time.fixedDeltaTime makes movement feel the same across all systems, regardless of framerate.
            //its good for optimization
            //This comes with the caveat of needing to make the moveSpeed a high value (e.g. 500)
            rb.velocity = new Vector2(moveInput.x * moveSpeed * Time.fixedDeltaTime,
                                      moveInput.y * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void Movement()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        //moveInput.Normalize();
        //rb.velocity = moveInput * moveSpeed;

    }

    public void EnableMovement(bool _enable)
    {
        if (!_enable) 
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
            if (coinNum > 0)
            {
                int subtractedCoins = UnityEngine.Random.Range(5, 10);
                coinNum -= subtractedCoins;
                uiManager.UpdateCoinText();


                //Spawn half of the coins that the player lost
                for (int i = 0; i < subtractedCoins / 2; i++)
                {
                    SpawnCoin();
                }

                /*Save this for loop for later; a "midas touch" item, staff, or potion
                 * could work well with this code
                for (int i = 0; i < subtractedCoins; i++)
                {
                    SpawnCoin();
                }
                */

            }
            else
            {
                uiManager.timer -= 30;
                uiManager.CoinText.text = ": 0";
            }
            //healthBar.UpdateHealthBar();
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
        Rigidbody2D coinRb = coinSpawnRef.GetComponent<Rigidbody2D>();

        // Apply a random force to the coin
        if (coinRb != null)
        {
            Vector2 force = new Vector2(UnityEngine.Random.Range(-maxForce, maxForce), UnityEngine.Random.Range(-maxForce, maxForce));
            coinRb.AddForce(force, ForceMode2D.Impulse);
        }
    }


    public IEnumerator Dash(Vector2 dashDirection, Rigidbody2D rb)
    {
        canDash = false;
        isDashing = true;

        // Perform dash logic here, for example, calculate velocity
        Vector2 dashVelocity = dashDirection.normalized * dashMoveSpeed;
        rb.velocity = dashVelocity;

        float startTime = Time.time;
        float endTime = startTime + dashDuration;

        while (Time.time < endTime)
        {
            yield return null;
        }

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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (canHit == true)
            {
                Damage();
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
}
