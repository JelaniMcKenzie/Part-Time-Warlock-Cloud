using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : GameEntity
{
    private Player P = null;
    [SerializeField] public GameObject EnemyDeathAnim = null;
    public Animator Anim = null;
    public bool frozen = false;
    public UIManager UI = null;
    public SpawnManager SM = null;
    public ManaBar MB = null;
    public bool isOnFire = false;
    public int burnSeconds = 5;
    public bool addAmmo;
    public bool canMove;

    [SerializeField] public AudioClip SlimeMove = null;
    // Start is called before the first frame update
    void Start()
    {
        health = 3f;
        canMove = false;
        P = FindAnyObjectByType<Player>();
        UI = FindAnyObjectByType<UIManager>();
        SM = FindAnyObjectByType<SpawnManager>();
        MB = FindAnyObjectByType<ManaBar>();
        //SM.EnemyCount++;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        if (canMove == true)
        {
            EnemyMovement();
        }

        else if (canMove == false)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }

        if (frozen == true)
        {
            GetComponent<Animator>().speed = 0;
        }
        else
        {
            GetComponent<Animator>().speed = 1;
        }

        if (isOnFire == true)
        {
            GetComponent<SpriteRenderer>().color = new Color32(222, 70, 97, 255);
        }

        if (health <= 0f)
        {
            AudioSource.PlayClipAtPoint(SlimeMove, transform.position);
            Die();
            //SM.EnemyCount--;
        }
    }

    public void EnemyMovement()
    {
        // Calculate the direction towards the player
        Vector3 direction = (P.transform.position - transform.position).normalized;

        // Move the enemy towards the player
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            health--;
            StartCoroutine(DamageFlash());

        }

        if (other.CompareTag("Ice"))
        {
            //Destroy GameObject
            //Instantiate "EnemyFreeze" object that's basically a mannequin with an ice block
            //Or instantaiate an opaque ice block on top of it that breaks after five seconds

            StartCoroutine(Freeze());

        }

        if (other.CompareTag("FireWall"))
        {
            isOnFire = true;
            StartCoroutine(Aflame());
            StartCoroutine(BurnTime());
        }

    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerProx"))
        {
            canMove = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerProx"))
        {
            StartCoroutine(ChaseTimer());
        }
    }

    public IEnumerator ChaseTimer()
    {
        yield return new WaitForSeconds(3f);
        canMove = false;
    }

    public IEnumerator SlimeSteps()
    {
        AudioSource.PlayClipAtPoint(SlimeMove, transform.position, 4);
        yield return new WaitForSeconds(2f);
    }

    public override void Die()
    {
        addAmmo = true;
        Instantiate(EnemyDeathAnim, transform.position, Quaternion.identity);
        if (P.currentTome == 1 && P.spellIce < 5)
        {
            if (P.canIce == false)
            {
                P.canIce = true;
            }
            if (addAmmo == true)
            {
                P.spellIce++;
                P.mana += 0.2f;
                MB.UpdateManaBar();
            }

            if (P.spellIce > 5)
            {
                addAmmo = false;
                P.spellIce = 5;
                P.mana = P.maxMana;
                MB.UpdateManaBar();
            }

        }
        else if (P.currentTome == 2 && P.spellFire < 3)
        {
            if (P.canFire == false)
            {
                P.canFire = true;
            }
            if (addAmmo == true)
            {
                P.spellFire++;
                P.mana += 0.3333333333333333f;
                MB.UpdateManaBar();
            }

            if (P.spellFire > 3)
            {
                addAmmo = false;
                P.spellFire = 3;
                P.mana = P.maxMana;
                MB.UpdateManaBar();
            }

        }
        Destroy(this.gameObject);
    }

    public IEnumerator Freeze()
    {
        moveSpeed = 0f;
        isFrozen = true;
        GetComponent<SpriteRenderer>().color = new Color32(0, 150, 150, 255);
        yield return new WaitForSeconds(2.5f);
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        moveSpeed = 4f;
        isFrozen = false;
    }

    public IEnumerator DamageFlash()
    {
        GetComponent<SpriteRenderer>().color = new Color32(100, 0, 0, 255);
        yield return new WaitForSeconds(0.25f);
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }

    public IEnumerator Aflame()
    {
        for (int s = 0; s <= burnSeconds; s++)
        {
            health -= 0.5f;
            yield return new WaitForSeconds(1.5f);
        }
    }

    public IEnumerator BurnTime()
    {
        yield return new WaitForSeconds(burnSeconds);
        isOnFire = false;
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }
}