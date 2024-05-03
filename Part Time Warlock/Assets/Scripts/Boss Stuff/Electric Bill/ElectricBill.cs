using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBill : GameEntity
{

    public Player player;
    public bool isFlipped = false;
    public bool canSlam = true;
    public bool isSlamming = false;

    public Animator animator;

    CameraShake cam;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Player>();
        cam = FindAnyObjectByType<CameraShake>();
        animator = GetComponent<Animator>();

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
        Debug.Log("THUNDERRRRR");
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
        base.Die();
    }

    
}
