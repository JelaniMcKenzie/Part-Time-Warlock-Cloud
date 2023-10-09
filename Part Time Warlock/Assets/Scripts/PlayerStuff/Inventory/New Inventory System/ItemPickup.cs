using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ItemPickup : MonoBehaviour
{
    public float pickupRadius;
    public ItemClass item;

    private SphereCollider myCollider;

    private void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = false;
        myCollider.radius = pickupRadius;
    }

    private void OnCollisionEnter(Collision other)
    {

        var inventory = other.transform.GetComponent<NewInventoryHolder>();
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
