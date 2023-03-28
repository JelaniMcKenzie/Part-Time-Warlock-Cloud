using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggro : MonoBehaviour
{
    public Enemy slimeEnemy = null;
    public Player P = null;
    public bool canMove;
    public float EnemyAggroSpeed = 4f;
    // Start is called before the first frame update
    void Start()
    {
        slimeEnemy = FindObjectOfType<Enemy>();
        P = FindObjectOfType<Player>();
    }


    // Update is called once per frame
    void Update()
    {
        if (canMove == true)
        {
            EnemyMovement();
        }
        else if (canMove == false)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
        
        if (slimeEnemy.EnemyHealth <= 0)
        {
            Destroy(gameObject, 1.5f);
        }
    }

    public void EnemyMovement()
    {
        Vector3 pos = transform.position;
        pos.z = 0f;
        transform.position = pos;
        var EnemyStep = EnemyAggroSpeed / 5;
        GetComponent<Rigidbody>().velocity = 5 * (Vector3.MoveTowards(transform.position, P.transform.position, EnemyStep) - transform.position);
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canMove = true;
            slimeEnemy.canMove = true;
        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ChaseTimer());
        }
    }
    public IEnumerator ChaseTimer()
    {
        yield return new WaitForSeconds(3f);
        canMove = false;
        slimeEnemy.canMove = false;
    }
}
