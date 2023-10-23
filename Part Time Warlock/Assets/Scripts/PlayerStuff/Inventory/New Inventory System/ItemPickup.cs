using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ItemPickup : MonoBehaviour
{
    public float pickupRadius;
    public ItemClass item;

    private CircleCollider2D myCollider;

    private void Awake()
    {
        myCollider = GetComponent<CircleCollider2D>();
        myCollider.isTrigger = false;
        myCollider.radius = pickupRadius;
    }

    //TODO: change the logic of slime chasing so that this code can be changed to OnTriggerEnter2D
    private void OnCollisionEnter2D(Collision2D other)
    {

        var inventory = other.transform.GetComponent<NewInventoryHolder>();
        //if the object we collided with doesn't have an inventory component, return 
        if (!inventory) return;

        if (inventory.InventorySystem.AddToInventory(item, 1))
        {
            //if item was successfully added to inventory, destroy this gameObject
            Destroy(this.gameObject);
        }
        else
        {
            myCollider.isTrigger = true; //pass through items if inventory is full
        }
    }
}
