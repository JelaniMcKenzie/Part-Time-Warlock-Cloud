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
        //check for that specfic chest's save data inventory and load it it
        if (data.playerInventory.invSystem != null)
        {
            this.primaryInventorySystem = data.playerInventory.invSystem;
            OnPlayerInventoryChanged?.Invoke();
            
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
