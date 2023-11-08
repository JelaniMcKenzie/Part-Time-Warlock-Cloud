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
    public Transform staffArm;
    public GameObject staffTip = null;
    public Inventory inventory;
    [SerializeField] private InputReader inputReader;

    [Space(30)]

    //--------------------Script comm fields--------------------
    private UIManager uiManager = null;

    [Space(30)]

    [Header("bool variables")]
    public float maxHealth = 1f;
    public string scene;
    private bool canHit = true;
    public bool canMove = true;
    private bool isHit = false;
    public bool isGamePaused; //move this to a gamemanager script later
    public int coinNum = 0;

    public Scene activeScene;
    public GameObject handParent = null;

    public GameObject inventoryObj = null;

    public bool isInventoryOpen = false;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("Dash Settings")]
    [SerializeField] float dashmoveSpeed = 10f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float dashCooldown = 1f;
    bool isDashing;
    bool canDash = true;

    


    // Start is called before the first frame update

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        canDash = true;
        activeScene = SceneManager.GetActiveScene();
        if (activeScene.name == "Apartment")
        {
            handParent.SetActive(false);
        } else
        {
            handParent.SetActive(true);
            health = maxHealth;
            //healthBar.UpdateHealthBar();
        }
        canHit = true;
        uiManager = FindAnyObjectByType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (inputReader.inventoryOn == true )
        {
            canMove = false;
            
        }
        else
        {
            canMove = true;
        }


        if (canMove == true)
        {
            Movement();
            Sprint();
            //Use a spell or an item
            if (Input.GetMouseButtonDown(0))
            {
                inventory.UseItem(inventory.hotbarUISlots[0]);

            }
            else if (Input.GetMouseButtonDown(1))
            {
                inventory.UseItem(inventory.hotbarUISlots[1]);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                inventory.UseItem(inventory.hotbarUISlots[2]);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (canDash)
                {
                    StartCoroutine(Dash());
                }
                inventory.UseItem(inventory.hotbarUISlots[3]);

            } 
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                
            }

            
            for (int i = 0; i < inventory.inventoryItems.Count; i++)
            {
                if (inventory.inventoryItems[i].ItemType.GetItem() is SpellClass)
                {
                    //Downcast from ItemSlot to SpellClass to access SpellClass methods
                    SpellClass s = (SpellClass) inventory.inventoryItems[i].ItemType.GetItem();
                    s.UpdateCooldown();
                }
            }
            
            
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

        /*foreach (var spell in spells)
        {
            if (spell != null)
            {
                spell.UpdateCooldown();
            }
        }*/
    }


    private void Movement()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();
        rb.velocity = moveInput * moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canDash)
            {
                StartCoroutine(Dash());
            }
        }
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
            isHit = true;
            if (coinNum > 0)
            {
                coinNum -= UnityEngine.Random.Range(5, 10);
                uiManager.UpdateCoinText();
            }
            else
            {
                uiManager.timer -= 30;
                uiManager.CoinText.text = ": 0";
            }
            //healthBar.UpdateHealthBar();
            StartCoroutine(Invulnerable());
            if (health <= 0)
            {
                Destroy(this.gameObject);
                SceneManager.LoadScene(scene);
            }
        }

    }

    /**
     * TODO: Add a method that, when the player leaves the dungeon, adds
     * the int coinNum value of this script to the Gold value of the NewInventorySystem script
     */

    public IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector3(moveInput.x * dashmoveSpeed, moveInput.y * dashmoveSpeed, 0f);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

        //TODO: Sync the cooldown of the dash to the usage of the dash spell
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
            Damage();
        }

    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Damage();
        }

    }
}
