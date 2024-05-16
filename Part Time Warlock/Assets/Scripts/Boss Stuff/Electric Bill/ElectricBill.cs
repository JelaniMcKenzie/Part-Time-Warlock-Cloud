using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBill : GameEntity
{

    public Player player;
    public bool isFlipped = false;
    public bool canSlam = true;
    public bool isSlamming = false;
    public bool isSpawningBalls = false;

    public Animator animator;
    private UIManager uiManager;
    public AudioSource audioSource;
    public TriggerBossMusic tbm;

    public GameObject BigCoin;

    CameraShake cam;
    private GameObject[] walls;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        player = FindAnyObjectByType<Player>();
        cam = FindAnyObjectByType<CameraShake>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        uiManager = FindAnyObjectByType(typeof(UIManager)) as UIManager;
        uiManager.bossHealthBar.maxValue = health;
        uiManager.bossHealthBar.value = health;

        if (uiManager.bossHealthBar.isActiveAndEnabled)
        {
            uiManager.bossHealthBar.gameObject.SetActive(false);
        }
        else
        {
            uiManager.bossHealthBar.gameObject.SetActive(true);
        }

        tbm = FindAnyObjectByType<TriggerBossMusic>();
        audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        
        walls = GameObject.FindGameObjectsWithTag("Border");

    }

    private void OnEnable()
    {  
        // Start the coroutine once
        StartCoroutine(SlamChanceTimer());
    }
    // Update is called once per frame
    void Update()
    {
        // No need to continuously check in Update
        
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.transform.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        } 
        else if (transform.position.x < player.transform.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    public IEnumerator SlamChanceTimer()
    {
        while (true) // Keep running indefinitely
        {
            yield return new WaitForSeconds(3f);
            float spawnChance = Random.Range(0f, 100f);

            if (spawnChance <= 50f)
            {
                if (canSlam)
                {
                    ThunderSlam();
                    canSlam = false; // Disable slamming until the next timer cycle
                }
            }
            
        }
    }

    public void ThunderSlam()
    {
        // Implement your slam logic here
        // For now, let's just log that slam occurred
        //Debug.LogWarning("Thunder Slam!");
        StartCoroutine(SlamTime());

        // You might want to reset the slam chance timer here if needed
    }

    public IEnumerator SlamTime()
    {
        canMove = false;
        isSlamming = true;
        //wait for seconds of slam animation
        yield return null;
        //spawn thunderbolts
        isSlamming = false;
        canSlam = true;
        yield return new WaitForSeconds(1f);
        canMove = true;
    }

    public void ShakeCameraTrigger()
    {
        cam.ShakeCamera(3f, 0.75f);
    }

    public override void Die()
    {
        
        uiManager.isBossDead = true;
        uiManager.bossHealthBar.gameObject.SetActive(false);
        audioSource.clip = tbm.defaultMusic;
        audioSource.loop = true;
        audioSource.pitch = 1.28f;
        audioSource.Play();

        Instantiate(onDeath, transform.position, Quaternion.identity);
        SpawnCoin();   
        Destroy(this.gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerProjectiles>() != null)
        {
            TakeDamage(collision.GetComponent<PlayerProjectiles>().damage);
            uiManager.bossHealthBar.value = health;
            StartCoroutine(DamageFlash());

        }
        else
        {
            Debug.LogWarning("Damage is Null!");
        }

        if (collision.CompareTag("FireWall"))
        {
            base.Burn();
        }
    }

    public void SpawnCoin()
    {
        // Get the player's position
        Vector2 bossPosition = transform.position;

        // Iterate through a loop to attempt spawning each coin
        for (int i = 0; i < 50; i++)
        {
            // Calculate a random offset within a range to spawn further away from the player
            float xOffset = UnityEngine.Random.Range(-2f, 2f);
            float yOffset = UnityEngine.Random.Range(-2f, 2f);

            // Create a Vector2 with the random offset
            Vector2 offset = new Vector2(xOffset, yOffset);

            // Calculate the spawn position further away from the player
            Vector2 spawnPosition = bossPosition + offset;

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
            GameObject newCoin = Instantiate(BigCoin, spawnPosition, Quaternion.identity);

            // Get the Rigidbody2D component of the newly spawned coin
            Rigidbody2D coinRigidbody = newCoin.GetComponent<Rigidbody2D>();

            // Calculate the direction from the player to the spawned coin
            Vector2 impulseDirection = (spawnPosition - bossPosition).normalized;

            // Apply a circular impulse to the coin to make them spread out with physics
            coinRigidbody.AddForce(impulseDirection * UnityEngine.Random.Range(1f, 3f), ForceMode2D.Impulse);

        }
    }

    public IEnumerator DamageFlash()
    {
        sprite.color = new Color32(100, 0, 0, 255);
        yield return new WaitForSeconds(0.25f);
        sprite.color = new Color32(255, 255, 255, 255);
    }


}
