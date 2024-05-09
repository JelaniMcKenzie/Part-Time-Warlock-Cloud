using Edgar.Legacy.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<InventoryPlus.Item> itemList = new List<InventoryPlus.Item>();
    public List<InventoryPlus.Item> shopStock = new List<InventoryPlus.Item>();

    public PTWShopItem[] shopItem = new PTWShopItem[3];
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < shopItem.Length; i++)
        {
            if (itemList.Count == 0)
            {
                break;
            }

            int randomIndex = Random.Range(0, itemList.Count);

            InventoryPlus.Item _item = itemList[randomIndex];
            shopStock.Add(_item);
            itemList.Remove(_item);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveItemFromList(InventoryPlus.Item _item)
    {
        shopStock.Remove(_item);
    }
}
