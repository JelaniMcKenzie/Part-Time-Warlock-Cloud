using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class NewInventorySystem
{
    [SerializeField] public List<SlotClass> inventorySlots;

    public List<SlotClass> InventorySlots => inventorySlots;

    public int inventorySize => InventorySlots.Count;

    public UnityAction<SlotClass> OnInventorySlotChanged;

    public NewInventorySystem(int size)
    {
        inventorySlots = new List<SlotClass>(size);

        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new SlotClass());
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
                if(slot.RoomLeftInStack(amountToAdd))
                {
                    slot.AddQuantity(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }
           
        }

        if (HasFreeSlot(out SlotClass freeSlot)) //Gets the first available slot
        {
            freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
            OnInventorySlotChanged?.Invoke(freeSlot);
            return true;
        }

        return false;
    }

    public bool ContainsItem(ItemClass itemtoAdd, out List<SlotClass> invSlot)
    {
        /*this code gets all the slots where the slot's itemData is equal to the item we're searching for, and then
        adds it to a List.*/
        invSlot = InventorySlots.Where(slot => slot.item == itemtoAdd).ToList();
        Debug.Log(invSlot.Count);
        return invSlot == null ? false : true; //if the invSlot is null, return false. Else return true.




        /*Note: System.Linq has a lot of useful functions. For example the one below gets the
        first inventory slot where the item's max stack size is greater than 5
        InventorySlots.First(slot => slot.item.stackSize > 5);
         */
    }

    public bool HasFreeSlot(out SlotClass freeSlot)
    {
        //Note: in System.Linq functions like FirstOrDefault, i is a parameter variable that references an item in the list.
        //therefore, it can be named anything. I just called it i.
        freeSlot = InventorySlots.FirstOrDefault(i => i.item == null);
        return freeSlot == null ? false : true; //if its found a free slot, return true. else return false
    }
}
