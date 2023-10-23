using System.Collections;
using System.Collections.Generic;
using Unity.MemoryProfiler.Editor.UI;
using UnityEngine;

[System.Serializable]

public class SlotClass
{
    /*The below fields are properties; Any class can get the values from the 
    slot class but only the SlotClass can set the values*/
    //[field: SerializeField] public ItemClass item { get; private set; } = null;
    //[field: SerializeField] public int quantity { get; private set; } = 0;

    [SerializeField] private ItemClass item; //reference to the item itself
    [SerializeField] private int quantity;  //reference to the amount of the item we have

    public ItemClass Item => item; // => is essentially a hypercondensed getter method. Is capitalized in name for outer classes to reference
    public int Quantity => quantity;


    public SlotType slotType;

    public enum SlotType
    {
        item,
        spell,
        potion,
    }

    public SlotClass(ItemClass _item, int _quantity) //constructor to make an occupied inventory slot
    {
        item = _item;
        quantity = _quantity;
    }

    public SlotClass() //constructor to make an empty inventory slot
    {
        //default/dummy constructor
        Clear();
    }


    /*public SlotClass(SlotClass slot) // deprecated constructor
    {
        this.item = slot.item;
        this.quantity = slot.quantity;
    }*/


    public void Clear() // Clears the slot
    {
        this.item = null;
        this.quantity = -1;
    }

    public void UpdateInventorySlot(ItemClass data, int amount) // updates slot directly
    {
        item = data;
        quantity = amount;
    }

    public void AssignItem(SlotClass invSlot) //Assigns an item to the slot
    {
        if (item == invSlot.item) // Does the slot contain the same item? Add to the stack if so.
        {
            AddQuantity(invSlot.quantity);
        }
        else // overwrite slot with the new item that we're trying to add
        {
            item = invSlot.item;
            quantity = 0;
            AddQuantity(invSlot.quantity);
        }
    }
    /*public ItemClass GetItem()
    {
        return item;
    }
    public int GetQuantity()
    {
        return quantity;
    }*/
    public void AddQuantity(int _quantity)
    {
        quantity += _quantity;
    }
    public void SubtractQuantity(int _quantity)
    {
        quantity -= _quantity;
        if (quantity <= 0)
        {
            Clear();
        }
    }
    //Note:AddItem is deprecated in NewInventorySystem.cs
    /*public void AddItem(ItemClass item, int quantity) 
    {
        this.item = item;
        this.quantity = quantity;
    }*/

    public bool EnoughRoomLeftInStack(int amountToAdd)
    {
        if (item == null || item != null && quantity + amountToAdd <= item.stackSize)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool EnoughRoomLeftInStack(int amountToAdd, out int amountRemaining) // would there be enough room in the stack for the amount we're trying to add?
    {
        amountRemaining = item.stackSize - quantity;
        return EnoughRoomLeftInStack(amountToAdd);
    }

    public bool SplitStack(out SlotClass splitStack)
    {
        if (Quantity <= 1) //is there enough to actually split; can't have half of an item. If not, return false
        {
            splitStack = null;
            return false;
        }
        int halfStack = Mathf.RoundToInt(quantity / 2); //Get half the stack
        SubtractQuantity(halfStack);

        splitStack = new SlotClass(item, halfStack); //Creates a copy of this slot with half the stack size
        return true;
    }
}
