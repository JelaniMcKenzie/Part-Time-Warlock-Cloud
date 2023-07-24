using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

public class Player : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    public float speed = 5.0f;
    public float pelletSpeed = 10.0f;
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
    public float health;
    public float maxHealth = 1f;
    public string scene;
    public bool canHit = true;
    public bool canMove = true;
    public bool isHit = false;
    public int coinNum = 0;

    public Scene activeScene;
    public GameObject handParent = null;

    //Spell Fields
    public bool canShoot = true;
    public GameObject inventoryObj = null;

    //arrow key fields

    public bool up = false;
    public bool down = false;
    public bool left = false;
    public bool right = false;

    public bool isShooting = false;
    public bool isInventoryOpen = false;

    public int ammo = 6;

    public Rigidbody rb;

    //Vector fields

    //Vector3 vel;
    private Vector3 moveInput;

    public Input inputKey = new Input();

    // Start is called before the first frame update

    void Start()
    {
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
            //manaBar.UpdateManaBar();
            health = maxHealth;
            //healthBar.UpdateHealthBar();
        }
        canHit = true;
        uiManager = FindAnyObjectByType<UIManager>();
        healthBar = FindAnyObjectByType<HealthBar>();
        //vel = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        if (canMove == true)
        {
            Movement();
            //Shoot();
        }


        if (isHit == true)
        {
            FlashTimer();
        }


        
        if (isInventoryOpen == false)
        {
            //Use a spell or an item
            if (Input.GetMouseButtonDown(0))
            {

                if (inventory.items[15].item.GetSpell() != null)
                {
                    //use spell 1
                    inventory.items[15].item.GetSpell().Use(this);
                    //Debug.Log("Cast " + inventory.items[15].item.name);
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (inventory.items[17].item.GetSpell() != null)
                {
                    //use spell 2
                    inventory.items[17].item.GetSpell().Use(this);
                    //Debug.Log("Cast " + inventory.items[17].item.name);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (inventory.items[18].item.GetSpell() != null)
                {
                    //use spell 3 (dash spell)
                    inventory.items[18].item.GetSpell().Use(this);
                    //Debug.Log("Cast " + inventory.items[18].item.name);
                }
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (inventory.items[16].item.GetSpell() != null)
                {
                    //use spell 4
                    inventory.items[16].item.GetSpell().Use(this);
                    //Debug.Log("Cast " + inventory.items[16].item.name);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                if (inventory.items[19].item != null)
                {
                    //use item slot
                    inventory.items[19].item.Use(this);
                    //Debug.Log("Used " + inventory.items[19].GetItem().name);
                }
            }
        }
        

        //Open and close inventory
        if (Input.GetKeyDown(KeyCode.Tab) && isInventoryOpen == false)
        {
            inventoryObj.SetActive(true);
            isInventoryOpen = true;
            canMove = false;
            //canShoot = false;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && isInventoryOpen == true)
        {
            inventoryObj.SetActive(false);
            isInventoryOpen = false;
            canMove = true;
            //canShoot = true;
        }

        for (int i = 15; i < inventory.items.Length - 1; i++)
        {
            if (inventory.items[i].item is SpellClass spell)
            {
                spell.UpdateCooldown();
            }
        }
    }

    private void Movement()
    {
        /*Vector3 targetVel = new Vector3();

        //-------------------------------------------WASD movement----------------------------------------
        //Sprint();

        if (Input.GetKey(KeyCode.A) == true)
        {
            targetVel += (Vector3.left * speed);
        }

        if (Input.GetKey(KeyCode.D) == true)
        {
            targetVel += (Vector3.right * speed);
        }

        if (Input.GetKey(KeyCode.W) == true)
        {
            targetVel += (Vector3.up * speed);
        }

        if (Input.GetKey(KeyCode.S) == true)
        {
            targetVel += (Vector3.down * speed);
        }

        //Normalizes the vector
        targetVel = Vector3.MoveTowards(new Vector3(), targetVel, speed);

        // the 30 adjusts for 30fps
        vel = Vector3.MoveTowards(vel, targetVel, speed * 30);

        GetComponent<Rigidbody>().velocity = vel; //gives the movement vector speed*/

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();
        rb.velocity = moveInput * speed;
    }

    public void Sprint()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 7.5f;
            Debug.Log(speed);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 5.0f;
        }
    }

    public void Damage()
    {
        if (canHit == true)
        {
            isHit = true;
            health -= 0.1f;
            healthBar.UpdateHealthBar();
            StartCoroutine(Invulnerable());
            if (health <= 0)
            {
                Destroy(this.gameObject);
                SceneManager.LoadScene(scene);
            }
        }
        
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
