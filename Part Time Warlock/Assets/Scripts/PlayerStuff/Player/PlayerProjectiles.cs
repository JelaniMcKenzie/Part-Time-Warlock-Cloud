using Edgar.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerProjectiles : DamageSpell
{
    // Start is called before the first frame update
    public GameObject Firewall = null;
    public AudioClip FireClip = null;

    //checks what direction the player is facing and fires there
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Destroy(this.gameObject, 2.5f);
        if (gameObject.tag == "Fire")
        {
            StartCoroutine(WaitForFire());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.CompareTag("Border") || other.gameObject.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);
        }
    }

    public void FireWall()
    {
        Instantiate(Firewall, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        
    }
        

    public IEnumerator WaitForFire()
    {
        yield return new WaitForSeconds(0.025f);
        FireWall();
    }
}
