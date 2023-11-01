using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ChestInventory : NewInventoryHolder , IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete {  get; set; }

    protected override void Awake()
    {
        base.Awake();
        SaveLoad.OnLoadGame += LoadInventory;
    }

    private void Start()
    {
        var ChestSaveData = new InventorySaveData(primaryInventorySystem, transform.position, transform.rotation);

        SaveGameManager.data.chestDictionary.Add(GetComponent<UniqueID>().ID, ChestSaveData);
    }

    protected override void LoadInventory(SaveData data)
    {
        //check for that specfic chest's save data inventory and load it it
        if (data.chestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out InventorySaveData chestData))
        {
            primaryInventorySystem = chestData.invSystem;
            transform.position = chestData.position;
            transform.rotation = chestData.rotation;
        }
    }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        OnDynamicInventoryDisplayRequested?.Invoke(PrimaryInventorySystem, 0);
        interactSuccessful = true;
    }

    public void EndInteraction()
    {
        
    }

    
}


