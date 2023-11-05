using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;

[System.Serializable]
public class NewInventorySystem
{
    [SerializeField] public List<SlotClass> inventorySlots; // the list of inventory slots
    [SerializeField] private int _gold;

    public int Gold => _gold;

    public List<SlotClass> InventorySlots => inventorySlots;

    public int InventorySize => InventorySlots.Count;

    public UnityAction<SlotClass> OnInventorySlotChanged;

    public NewInventorySystem(int size) // Constructor that sets the amount of slots
    {
        _gold = 0;
        CreateInventory(size);

    }

    public NewInventorySystem(int size, int gold) 
    {
        _gold = gold;
        CreateInventory(size);

    }

    private void CreateInventory(int size)
    {
        inventorySlots = new List<SlotClass>(size);

        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new SlotClass()); //add # of empty slots based on size variable
        }
    }

    public bool AddToInventory(ItemClass itemToAdd, int amountToAdd)
    {
        //if the inventory contains x item, return a list (defined as invSlot) of all the slots in the inventory that have x item in it
        if (ContainsItem(itemToAdd, out List<SlotClass> invSlot)) //check whether item exists in inventory.
        {
            //check the entire list (invSlot) for a slot that has room left in it
            foreach (var slot in invSlot)
            {
                //does the slot have room left in the stack
                if (slot.EnoughRoomLeftInStack(amountToAdd))
                {
                    slot.AddQuantity(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }                
            }
        }

        if (HasFreeSlot(out SlotClass freeSlot)) //Gets the first available slot
        {
            if (freeSlot.EnoughRoomLeftInStack(amountToAdd))
            {
                freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
                OnInventorySlotChanged?.Invoke(freeSlot);
                return true;
            }
            //TODO Add implementation to only take what can fill the stack, and check for another free slot to put the remainder in.
        }
        return false;
    }

    public bool ContainsItem(ItemClass itemtoAdd, out List<SlotClass> invSlot) //do any of our slots have the item to add in therm?
    {
        /*this code gets all the slots where the slot's itemData is equal to the item we're searching for, and then
        adds it to a List.*/
        invSlot = InventorySlots.Where(slot => slot.Item == itemtoAdd).ToList();
        Debug.Log(invSlot.Count);
        return invSlot == null ? false : true; //if the invSlot list is null, return false. Else return true.

        /*Note: System.Linq has a lot of useful functions. For example the one below gets the
        first inventory slot where the item's max stack size is greater than 5
        InventorySlots.First(slot => slot.item.stackSize > 5);
         */
    }

    public bool HasFreeSlot(out SlotClass freeSlot)
    {
        //Note: in System.Linq functions like FirstOrDefault, i is a parameter variable that references an item in the list.
        //therefore, it can be named anything. I just called it i.
        freeSlot = InventorySlots.FirstOrDefault(i => i.Item == null); //get the first free slot
        return freeSlot == null ? false : true; //if its found a free slot, return true. else return false
    }

    public bool CheckInventoryRemaining(Dictionary<ItemClass, int> shoppingCart)
    {
        //clone the player's inventory to check its space
        var clonedSystem = new NewInventorySystem(this.InventorySize);

        for (int i = 0; i < InventorySize; i++)
        {
            clonedSystem.InventorySlots[i].AssignItem(this.inventorySlots[i].Item, 
                InventorySlots[i].Quantity);
        }

        //for each key-value pair in the shopping cart dictionary, add every item to the player's inventory one by one
        foreach (var kvp in shoppingCart)
        {
            for (int i = 0; i < kvp.Value; i++)
            {
                //if you failed to add an item to the cloned system, break of the loop
                if (!clonedSystem.AddToInventory(kvp.Key, 1)) 
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void SpendGold(int basketTotal)
    {
        _gold -= basketTotal;
    }

    public Dictionary<ItemClass, int> GetAllItemsHeld()
    {
        //an organized list of every item the player has, irrespective of split stacks
        var distinctItems = new Dictionary<ItemClass, int>();

        foreach (var item in inventorySlots)
        {
            if (item.Item == null)
            {
                continue; //skip to the next item in the loop
            }
            if (!distinctItems.ContainsKey(item.Item))
            {
                distinctItems.Add(item.Item, item.Quantity);
            }
            else
            {
                distinctItems[item.Item] += item.Quantity;
            }
        }

        return distinctItems;
    }

    public void GainGold(int price)
    {
        _gold += price;
    }

    public void RemoveItemsFromInventory(ItemClass item, int amount)
    {
        if (ContainsItem(item, out List<SlotClass> invSlot))
        {
            foreach (var slot in invSlot)
            {
                var quantity = slot.Quantity;

                if (quantity > amount)
                {
                    slot.SubtractQuantity(amount);
                    amount -= quantity;
                }
                else
                {
                    slot.SubtractQuantity(quantity);
                    amount -= quantity;
                }

                if (amount <= 0)
                {
                    amount = 0;
                }

                OnInventorySlotChanged?.Invoke(slot);
            }
        }
    }
}
