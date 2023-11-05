using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInventoryHolder : NewInventoryHolder
{
    [SerializeField] private List<SlotClass> loadout;

    private DynamicInventoryDisplay inventoryDisplay;

    public static UnityAction OnPlayerInventoryChanged;

    public static UnityAction<NewInventorySystem, int> OnPlayerInventoryDisplayRequested; //Inventory to display, amount to display by

    private void Start()
    {
        SaveGameManager.data.playerInventory = new InventorySaveData(primaryInventorySystem);
        SaveLoad.OnLoadGame += LoadInventory;

        if (this.gameObject.GetComponent<Player>() != null)
        {
            inventoryDisplay = new DynamicInventoryDisplay();
            Debug.Log("Found Player");
            for (int i = 0; i < loadout.Count; i++)
            {
                
                Debug.Log("Setting Slot Types");
                if (i < 4)
                {
                    primaryInventorySystem.InventorySlots[i].slotType = SlotClass.SlotType.spell;
                    primaryInventorySystem.InventorySlots[i].AssignItem(loadout[i]);
                    

                }
                if (i == 4)
                {
                    primaryInventorySystem.InventorySlots[i].slotType = SlotClass.SlotType.potion;
                    primaryInventorySystem.InventorySlots[i].AssignItem(loadout[i]);

                }

            }
        }
        
    }

    protected override void LoadInventory(SaveData data)
    {
        
        if (data.playerInventory.invSystem != null)
        {
            primaryInventorySystem = data.playerInventory.invSystem;
            OnPlayerInventoryChanged?.Invoke();

        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
