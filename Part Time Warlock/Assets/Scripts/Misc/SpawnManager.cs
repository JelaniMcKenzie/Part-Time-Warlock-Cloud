using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    float spawnTime;
    [SerializeField] public GameObject Enemy;
    [SerializeField] public GameObject ShadowArcher = null;

    public Animator Anim = null;
    public Player P = null;
    public int ShadowArcherCount;
    public int EnemyLimit = 30;
    public int EnemyCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        P = FindObjectOfType<Player>();
        GetComponent<SpriteRenderer>().enabled = false;
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (P.canMove == true)
        {
            if (EnemyCount < EnemyLimit)
            {
                if (Vector3.Distance(P.transform.position, transform.position) <= 500f)
                {
                    if (Time.realtimeSinceStartup > (spawnTime - 1f))
                    {
                        GetComponent<SpriteRenderer>().enabled = true;
                        Anim.Play("MagicMissile");
                    }
                    if (Time.realtimeSinceStartup > spawnTime)
                    {
                        SpawnEnemy();
                        spawnTime = Time.realtimeSinceStartup + 15f;
                        GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
            }
            
        }
    }


    public void SpawnEnemy()
    {
       int spawnEnemy = Random.Range(0, 3);
        if (spawnEnemy == 0 || spawnEnemy == 1)
        {
            GameObject K = Instantiate(Enemy, transform.position, Quaternion.identity);
            K.transform.parent = null;
            
        }
        else if (spawnEnemy == 2) 
        {
            GameObject S = Instantiate(ShadowArcher, transform.position, Quaternion.identity);
            S.transform.parent = null;
            
        }

        if (ShadowArcherCount == 5)
        {
            spawnEnemy = Random.Range(0, 2);
        }
    }
}
