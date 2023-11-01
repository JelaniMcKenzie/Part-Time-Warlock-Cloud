using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ShopKeeper : MonoBehaviour, IInteractable
{
    /**
     * IMPORTANT TODO
     * Currently, the code requires one ShopSystem per object.
     * Create a UI where, based on a button click, loads the correct
     * ShopKeeper Object
     */
    [SerializeField] private ShopItemList _shopItemsHeld;

    [SerializeField] private ShopSystem _shopSystem;

    private ShopSaveData _shopSaveData;

    public static UnityAction<ShopSystem, PlayerInventoryHolder> OnShopWindowRequested;

    private string _id;

    private void Awake()
    {
         //the uniqueID of the shop for saving its inventory.
                                          //Note: do we need this?
        _shopSystem = new ShopSystem(_shopItemsHeld.Items.Count, _shopItemsHeld.MaxAllowedGold, 
            _shopItemsHeld.BuyMarkUp, _shopItemsHeld.SellMarkUp);

        foreach (var item in _shopItemsHeld.Items)
        {
            _shopSystem.AddToShop(item.item, item.amount);
        }

        _id = GetComponent<UniqueID>().ID;
        _shopSaveData = new ShopSaveData(_shopSystem);
    }

    private void Start()
    {
        //if the shop doesn't already contain a uniqueID, give it one
        if (!SaveGameManager.data.shopKeeperDictionary.ContainsKey(_id))
        {
            SaveGameManager.data.shopKeeperDictionary.Add(_id, _shopSaveData);
        }
    }

    private void OnEnable()
    {
        SaveLoad.OnLoadGame += LoadInventory;
    }

    private void LoadInventory(SaveData data)
    {
        //if the shopKeeperDictionary doesn't have a reference to the current shopKeeper, then return

        try
        {
            if (!data.shopKeeperDictionary.TryGetValue(_id, out ShopSaveData shopSaveData))
            {
                return;
            }

            _shopSaveData = shopSaveData;
            _shopSystem = _shopSaveData.ShopSystem;
            
        }
        catch
        {
            Debug.Log("failed for some reason");
        }

        

       
    }

    private void OnDisable()
    {
        SaveLoad.OnLoadGame -= LoadInventory;
    }

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        var playerInv = interactor.GetComponent<PlayerInventoryHolder>();

        if (playerInv != null)
        {
            OnShopWindowRequested?.Invoke(_shopSystem, playerInv);
            interactSuccessful = true;
        }
        else
        {
            interactSuccessful = false;
            Debug.LogError("Player Inventory not found");
        }
    }

    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }
}

[System.Serializable]
public class ShopSaveData
{
    public ShopSystem ShopSystem;

    public ShopSaveData(ShopSystem shopSystem)
    {
        ShopSystem = shopSystem;
    }
}
