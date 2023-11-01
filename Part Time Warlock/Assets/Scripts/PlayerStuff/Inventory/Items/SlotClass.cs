using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

///<summary>
/// A class that determines the logic for slots with stacking (e.g., player inventories and chests).
/// </summary>

public class SlotClass : ItemSlot
{
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
        itemID = item.ID;
        quantity = _quantity;
    }

    public SlotClass() //constructor to make an empty inventory slot
    {
        //default/dummy constructor
        Clear();
    }

    public void UpdateInventorySlot(ItemClass data, int amount) // updates slot directly
    {
        item = data;
        itemID = item.ID;
        quantity = amount;
    }

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
