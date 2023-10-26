using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInventoryHolder : NewInventoryHolder
{
    [SerializeField] protected int secondaryInventorySize;
    [SerializeField] protected NewInventorySystem secondaryInventorySystem;

    public NewInventorySystem SecondaryInventorySystem => secondaryInventorySystem;

    public static UnityAction<NewInventorySystem> OnPlayerBackpackDisplayRequested;


    protected override void Awake()
    {
        base.Awake();
        secondaryInventorySystem = new NewInventorySystem(secondaryInventorySize);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            OnPlayerBackpackDisplayRequested?.Invoke(secondaryInventorySystem);
        }
    }

    public bool AddToInventory(ItemClass item, int quantity)
    {
        //primaryInventorySystem is the hotbar. secondaryInventorySystem is the player backpack
        /*if (primaryInventorySystem.AddToInventory(item, quantity))
        {
            return true;
        }*/
        if(secondaryInventorySystem.AddToInventory(item, quantity)) 
        {
            return true;
        }
        return false;
    }
}
