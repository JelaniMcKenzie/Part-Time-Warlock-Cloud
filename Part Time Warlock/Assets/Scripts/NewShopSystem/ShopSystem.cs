using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;


[System.Serializable]
public class ShopSystem
{
    [SerializeField] private List<ShopSlot> shopInventory;
    [SerializeField] private int availableGold;
    [SerializeField] private float buyMarkUp;
    [SerializeField] private float sellMarkUp;

    public List<ShopSlot> ShopInventory => shopInventory;
    public float BuyMarkUp => buyMarkUp;
    public float SellMarkUp => sellMarkUp;

    public int AvailableGold => availableGold;

    public ShopSystem(int _size, int _gold, float _buyMarkUp, float _sellMarkUp)
    {
        availableGold = _gold;
        buyMarkUp = _buyMarkUp;
        sellMarkUp = _sellMarkUp;

        SetShopSize(_size);
    }

    private void SetShopSize(int size)
    {
        shopInventory = new List<ShopSlot>(size);

        for (int i = 0; i < size; i++) 
        {
            shopInventory.Add(new ShopSlot()); //create a shop inventory and fill it with backend slots
        }
    }

    public void AddToShop(ItemClass data, int amount)
    {
        if (ContainsItem(data, out ShopSlot shopSlot)) {
            shopSlot.AddQuantity(amount);
            return;
        }

        var freeSlot = GetFreeSlot();
        freeSlot.AssignItem(data, amount);
    }

    private ShopSlot GetFreeSlot()
    {
        var freeSlot = shopInventory.FirstOrDefault(i => i.Item == null);
        if (freeSlot == null)
        {
            freeSlot = new ShopSlot();
            shopInventory.Add(freeSlot);
            
        }
        return freeSlot;
    }

    public bool ContainsItem(ItemClass itemtoAdd, out ShopSlot shopSlot) //do any of our slots have the item to add in therm?
    {
        shopSlot = shopInventory.Find(i => i.Item == itemtoAdd);
        return shopSlot != null; 
    }

    public void PurchaseItem(ItemClass item, int amount)
    {
        if (!ContainsItem(item, out ShopSlot slot))
        {
            return;
        }

        slot.SubtractQuantity(amount);
    }

    public void GainGold(int basketTotal)
    {
        availableGold += basketTotal;
    }

    public void SellItem(ItemClass key, int value, int price)
    {
        //Sell an item to the shop
        AddToShop(key, value);
        ReduceGold(price);
    }

    private void ReduceGold(int price)
    {
        availableGold -= price;
    }
}
