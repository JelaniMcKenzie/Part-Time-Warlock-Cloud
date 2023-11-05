using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{

    [SerializeField] private NewInventoryHolder inventoryHolder;
    [SerializeField] protected InventorySlot_UI[] slots;

    protected virtual void OnEnable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged += RefreshStaticDisplay;
    }

    protected virtual void OnDisable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged -= RefreshStaticDisplay;

    }

    private void RefreshStaticDisplay()
    {
        if (inventoryHolder != null)
        {
            inventorySystem = inventoryHolder.PrimaryInventorySystem;
            inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }
        else Debug.LogWarning($"No inventory assigned to {this.gameObject}");

        AssignSlot(inventorySystem, 0);
    }

    protected override void Start()
    {
        base.Start();
        RefreshStaticDisplay();
    }
    public override void AssignSlot(NewInventorySystem invToDisplay, int offset)
    {
        slotDictionary = new Dictionary<InventorySlot_UI, SlotClass>();

        //the hotbar has 5 slots. Code checks if the backend has 10 slots
        //will this throw an error due to the size mismatch?
        //temporarialy make the inventory slots a size of 5 to solve this;

        //This is a static display so the slots count on the UI and the backend must match up. Else it will throw a warning
         
        for (int i = 0; i < inventoryHolder.Offset; i++) 
        {
            slotDictionary.Add(slots[i], inventorySystem.InventorySlots[i]);
            slots[i].InitializeSlot(inventorySystem.InventorySlots[i]); //initialize the UI slot with its counterpart on the backend
        }
    }
}
