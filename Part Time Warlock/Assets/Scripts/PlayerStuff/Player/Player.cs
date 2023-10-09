using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;
using System;

public class Player : GameEntity
{
    public Transform staffArm;


    [Header("Spell Projectiles")]

    //--------------------Spell projectiles---------------------
    [SerializeField] public GameObject staffTip = null;


    [Space(30)]


    
    //--------------------Script comm fields--------------------
    public UIManager uiManager = null;
    public HealthBar healthBar = null;
    public InventoryManager inventory;


    [Header("Audio Fields")]
    //-------------------Audio fields (CHANGE LATER)------------
    [SerializeField] public AudioClip spellSound = null;
    [SerializeField] public AudioClip iceSound = null;
    [SerializeField] public AudioClip fireClip = null;
    [SerializeField] public AudioClip lightningClip = null;


    [Space(30)]

    [Header("bool variables")]
    public float maxHealth = 1f;
    public string scene;
    public bool canHit = true;
    public bool canMove = true;
    public bool isHit = false;
    public int coinNum = 0;

    public Scene activeScene;
    public GameObject handParent = null;

    public bool canShoot = true;
    public GameObject inventoryObj = null;

    //arrow key fields

    public bool up = false;
    public bool down = false;
    public bool left = false;
    public bool right = false;
    public bool isInventoryOpen = false;
    public Rigidbody rb;
    private Vector3 moveInput;

    [Header("Dash Settings")]
    [SerializeField] float dashmoveSpeed = 10f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float dashCooldown = 1f;
    bool isDashing;
    bool canDash = true;


    // Start is called before the first frame update

    void Start()
    {
        canDash = true;
        canShoot = false;
        activeScene = SceneManager.GetActiveScene();
        if (activeScene.name == "Apartment")
        {
            handParent.SetActive(false);
            canShoot = false;
        } else
        {
            handParent.SetActive(true);
            canShoot= true;
            health = maxHealth;
            //healthBar.UpdateHealthBar();
        }
        canHit = true;
        uiManager = FindAnyObjectByType<UIManager>();
        healthBar = FindAnyObjectByType<HealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }
     
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        if (canMove == true)
        {
            Movement();
            Sprint();
        }


        if (isHit == true)
        {
            FlashTimer();
        }


        
        /*if (isInventoryOpen == false)
        {
            //Use a spell or an item
            if (Input.GetMouseButtonDown(0))
            {

                if (inventory.items[15].item.GetSpell() != null)
                {
                    //use spell 1
                    inventory.items[15].item.GetSpell().Use(this);
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (inventory.items[17].item.GetSpell() != null)
                {
                    //use spell 2
                    inventory.items[17].item.GetSpell().Use(this);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (canDash)
                {
                    StartCoroutine(Dash());
                }
                if (inventory.items[18].item.GetSpell() != null)
                {
                    //use spell 3 (dash spell)
                    inventory.items[18].item.GetSpell().Use(this);
                }
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (inventory.items[16].item.GetSpell() != null)
                {
                    //use spell 4
                    inventory.items[16].item.GetSpell().Use(this);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                if (inventory.items[19].item != null)
                {
                    //use item slot
                    inventory.items[19].item.Use(this);
                }
            }
        }
        

        //Open and close inventory
        if (Input.GetKeyDown(KeyCode.Tab) && isInventoryOpen == false)
        {
            inventoryObj.SetActive(true);
            isInventoryOpen = true;
            canMove = false;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && isInventoryOpen == true)
        {
            inventoryObj.SetActive(false);
            isInventoryOpen = false;
            canMove = true;
        }

        for (int i = 15; i < inventory.items.Length - 1; i++)
        {
            if (inventory.items[i].item is SpellClass spell)
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

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Damage();
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Damage();
        }
    }
}
