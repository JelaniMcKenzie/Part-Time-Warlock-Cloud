using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(UniqueID))]
public class ItemPickup : MonoBehaviour
{
    public float pickupRadius;
    public ItemClass item;

    private CircleCollider2D myCollider;

    [SerializeField] private ItemPickupSaveData itemSaveData;
    private string id;

    private void Awake()
    {
        id = GetComponent<UniqueID>().ID;
        SaveLoad.OnLoadGame += LoadGame;
        itemSaveData = new ItemPickupSaveData(item, transform.position, transform.rotation);

        myCollider = GetComponent<CircleCollider2D>();
        myCollider.isTrigger = false;
        myCollider.radius = pickupRadius;
    }

    private void Start()
    {
        SaveGameManager.data.activeItems.Add(id, itemSaveData);
    }

    //Logic for if we want item pickups to be saved (no duplicating items)
    private void LoadGame(SaveData data)
    {
        if (data.collectedItems.Contains(id))
        {
            Destroy(this.gameObject);
        }   
    }

    private void OnDestroy()
    {
        if (SaveGameManager.data.activeItems.ContainsKey(id))
        {
            SaveGameManager.data.activeItems.Remove(id);
        }
        SaveLoad.OnLoadGame -= LoadGame;
    }

    //TODO: change the logic of slime chasing so that this code can be changed to OnTriggerEnter2D
    private void OnCollisionEnter2D(Collision2D other)
    {

        var inventory = other.transform.GetComponent<PlayerInventoryHolder>();
        //if the object we collided with doesn't have an inventory component, return 
        if (!inventory) return;

        if (inventory.AddToInventory(item, 1))
        {
            //Add to list of picked up items
            SaveGameManager.data.collectedItems.Add(id);
            //if item was successfully added to inventory, destroy this gameObject
            Destroy(this.gameObject);
        }
        else
        {
            myCollider.isTrigger = true; //pass through items if inventory is full
        }
    }
}

[System.Serializable]
//Save data if, for example, we want to save the position/rotation of a dropped item
public struct ItemPickupSaveData 
{
    public ItemClass item;
    public Vector2 position;
    public Quaternion rotation;

    public ItemPickupSaveData(ItemClass _item, Vector2 _position, Quaternion _rotation)
    {
        item = _item;
        position = _position;
        rotation = _rotation;
    }
}
