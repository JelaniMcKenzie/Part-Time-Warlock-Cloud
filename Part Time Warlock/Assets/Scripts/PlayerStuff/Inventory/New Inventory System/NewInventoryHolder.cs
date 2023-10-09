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
    [SerializeField] protected NewInventorySystem inventorySystem;

    public NewInventorySystem InventorySystem => inventorySystem; //this is essentially the same as a getter method for other classes to reference this

    public static UnityAction<NewInventorySystem> OnDynamicInventoryDisplayRequested;

    private void Awake()
    {
        inventorySystem = new NewInventorySystem(inventorySize);
    }
}
