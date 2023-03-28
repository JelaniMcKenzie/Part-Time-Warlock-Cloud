using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowRange : MonoBehaviour
{
    // Start is called before the first frame update
    public ShadowArcher Shadow = null;
    public Player P = null;
    void Start()
    {
        Shadow = FindObjectOfType<ShadowArcher>();
        P = FindObjectOfType<Player>();
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Shadow.transform.position;

        if (Shadow.timer < 2)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<SphereCollider>().enabled = true;
        }
        else if (Shadow.timer > 2)
        {
            GetComponent<SphereCollider>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerProx"))
        {
            Shadow.canShoot = true;
        }
    }


    public void OnTriggerExit(Collider other)
    {
        IEnumerator StopShooting()
        {
            yield return new WaitForSeconds(5f);
            if (other.CompareTag("PlayerProx"))
            {
                Shadow.canShoot = false;
            }
        }
        StartCoroutine(StopShooting());
    }

   
}
