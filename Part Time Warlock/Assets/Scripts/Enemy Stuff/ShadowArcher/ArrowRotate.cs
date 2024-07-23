using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRotate : MonoBehaviour
{
    public Player P = null;
    [SerializeField] public AudioClip Arrow = null;
    private float damage = 9f;
    // Start is called before the first frame update
    void Start()
    {
        P = FindAnyObjectByType<Player>();
        AudioSource.PlayClipAtPoint(Arrow, transform.position, 4);
        Vector3 dir = P.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        StartCoroutine(PassThrough());
        GetComponent<Rigidbody2D>().velocity = (Vector3.MoveTowards(transform.position, P.transform.position, 0.1f) - transform.position) * 200;
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
            P.TakeDamage(damage);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<GameEntity>().TakeDamage(damage);

        }

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FireWall"))
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator PassThrough() 
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        yield return new WaitForSeconds(0.2f);
        GetComponent<BoxCollider2D>().isTrigger = false;

    }


}
