using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRotate : MonoBehaviour
{
    public Player P = null;
    [SerializeField] public AudioClip Arrow = null;
    // Start is called before the first frame update
    void Start()
    {
        P = FindAnyObjectByType<Player>();
        AudioSource.PlayClipAtPoint(Arrow, transform.position, 4);
        Vector3 dir = P.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.Rotate(0, 0, -90); //rotates the arrow towards the player;
        GetComponent<Rigidbody>().velocity = (Vector3.MoveTowards(transform.position, P.transform.position, 0.1f) - transform.position) * 200;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = transform.localEulerAngles;
        v.y = 0;
        transform.localEulerAngles = v;
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            P.Damage();
        }

        if (collision.gameObject.tag == "Border")
        {
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FireWall"))
        {
            Destroy(this.gameObject);
        }
    }


}
