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

    [SerializeField] public ItemClass item; //change to private after new inventory is done
    [SerializeField] public int quantity;  //change to private after new inventory is done

    public ItemClass itemRef => item;
    public int quantityRef => quantity;


    public SlotType slotType;

    public enum SlotType
    {
        item,
        spell,
        potion,
    }

    public SlotClass(ItemClass _item, int _quantity)
    {
        item = _item;
        quantity = _quantity;
    }

    public SlotClass()
    {
        //default/dummy constructor
        this.item = null;
        this.quantity = -1;
    }

    public SlotClass(SlotClass slot)
    {
        this.item = slot.item;
        this.quantity = slot.quantity;
    }


    public void Clear()
    {
        this.item = null;
        this.quantity = 0;
    }

    public void UpdateInventorySlot(ItemClass data, int amount)
    {
        item = data;
        quantity = amount;
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
    public void AddItem(ItemClass item, int quantity) 
    {
        this.item = item;
        this.quantity = quantity;
    }

    public bool RoomLeftInStack(int amountToAdd)
    {
        if (quantity + amountToAdd <= item.stackSize)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RoomLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = item.stackSize - quantity;
        return RoomLeftInStack(amountToAdd);
    }
}
