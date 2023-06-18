using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotClass
{
    [SerializeField] private ItemClass item;
    [SerializeField] private int quantity;


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
        this.item = slot.GetItem();
        this.quantity = slot.GetQuantity();
    }

    public void Clear()
    {
        this.item = null;
        this.quantity = 0;
    }

    public ItemClass GetItem()
    {
        return item;
    }
    public int GetQuantity()
    {
        return quantity;
    }
    public void AddQuantity(int _quantity)
    {
        quantity += _quantity;
    }
    public void SubtractQuantity(int _quantity)
    {
        quantity -= _quantity;
    }
    public void AddItem(ItemClass item, int quantity) 
    {
        this.item = item;
        this.quantity = quantity;
    }
}
