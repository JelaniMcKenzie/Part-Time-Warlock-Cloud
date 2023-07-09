using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCraftingRecipe", menuName = "Crafting/Recipe")]
public class CraftingRecipeClass : ScriptableObject
{
    [Header("Crafting Recipe")]
    public SlotClass[] inputItems;
    public SlotClass outputItem;

    public bool CanCraft(InventoryManager inventory)
    {
        //check if we have space in our inventory to craft
        if (inventory.isInventoryFull())
        {
            return false;
        }
        for (int i = 0; i < inputItems.Length; i++) 
        { 
            if (!inventory.Contains(inputItems[i].item, inputItems[i].quantity))
            {
                return false; 
            }
        }


        //return if inventory has input items
        return true;
    }

    public void Craft(InventoryManager inventory)
    {
        //remove input items from inventory
        for (int i = 0; i < inputItems.Length; i++) 
        {
            inventory.Remove(inputItems[i].item, inputItems[i].quantity);
        }

        try
        {
            inventory.Add(outputItem.item, outputItem.quantity);
            Debug.Log("Crafted" + outputItem.item.itemName);
        }
        catch
        {
            Debug.Log("Failed to craft item");
        }
        //add output items to inventory
        
        //allows for the crafting of multiple output items from one recipe

    }
}
