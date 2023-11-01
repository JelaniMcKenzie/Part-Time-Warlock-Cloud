using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private ShopSlot assignedItemSlot;

    public ShopSlot AssignedItemSlot => assignedItemSlot;

    [SerializeField] private Button addItemToCartButton;
    [SerializeField] private Button removeItemFromCartButton;

    private int tempAmount; //temp refrence to the stock of items in the shop

    public ShopKeeperDisplay ParentDisplay { get; private set; }
    public float MarkUp { get; private set; }

    private void Awake()
    {
        itemSprite.sprite = null;
        itemSprite.preserveAspect = true;
        itemSprite.color = Color.clear;
        itemName.text = "";
        itemCount.text = "";

        addItemToCartButton?.onClick.AddListener(AddItemToCart);
        removeItemFromCartButton?.onClick.AddListener(RemoveItemFromCart);
        ParentDisplay = transform.parent.GetComponentInParent<ShopKeeperDisplay>();
    }

    public void Initialize(ShopSlot slot, float markUp)
    {
        assignedItemSlot = slot;
        MarkUp = markUp;
        tempAmount = slot.Quantity;
        UpdateUISlot();
    }

    private void UpdateUISlot()
    {
        if (assignedItemSlot.Item != null)
        {
            itemSprite.sprite = assignedItemSlot.Item.itemIcon;
            itemSprite.color = Color.white;
            itemCount.text = assignedItemSlot.Quantity.ToString();
            var modifiedPrice = ShopKeeperDisplay.GetModifiedPrice(assignedItemSlot.Item, 1, MarkUp);
            itemName.text = $"{assignedItemSlot.Item.name} x{modifiedPrice}G";
        }
        else
        {
            itemSprite.sprite = null;
            itemSprite.color = Color.clear;
            itemName.text = "";
            itemCount.text = "";
        }
    }

    private void AddItemToCart()
    {
        Debug.Log("Adding item to cart");

        if (tempAmount <= 0)
        {
            return;
        }


        tempAmount--; //item is going into the cart
        ParentDisplay.AddItemToCart(this);
        itemCount.text = tempAmount.ToString();
        
    }

    private void RemoveItemFromCart()
    {
        Debug.Log("Removing item to cart");

        if (tempAmount == assignedItemSlot.Quantity)
        {
            return;
        }

        tempAmount++;
        ParentDisplay.RemoveItemFromCart(this);
        itemCount.text = tempAmount.ToString();

    }


}
