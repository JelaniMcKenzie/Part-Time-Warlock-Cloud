using PixelCrushers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InventoryPlus
{
    public class InventorySaver : MonoBehaviour
    {
        public List<ItemSlot> savedItems = new List<ItemSlot>();
        public WizardPlayer P;
        public Scene scene;

        //read from player inventory and save it to this object
        //on loading another scene, refresh the player inventory with this inventory
        

        // Update is called once per frame
        void Update()
        {
            //calling findanyobjectbytype every frame is BAD code practice. CHANGE THIS
            P = FindAnyObjectByType<WizardPlayer>();
            if (SceneManager.GetActiveScene().name == "Apartment") 
            {
                ClearSavedInventory();
            }
        }

        public void ClearSavedInventory()
        {
            savedItems.Clear();
        }

        public void UpdateSavedInventory(Inventory inventory)
        {
            foreach (var item in inventory.inventoryItems)
            {
                savedItems.Add(item);
            }
        }

        public void LoadSavedInventory(Inventory inventory)
        {
            foreach (ItemSlot _item in savedItems)
            {
                //This code currently does not account for stackable items. change later
                inventory.AddInventory(_item.GetItemType(), 1, 100, true);
            }
        }

        
    }
}

