using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    float spawnTime;
    [SerializeField] public GameObject Enemy;

    public Animator Anim = null;
    public Player P = null;
    public int EnemyLimit = 30;
    public int EnemyCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        P = FindAnyObjectByType<Player>();
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
        GameObject K = Instantiate(Enemy, transform.position, Quaternion.identity);
        K.transform.parent = null;
    }
}
