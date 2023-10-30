using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]

public abstract class NewInventoryHolder : MonoBehaviour
{
    //all new inventory scripts will inherit from this; chests, the player's inventory, etc.
    [SerializeField] private int inventorySize;
    [SerializeField] protected NewInventorySystem primaryInventorySystem;
    [SerializeField] protected int offset = 10;

    public int Offset => offset;

    public NewInventorySystem PrimaryInventorySystem => primaryInventorySystem; //this is essentially the same as a getter method for other classes to reference this

    public static UnityAction<NewInventorySystem, int> OnDynamicInventoryDisplayRequested; //Inventory to display, amount to display by

    protected virtual void Awake()
    {
        //automate the inventorySize to equal the amount of slots (e.g. with transform.childcount or something)
        primaryInventorySystem = new NewInventorySystem(inventorySize);
    }

    protected abstract void LoadInventory(SaveData saveData);
}

[System.Serializable]
public struct InventorySaveData
{
    public NewInventorySystem invSystem;
    public Vector2 position;
    public Quaternion rotation;

    public InventorySaveData(NewInventorySystem _invSystem, Vector2 _position, Quaternion _rotation)
    {
        invSystem = _invSystem;
        position = _position;
        rotation = _rotation;
    }

    public InventorySaveData(NewInventorySystem _invSystem)
    {
        invSystem = _invSystem;
        position = Vector2.zero;
        rotation = Quaternion.identity;
    }
}
