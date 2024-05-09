using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Reflect the ball's velocity when it collides with a wall
        if (collision.gameObject.CompareTag("Border") || collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 reflection = Vector2.Reflect(GetComponent<Rigidbody2D>().velocity, collision.contacts[0].normal);
            GetComponent<Rigidbody2D>().velocity = reflection;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
