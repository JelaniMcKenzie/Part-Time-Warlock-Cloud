using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointTracker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other collider is a point object
        if (other.CompareTag("Player"))
        {
            WizardPlayer p = other.GetComponent<WizardPlayer>();
            // Get the point of collision
            Vector2 collisionPoint = other.transform.position;

            // Convert the collision point to local space of the collider
           
            p.pitRespawnPos = collisionPoint;

            Debug.Log("Collision point in local space: " + collisionPoint);
        }
    }
}
