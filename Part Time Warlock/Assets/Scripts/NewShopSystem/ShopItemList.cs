using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop System/Shop Item List")]
public class ShopItemList : ScriptableObject
{
    [SerializeField] private List<ShopInventoryItem> items;
    [SerializeField] private int maxAllowedGold;
    [SerializeField] private float sellMarkUp;
    [SerializeField] private float buyMarkUp;

    public List<ShopInventoryItem> Items => items;
    public int MaxAllowedGold => maxAllowedGold;
    public float SellMarkUp => sellMarkUp;
    public float BuyMarkUp => buyMarkUp;

    ///the buyMarkUp will have the shop mark up the price by x percent
    ///the sellMarkUp will have the shop mark up the sell price by x percent (e.g. it'll give you 75 gold for something worth 100 gold)
    /// we can always change the values to give the player more gold for a sell, or mark up the buy price by a metric

    [System.Serializable]
    public struct ShopInventoryItem
    {
        public ItemClass item;
        public int amount;
    }
}
