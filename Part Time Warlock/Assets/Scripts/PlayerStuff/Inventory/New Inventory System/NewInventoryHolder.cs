using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]

public class NewInventoryHolder : MonoBehaviour
{
    //all new inventory scripts will inherit from this; chests, the player's inventory, etc.
    [SerializeField] private int inventorySize;
    [SerializeField] protected NewInventorySystem primaryInventorySystem;

    public NewInventorySystem PrimaryInventorySystem => primaryInventorySystem; //this is essentially the same as a getter method for other classes to reference this

    public static UnityAction<NewInventorySystem> OnDynamicInventoryDisplayRequested;

    protected virtual void Awake()
    {
        //automate the inventorySize to equal the amount of slots (e.g. with transform.childcount or something)
        primaryInventorySystem = new NewInventorySystem(inventorySize);
    }
}
