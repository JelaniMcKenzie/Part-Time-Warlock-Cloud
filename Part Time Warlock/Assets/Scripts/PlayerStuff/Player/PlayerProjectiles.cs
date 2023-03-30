using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectiles : MonoBehaviour
{
    // Start is called before the first frame update
    public Player P = null;
    public GameObject firewall = null;
    public AudioClip fireClip = null;


    public AudioClip spellSound;
    public float speed = 10f;

    private Rigidbody rb;

    Vector3 dir;
    //checks what direction the player is facing and fires there
    void Start()
    {
       rb = GetComponent<Rigidbody>();
       AudioSource.PlayClipAtPoint(spellSound, transform.position, 4);

       //2.5f refers to the amount of time before the gameobject gets destroyed
       P = FindObjectOfType<Player>();
       
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 2.5f);
        if (gameObject.tag == "Fire")
        {
            StartCoroutine(WaitForFire());
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Border"))
        {
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);
        }
    }

    public void FireWall()
    {
        Instantiate(firewall, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        
    }
        

    public IEnumerator WaitForFire()
    {
        yield return new WaitForSeconds(0.05f);
        FireWall();
    }
}
