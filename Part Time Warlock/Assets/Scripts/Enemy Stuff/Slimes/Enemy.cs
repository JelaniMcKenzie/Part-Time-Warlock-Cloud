using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Player P = null;
    [SerializeField] public GameObject EnemyDeathAnim = null;
    public Animator Anim = null;
    public bool frozen = false;
    [SerializeField] public string EnemyID;
    public GameObject Arrow = null;
    public UIManager UI = null;
    public EnemyAggro EA = null;
    public SpawnManager SM = null;
    public ManaBar MB = null;
    public bool isOnFire = false;
    public float EnemyHealth = 3f;
    public float EnemySpeed = 4f;
    public int BurnSeconds = 5;
    public bool addAmmo;
    public bool canMove;

    [SerializeField] public AudioClip SlimeMove = null;
    // Start is called before the first frame update
    void Start()
    {
        canMove = false;
        P = FindObjectOfType<Player>();
        UI = FindObjectOfType<UIManager>();
        EA = FindObjectOfType<EnemyAggro>();
        SM = FindObjectOfType<SpawnManager>();
        MB = FindObjectOfType<ManaBar>();
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

        if (EnemyHealth <= 0f)
        {
            AudioSource.PlayClipAtPoint(SlimeMove, transform.position);
            EnemyDeath();
            //SM.EnemyCount--;
        }
    }

    public void EnemyMovement()
    {
        if (canMove == true)
        {
            switch (EnemyID)
            {
                case "Slime": {
                        Vector3 pos = transform.position;
                        pos.z = 0f;
                        transform.position = pos;
                        var EnemyStep = EnemySpeed / 5;
                        GetComponent<Rigidbody>().velocity = 5 * (Vector3.MoveTowards(transform.position, P.transform.position, EnemyStep) - transform.position);
                        break;
                    }
            }
            //StartCoroutine(SlimeSteps());
           
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            EnemyHealth--;
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

    /*public IEnumerator SlimeSteps()
    {
        AudioSource.PlayClipAtPoint(SlimeMove, transform.position, 4);
        yield return new WaitForSeconds(2f);
    }*/

    public void EnemyDeath()
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
        else if(P.currentTome == 2 && P.spellFire < 3)
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
        EnemySpeed = 0f;
        frozen = true;
        GetComponent<SpriteRenderer>().color = new Color32(0, 150, 150, 255);
        yield return new WaitForSeconds(2.5f);
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        EnemySpeed = 4f;
        frozen = false;
    }

    public IEnumerator DamageFlash()
    {
        GetComponent<SpriteRenderer>().color = new Color32(100, 0, 0, 255);
        yield return new WaitForSeconds(0.25f);
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }

    public IEnumerator Aflame()
    {
        for (int s = 0; s <= BurnSeconds; s++)
        {
            EnemyHealth -= 0.5f;
            yield return new WaitForSeconds(1.5f);
        } 
    }

    public IEnumerator BurnTime()
    {
        yield return new WaitForSeconds(BurnSeconds);
        isOnFire = false;
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }


}
