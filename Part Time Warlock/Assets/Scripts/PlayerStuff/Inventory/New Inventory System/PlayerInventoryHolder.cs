using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInventoryHolder : NewInventoryHolder
{
    
    
    public static UnityAction OnPlayerInventoryChanged;

    public static UnityAction<NewInventorySystem, int> OnPlayerInventoryDisplayRequested; //Inventory to display, amount to display by

    private void Start()
    {
        SaveGameManager.data.playerInventory = new InventorySaveData(primaryInventorySystem);
        SaveLoad.OnLoadGame += LoadInventory;
    }

    protected override void LoadInventory(SaveData data)
    {
        
        if (data.playerInventory.invSystem != null)
        {
            primaryInventorySystem = data.playerInventory.invSystem;
            OnPlayerInventoryChanged?.Invoke();

        }
        else
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            OnPlayerInventoryDisplayRequested?.Invoke(primaryInventorySystem, offset);
        }
    }

    public bool AddToInventory(ItemClass item, int quantity)
    {
        if (primaryInventorySystem.AddToInventory(item, quantity))
        {
            return true;
        }
        
        return false;
    }
}
