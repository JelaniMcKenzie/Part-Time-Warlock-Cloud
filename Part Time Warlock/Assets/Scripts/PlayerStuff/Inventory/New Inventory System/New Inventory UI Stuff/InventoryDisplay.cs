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

    public abstract void AssignSlot(NewInventorySystem invToDisplay, int offset); //Implemented in child classes

    public virtual void UpdateSlot(SlotClass updatedSlot) //called whenever something changes in the inventory, pass in the slot that was changed
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
        //TODO: either change the hardcoded key or utilize the new input system to change the logic here
        //The new input system requires an inputsystem gameobject to be active in the scene
        bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed;

        // if clicked slot has item and mouse doesn't have item, pick up the item
        if (clickedUISlot.AssignedInventorySlot.Item != null && mouseInventoryItem.AssignedMouseInvSlot.Item == null)
        {
            //If player is holding shift, split the stack in half
            if (isShiftPressed && clickedUISlot.AssignedInventorySlot.SplitStack(out SlotClass halfStackSlot))
            {
                mouseInventoryItem.UpdateMouseSlot(halfStackSlot);
                clickedUISlot.UpdateUISlot();
                return;
            } 
            else //pick up item in the clicked slot
            {
                mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
                clickedUISlot.ClearSlot(); //clear the hotbar/inventory slot because the item is picked up by the mouse
                return;
            }
           
        }

        // clicked slot doesn't have item and mouse has an item, place the mouse item in the empty slot
        if (clickedUISlot.AssignedInventorySlot.Item == null && mouseInventoryItem.AssignedMouseInvSlot.Item != null)
        {
            clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedMouseInvSlot);
            clickedUISlot.UpdateUISlot();

            mouseInventoryItem.ClearSlot(); //the selected item has been placed into an inventory slot
            return;
        }

        // if clicked slot and mouse have an item, decide what to do
        if (clickedUISlot.AssignedInventorySlot.Item != null && mouseInventoryItem.AssignedMouseInvSlot.Item != null)
        {
            bool isSameItem = clickedUISlot.AssignedInventorySlot.Item == mouseInventoryItem.AssignedMouseInvSlot.Item;

            //if both items are the same, combine the stack
            if (isSameItem && clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(mouseInventoryItem.AssignedMouseInvSlot.Quantity))
            {
                clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedMouseInvSlot); //add to the stack
                clickedUISlot.UpdateUISlot();

                mouseInventoryItem.ClearSlot(); //stacks are combined; clear what's on the mouse
                return;
            }

            //if there isn't room left in stack, keep the remaining items in the mouse
            else if (isSameItem && 
                !clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(mouseInventoryItem.AssignedMouseInvSlot.Quantity, out int leftInStack))
            {
                if (leftInStack < 1)
                {
                    SwapSlots(clickedUISlot); //Stack is full so swap the items
                }
                else // slot is not at max, so take whats needed from the mouse inventory
                {
                    int remainingOnMouse = mouseInventoryItem.AssignedMouseInvSlot.Quantity - leftInStack; //how much would be left on the mouse when you fill the clicked slot stack
                    
                    //actually fill the clicked slot stack
                    clickedUISlot.AssignedInventorySlot.AddQuantity(leftInStack);
                    clickedUISlot.UpdateUISlot();

                    //make the mouse item have the new (remainder) amount of the item
                    var newItem = new SlotClass(mouseInventoryItem.AssignedMouseInvSlot.Item, remainingOnMouse);
                    mouseInventoryItem.ClearSlot();
                    mouseInventoryItem.UpdateMouseSlot(newItem);
                    return;
                }
            }
            
            //if the items are different, swap the slot item with the mouse item
            else if (!isSameItem)
            {
                SwapSlots(clickedUISlot);
                return;
            }
        }
    }

    private void SwapSlots(InventorySlot_UI clickedUISlot)
    {
        //we have an item on the mouse and make a copy of it to be overwritten
        var clonedSlot = new SlotClass(mouseInventoryItem.AssignedMouseInvSlot.Item, mouseInventoryItem.AssignedMouseInvSlot.Quantity);

        //clear the mouse slot
        mouseInventoryItem.ClearSlot();

        //put the item we clicked on in the inventory On the mouse
        mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);

        clickedUISlot.ClearSlot();

        //Update the UI with the item we cloned. (We cloned the item to save a reference to it for this purpose, because the original was deleted in clickedUISlot.ClearSlot());
        clickedUISlot.AssignedInventorySlot.AssignItem(clonedSlot);
        clickedUISlot.UpdateUISlot();
    }
}
