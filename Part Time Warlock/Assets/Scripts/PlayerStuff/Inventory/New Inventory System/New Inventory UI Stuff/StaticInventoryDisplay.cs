using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{

    [SerializeField] private NewInventoryHolder inventoryHolder;
    [SerializeField] private InventorySlot_UI[] slots;


    protected override void Start()
    {
        base.Start();
        if (inventoryHolder != null)
        {
            inventorySystem = inventoryHolder.InventorySystem;
            inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }
        else Debug.LogWarning($"No inventory assigned to {this.gameObject}");

        AssignSlot(inventorySystem);
    }
    public override void AssignSlot(NewInventorySystem invToDisplay)
    {
        slotDictionary = new Dictionary<InventorySlot_UI, SlotClass>();

        //the hotbar has 5 slots. Code checks if the backend has 10 slots
        //will this throw an error due to the size mismatch?
        //temporarialy make the inventory slots a size of 5 to solve this;
         
        if (slots.Length != inventorySystem.inventorySize)
        {
            Debug.Log($"inventory slots out of sync on {this.gameObject} ");
        }
        for (int i = 0; i < inventorySystem.inventorySize; i++) 
        {
            slotDictionary.Add(slots[i], inventorySystem.InventorySlots[i]);
            slots[i].InitializeSlot(inventorySystem.InventorySlots[i]);
        }
    }
}
