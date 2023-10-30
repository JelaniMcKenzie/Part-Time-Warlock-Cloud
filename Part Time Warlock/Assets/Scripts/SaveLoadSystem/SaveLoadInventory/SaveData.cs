using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveData
{
    public List<string> collectedItems;
    public SerializableDictionary<string, InventorySaveData> chestDictionary;
    public SerializableDictionary<string, ItemPickupSaveData> activeItems;
    public InventorySaveData playerInventory;
    
    
    public SaveData()
    {
        collectedItems = new List<string>();
        chestDictionary = new SerializableDictionary<string, InventorySaveData>();
        activeItems = new SerializableDictionary<string, ItemPickupSaveData>();
        playerInventory = new InventorySaveData();
    }
}
