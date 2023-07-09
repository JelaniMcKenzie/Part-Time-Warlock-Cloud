using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotClass
{
    /*The below fields are properties; Any class can get the values from the 
    slot class but only the SlotClass can set the values*/
    [field: SerializeField] public ItemClass item { get; private set; } = null;

    [field: SerializeField] public int quantity { get; private set; } = 0;

    public SlotType slotType;

    public enum SlotType
    {
        item,
        spell,
        potion,
    }

    public SlotClass(ItemClass _item, int _quantity)
    {
        this.item = _item;
        this.quantity = _quantity;
    }

    public SlotClass()
    {
        //default/dummy constructor
        item = null;
        quantity = 0;
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
    public void AddItem(ItemClass item, int quantity) 
    {
        this.item = item;
        this.quantity = quantity;
    }
}
