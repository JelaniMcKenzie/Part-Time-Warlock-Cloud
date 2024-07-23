using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REMLogic : MonoBehaviour
{
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType(typeof(Player)) as Player;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;

    }

    public float magnetForce = 50f; // Adjust this value to control the strength of the magnet

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Coin") || other.CompareTag("BigCoin"))
        {
            // Check if the object is affected by magnetism (has a Rigidbody2D component)
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Calculate the direction from the magnet to the object
                Vector2 direction = transform.position - other.transform.position;

                // Apply a force towards the magnet
                rb.AddForce(magnetForce * Time.deltaTime * direction.normalized);
            }
        }
        
    }
}
