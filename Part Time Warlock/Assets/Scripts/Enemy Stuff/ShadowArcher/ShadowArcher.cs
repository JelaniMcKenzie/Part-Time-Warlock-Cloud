using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowArcher : MonoBehaviour
{
    public GameObject Arrow = null;
    public static GameObject[] ShadowPosition;
    public GameObject Bow = null;
    public Player P = null;
    public SpawnManager SM = null;
    public ArrowRotate AR = null;
    public GameObject CoinPrefab = null;
    public GameObject PotionPrefab = null;
    [SerializeField] public GameObject ShadowArcherDeath = null;
    public float timer = 0;
    int waitingTime = 5;
    public bool canTP = true;
    public bool canShoot = false;
    public float Health = 5;
    public int BurnSeconds = 5;
    public bool isOnFire = false;
    [SerializeField] public AudioClip Death = null;
    // Start is called before the first frame update
    void Start()
    {
        P = FindObjectOfType<Player>();
        AR = FindObjectOfType<ArrowRotate>();
        SM = FindObjectOfType<SpawnManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        if (Health <= 0f)
        {
            AudioSource.PlayClipAtPoint(Death, transform.position, 4);
            Debug.Log("dead");
            EnemyDeath();
        }
        if (P.canMove == true)
        {
            //Appear
            //Shoot
            timer += Time.deltaTime;
            if (timer < 2)
            {
                GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<BoxCollider>().enabled = true;
                Bow.GetComponent<SpriteRenderer>().enabled = true;
            }
            else if (timer > 2)
            {
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<BoxCollider>().enabled = false;
                Bow.GetComponent<SpriteRenderer>().enabled = false;

            }

            if (timer > waitingTime)
            {
                if (canTP == true)
                {
                    Teleport();
                    Shoot();
                }
                
                
                //Bow Looks at player
                Vector3 dir = P.transform.position - Bow.transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Bow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                timer = 0;
            }
        }    
    }

    public void Teleport()
    {
        ShadowPosition = GameObject.FindGameObjectsWithTag("ShadowPosition");
        GameObject selectedObject = ShadowPosition[Random.Range(0, ShadowPosition.Length-1)];
        transform.position = selectedObject.transform.position;
        Debug.Log("moved");
        Bow.transform.position = selectedObject.transform.position;
        

    }

    public void EnemyDeath()
    {
        //SM.ShadowArcherCount--;
        //SM.EnemyCount--;
        Instantiate(ShadowArcherDeath, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Health--;
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
            GetComponent<SpriteRenderer>().color = new Color32(255, 218, 0, 255);
            isOnFire = true;
            StartCoroutine(Aflame());
            StartCoroutine(BurnTime());
        }
    }

    public IEnumerator DamageFlash()
    {
        GetComponent<SpriteRenderer>().color = new Color32(255, 10, 100, 255);
        yield return new WaitForSeconds(0.25f);
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }

    public IEnumerator Aflame()
    {
        for (int s = 0; s <= BurnSeconds; s++)
        {
            Health -= 0.5f;
            yield return new WaitForSeconds(1.5f);
        }
    }

    public IEnumerator BurnTime()
    {
        yield return new WaitForSeconds(BurnSeconds);
        isOnFire = false;
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }

    public IEnumerator Freeze()
    {
        canTP = false;
        GetComponent<SpriteRenderer>().color = new Color32(0, 150, 150, 255);
        yield return new WaitForSeconds(2.5f);
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        canTP = true;
    }

    public void Shoot()
    {
        //yield return new WaitForSeconds(1f);
        GameObject a = Instantiate(Arrow, Bow.transform.position, Quaternion.identity);
        a.transform.parent = null;
        a.transform.localScale = new Vector3(1, 1, 1);
        a.transform.position = transform.position;
        a.transform.LookAt(P.transform.position);
        Bow.transform.LookAt(P.transform.position);
        Destroy(a.gameObject, 2.5f);
    }


}
