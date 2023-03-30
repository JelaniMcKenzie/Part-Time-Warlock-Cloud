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

    //--------------------Spell projectiles---------------------
    [SerializeField] public GameObject staffTip = null;
    [SerializeField] public GameObject pellet = null;
    [SerializeField] public GameObject icePellet = null;
    [SerializeField] public GameObject firePellet = null;
    [SerializeField] public GameObject lightningPellet = null;

    //--------------------Script comm fields--------------------

    [SerializeField] public PowerUps powerUps = null;
    [SerializeField] public UIManager uiManager = null;
    [SerializeField] public HealthBar healthBar = null;
    [SerializeField] public ManaBar manaBar = null;

    //-------------------Audio fields (CHANGE LATER)------------
    [SerializeField] public AudioClip spellSound = null;
    [SerializeField] public AudioClip iceSound = null;
    [SerializeField] public AudioClip fireClip = null;



    public float health;
    public float maxHealth = 1f;
    public string scene;
    public bool canHit = true;
    public bool canMove = true;
    public bool isHit = false;
    public int currentTome;
    public int coinNum = 0;
    public float timeBetweenShots = 0.2f;
    private float shotCounter;


    //Spell Fields
    public bool tome = false;
    public bool canIce = false;
    public bool canFire = false;
    public bool canLightning = false;
    public bool canShoot = true;

    //arrow key fields

    public bool up = false;
    public bool down = false;
    public bool left = false;
    public bool right = false;

    public bool isShooting = false;

    public int ammo = 6;

    public Rigidbody rb;


    float fireTime = 0;

    //Vector fields

    //Vector3 vel;
    private Vector3 moveInput;


    // Start is called before the first frame update

    public float spellIce = 5;
    public float spellLightning = 2;
    public float maxIce;
    public float spellFire = 3;
    public float maxFire;

    public float mana;
    public float maxMana = 1f;
    void Start()
    {
        canHit = true;
        powerUps = FindObjectOfType<PowerUps>();
        uiManager = FindObjectOfType<UIManager>();
        manaBar = FindObjectOfType<ManaBar>();
        healthBar = FindObjectOfType<HealthBar>();
        //vel = new Vector3();
        canShoot = true;
        mana = 0f;
        manaBar.UpdateManaBar();
        health = maxHealth;
        healthBar.UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(ammo.ToString());
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        if (canMove == true)
        {
            Movement();
            Shoot();
        }


        if (isHit == true)
        {
            FlashTimer();
        }

        if (spellIce > 5)
        {
            spellIce = 5;
        }

        if (spellFire > 3)
        {
            spellFire = 3;
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

    private void Shoot()
    {

        if (Input.GetButtonDown("Fire1") && canShoot)
        {

            if (Time.realtimeSinceStartup > fireTime)
            {
                ShootLogic();
            }

        }

        if (Input.GetButton("Fire1") && canShoot)
        {
            shotCounter -= Time.deltaTime;

            if (shotCounter <= 0)
            {
                ShootLogic();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (Time.realtimeSinceStartup > fireTime)
            {
                if (canIce == true)
                {
                    GameObject k = Instantiate(icePellet, staffTip.transform.position, Quaternion.identity);
                    AudioSource.PlayClipAtPoint(iceSound, transform.position, 1);
                    //K.transform.position = Hand.transform.position;
                    Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    k.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    k.GetComponent<Rigidbody>().velocity = k.transform.right * 10f;
                    mana -= 0.2f;
                    spellIce--;
                    manaBar.UpdateManaBar();
                }
                else if (canFire == true)
                {
                    GameObject f = Instantiate(firePellet, staffTip.transform.position, Quaternion.identity);
                    //AudioSource.PlayClipAtPoint(FireClip, transform.position);
                    //F.transform.position = Hand.transform.position;
                    Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    f.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    f.GetComponent<Rigidbody>().velocity = f.transform.right * 10f;
                    mana -= 0.3333333333333333333f;
                    spellFire--;
                    manaBar.UpdateManaBar();
                }
                else if (canLightning == true)
                {
                    GameObject l = Instantiate(lightningPellet, staffTip.transform.position, Quaternion.identity);
                    //L.transform.position = Hand.transform.position;
                    Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    l.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    l.GetComponent<Rigidbody>().velocity = l.transform.right * 10f;
                    // change the 10 to make it slower if need be
                    mana -= 0.5f;
                    spellLightning--;
                    manaBar.UpdateManaBar();
                }
            }
            

            if (spellIce <= 0)
            {
                canIce = false;
            }

            if (spellFire <= 0f)
            {
                canFire = false;
            }


            if (spellLightning <= 0)
            {
                canLightning = false;
            }

            if (spellIce > 0)
            {
                canIce = true;  
            }

            if (spellFire > 0f)
            {
                canFire = true;
            }

            if (spellLightning > 0)
            {
                canLightning = true;
            }
        }
        

    }

    public void ShootLogic()
    {
        if (ammo > 0)
        {
            ammo--;
            
            GameObject K = Instantiate(pellet, staffTip.transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(spellSound, transform.position, 4);
            //K.transform.position = Hand.transform.position;
            Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(staffTip.transform.parent.parent.parent.transform.position);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            K.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            K.GetComponent<Rigidbody>().velocity = K.transform.right * 10f;
            shotCounter = timeBetweenShots;
        }
         else if (ammo <= 0)
        {
            StartCoroutine(Refill());
        }

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

    public IEnumerator Refill() {
        canShoot = false;
        yield return new WaitForSeconds(0.25f);
        ammo = 5;
        canShoot = true;
        
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
