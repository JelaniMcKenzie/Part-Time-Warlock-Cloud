using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] MouseItemData mouseInventoryItem;
    protected NewInventorySystem inventorySystem;

    //Similar to a HashSet; pairs the slot in the UI to its slot in the backend
    //The UI slot is the "key" and the backend Slot is the "value" in the key-value pair
    protected Dictionary<InventorySlot_UI, SlotClass> slotDictionary; 

    public NewInventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, SlotClass> SlotDictionary => slotDictionary;

    protected virtual void Start()
    {
        
    }

    public abstract void AssignSlot(NewInventorySystem invToDisplay);

    protected virtual void UpdateSlot(SlotClass updatedSlot)
    {
        foreach (var slot in SlotDictionary)
        {
            if (slot.Value == updatedSlot) //Slot value - the "under the hood" inventory slot.
            {
                slot.Key.UpdateUISlot(updatedSlot); //Slot key - the UI representation of the value
            }
        }
    }

    public void SlotClicked(InventorySlot_UI clickedUISlot)
    {
        

        // if clicked slot has item and mouse doesn't have item, pick up the item
        if (clickedUISlot.AssignedInventorySlot.Item != null && mouseInventoryItem.AssignedMouseInvSlot.Item == null)
        {
            //If player is holding shift, split the stack in half
            mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
            clickedUISlot.ClearSlot(); //clear the hotbar/inventory slot because the item is picked up by the mouse
            return;
        }

        // clicked slot doesn't have item and mouse has an item, place the mouse item in the empty slot
        if (clickedUISlot.AssignedInventorySlot.Item == null && mouseInventoryItem.AssignedMouseInvSlot.Item != null)
        {
            clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedMouseInvSlot);
            clickedUISlot.UpdateUISlot();

            mouseInventoryItem.ClearSlot(); //the selected item has been placed into an inventory slot
        }

        // if clicked slot and mouse have an item, decide what to do
            //if both items are the same, combine the stack
        //if the slot stack size + mouse stack size > the slot max stack size? If so, take only enough from the mouse to fill the stack
        //if the items are different, swap the slot item with the mouse item
        
        
    }

    private void SwapSlots(SlotClass clickedSlot)
    {

    }
}
