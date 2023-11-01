using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopKeeperDisplay : MonoBehaviour
{
    [SerializeField] private ShopSlotUI shopSlotPrefab;
    [SerializeField] private ShoppingCartItemUI shoppingCartItemPrefab;

    [SerializeField] private Button buyTab;
    [SerializeField] private Button sellTab;

    [Header("Shopping Cart")]
    [SerializeField] private TextMeshProUGUI basketTotalText;
    [SerializeField] private TextMeshProUGUI playerGoldText;
    [SerializeField] private TextMeshProUGUI shopGoldText;
    [SerializeField] private Button useCartButton;
    [SerializeField] private TextMeshProUGUI useCartButtonText;

    [Header("Item Preview Section")]
    [SerializeField] private Image itemPreviewSprite;
    [SerializeField] private TextMeshProUGUI itemPreviewName;
    [SerializeField] private TextMeshProUGUI itemPreviewDescription;

    [SerializeField] private GameObject itemListContentPanel;
    [SerializeField] private GameObject shoppingCartContentPanel;

    [SerializeField] private int basketTotal; //how much gold the cart will cost
    private bool _isSelling;

    private ShopSystem shopSystem;
    private PlayerInventoryHolder playerInventoryHolder;

    private Dictionary<ItemClass, int> shoppingCart = new Dictionary<ItemClass, int>(); //Backend version of the shopping cart
    private Dictionary<ItemClass, ShoppingCartItemUI> shoppingCartUI = new Dictionary<ItemClass, ShoppingCartItemUI>(); //Frontend/UI version of the shopping cart

    public void DisplayShopWindow(ShopSystem _shopSystem, PlayerInventoryHolder _playerInventoryHolder)
    {
        shopSystem = _shopSystem;
        playerInventoryHolder = _playerInventoryHolder;
        RefreshDisplay();
    }

    private void RefreshDisplay()
    {

        if (useCartButton != null)
        {
            //if you're on the sell tab, change the text to sell items. Else, change to buy items
            useCartButtonText.text = _isSelling ? "Sell Items" : "Buy Items";
            useCartButton.onClick.RemoveAllListeners();
            if (_isSelling )
            {
                useCartButton.onClick.AddListener(SellItems);
            }
            else
            {
                useCartButton.onClick.AddListener(BuyItems);
            }
        }
        ClearSlots();
        ClearItemPreview();
        basketTotalText.enabled = false;
        useCartButton.gameObject.SetActive(false);
        basketTotal = 0;
        playerGoldText.text = $"Player Gold: {playerInventoryHolder.PrimaryInventorySystem.Gold}";
        shopGoldText.text = $"Shop Gold: {shopSystem.AvailableGold}";

        if (_isSelling)
        {
            DisplayPlayerInventory();
        }
        else
        {
            DisplayShopInventory();
        }
        
    }

    private void BuyItems()
    {
        if (playerInventoryHolder.PrimaryInventorySystem.Gold < basketTotal)
        {
            return; //useCartButton doesn't need to do anything
        }

        //if player doesn't have enough space for items in their inventory, return (they can't buy them)
        //idea: buy items directly to a chest rather than player inventory
        if (!playerInventoryHolder.PrimaryInventorySystem.CheckInventoryRemaining(shoppingCart))
        {
            return;
        }

        //kvp = key value pair. The item to buy and the amount we're buying
        foreach (var kvp in shoppingCart)
        {
            shopSystem.PurchaseItem(kvp.Key, kvp.Value);

            //add item to the player's inventory one by one
            for (int i = 0; i < kvp.Value; i++)
            {
                playerInventoryHolder.PrimaryInventorySystem.AddToInventory(kvp.Key, 1);
            }  
        }

        shopSystem.GainGold(basketTotal);
        playerInventoryHolder.PrimaryInventorySystem.SpendGold(basketTotal);

        RefreshDisplay(); //the item stock has changed. Refresh the UI to reflect that
    }

    private void SellItems()
    {
        if (shopSystem.AvailableGold < basketTotal)
        {
            return;
        }

        //if the shop has enough gold to buy all the player's items

        foreach(var kvp in shoppingCart)
        {
            var price = GetModifiedPrice(kvp.Key, kvp.Value, shopSystem.SellMarkUp);
            shopSystem.SellItem(kvp.Key, kvp.Value, price);

            playerInventoryHolder.PrimaryInventorySystem.GainGold(price);
            playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(kvp.Key, kvp.Value);

        }

        RefreshDisplay();
    }

    private void ClearSlots()
    {
        shoppingCart = new Dictionary<ItemClass, int>();
        shoppingCartUI = new Dictionary<ItemClass, ShoppingCartItemUI>();

        foreach (var item in itemListContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
        foreach (var item in shoppingCartContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
    }

    //find a way to display different shop inventories per tab. maybe with a method of some kind
    private void DisplayShopInventory()
    {
        foreach (var item in shopSystem.ShopInventory)
        {
            if (item.Item == null)
            {
                continue;
            }

            var slotShop = Instantiate(shopSlotPrefab, itemListContentPanel.transform);
            slotShop.Initialize(item, shopSystem.BuyMarkUp);
        }
    }

    private void DisplayPlayerInventory()
    {
        //loop through player inventory and display them on the shop list
        //TODO: loop through every item that isn't a spell
        foreach (var item in playerInventoryHolder.PrimaryInventorySystem.GetAllItemsHeld())
        {
            var tempSlot = new ShopSlot();
            tempSlot.AssignItem(item.Key, item.Value);

            var shopSlot = Instantiate(shopSlotPrefab, itemListContentPanel.transform);
            shopSlot.Initialize(tempSlot, shopSystem.SellMarkUp);
        }
    }

    public void AddItemToCart(ShopSlotUI shopSlotUI)
    {
        var data = shopSlotUI.AssignedItemSlot.Item;

        UpdateItemPreview(shopSlotUI);

        var price = GetModifiedPrice(data, 1, shopSlotUI.MarkUp);
        

        if (shoppingCart.ContainsKey(data))
        {
            shoppingCart[data]++;
            var newString = $"{data.itemName} ({price}G) x{shoppingCart[data]}";
            shoppingCartUI[data].SetItemText(newString);
            
        }
        else
        {
            shoppingCart.Add(data, 1);
            var shoppingCartTextObj = Instantiate(shoppingCartItemPrefab, shoppingCartContentPanel.transform);
            var newString = $"{data.itemName} ({price}G) x1";
            shoppingCartTextObj.SetItemText(newString);
            shoppingCartUI.Add(data, shoppingCartTextObj);
        }

        basketTotal += price;
        basketTotalText.text = $"Total: {basketTotal}G";

        if (basketTotal > 0 && !basketTotalText.IsActive())
        {
            Debug.Log("Activating buysell button");
            basketTotalText.enabled = true;
            useCartButton.gameObject.SetActive(true);
        }

        CheckCartVsAvailableGold();

    }

    public void RemoveItemFromCart(ShopSlotUI shopSlotUI)
    {
        var data = shopSlotUI.AssignedItemSlot.Item;
        var price = GetModifiedPrice(data, 1, shopSlotUI.MarkUp);

        if (shoppingCart.ContainsKey(data))
        {
            shoppingCart[data]--;
            var newString = $"{data.itemName} ({price}G) x{shoppingCart[data]}";
            shoppingCartUI[data].SetItemText(newString);

            if (shoppingCart[data] <= 0)
            {
                shoppingCart.Remove(data);
                var tempObj = shoppingCartUI[data].gameObject;
                shoppingCartUI.Remove(data);
                Destroy(tempObj);
            }
        }

        basketTotal -= price;
        basketTotalText.text = $"Total: {basketTotal}G";

        if (basketTotal <= 0 && basketTotalText.IsActive()) 
        {
            basketTotalText.enabled = false;
            useCartButton.gameObject.SetActive(false);
            ClearItemPreview();
            return; //if there's nothing in the shopping cart, we don't have to check it against available gold
        }

        CheckCartVsAvailableGold();
    }

    private void ClearItemPreview()
    {
        itemPreviewSprite.sprite = null;
        itemPreviewSprite.color = Color.clear;
        itemPreviewName.text = "";
        itemPreviewDescription.text = "";
    }

    private void CheckCartVsAvailableGold()
    {
        //if you're selling, go by the shop's available gold. If you're buying, go by the player's gold
        var goldToCheck = _isSelling ? shopSystem.AvailableGold : playerInventoryHolder.PrimaryInventorySystem.Gold;

        //if the player can't afford what they're trying to buy, the total text turns red
        basketTotalText.color = basketTotal > goldToCheck ? Color.red : Color.white;

        //if you're selling or the player has enough inventory space
        if (_isSelling || playerInventoryHolder.PrimaryInventorySystem.CheckInventoryRemaining(shoppingCart))
        {
            return;
        }

        basketTotalText.text = "Not enough room in inventory";
        basketTotalText.color = Color.red;
    }

    public static int GetModifiedPrice(ItemClass data, int amount, float markUp)
    {
        var baseValue = data.GoldValue * amount;

        return Mathf.FloorToInt(baseValue + baseValue * markUp);
    }

    private void UpdateItemPreview(ShopSlotUI shopSlotUI)
    {
        var data = shopSlotUI.AssignedItemSlot.Item;

        itemPreviewSprite.sprite = data.itemIcon;
        itemPreviewSprite.color = Color.white;
        itemPreviewName.text = data.itemName;
        itemPreviewDescription.text = data.description;
    }

    public void OnBuyTabPressed()
    {
        _isSelling = false;
        RefreshDisplay();
    }

    public void OnSellTabPressed()
    {
        _isSelling = true;
        RefreshDisplay();
    }

    ///Tabs for different "websites" (wizard gear, wizard ingredients, spells)
    ///public void OnGearTabPressed() { }
    ///public void OnIngredientTabPressed() { }
    ///public void OnSpellTabPressed() { }

    
}
