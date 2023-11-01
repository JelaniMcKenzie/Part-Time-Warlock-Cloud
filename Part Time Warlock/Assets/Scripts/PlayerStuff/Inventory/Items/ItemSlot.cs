using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSlot : ISerializationCallbackReceiver
{
    [NonSerialized] protected ItemClass item; //reference to the item itself
    [SerializeField] protected int itemID = -1;
    [SerializeField] protected int quantity;  //reference to the amount of the item we have

    public ItemClass Item => item; // => is essentially a hypercondensed getter method. Is capitalized in name for outer classes to reference
    public int Quantity => quantity;

    public void Clear() // Clears the slot
    {
        this.item = null;
        itemID = -1;
        this.quantity = -1;
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
            itemID = item.ID;
            quantity = 0;
            AddQuantity(invSlot.quantity);
        }
    }

    public void AssignItem(ItemClass data, int amount)
    {
        if (item == data)
        {
            AddQuantity(amount);
        }
        else
        {
            item = data;
            itemID = data.ID;
            quantity = 0;
            AddQuantity(amount);
        }
    }

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

    public void OnAfterDeserialize()
    {
       
    }

    public void OnBeforeSerialize()
    {
        if (itemID == -1)
        {
            return; //slot is empty
        }

        var db = Resources.Load<Database>("Database");
        item = db.GetItem(itemID);
    }

 
}
