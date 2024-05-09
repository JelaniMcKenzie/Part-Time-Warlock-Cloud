using InventoryPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCTLogic : MonoBehaviour
{
    private Player player;
    public int maxBonusDamage = 20; // Maximum bonus damage that can be added
    public int coinThreshold = 10; // Number of coins needed to reach max bonus damage

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < player.inventory.inventoryItems.Count; i++)
        {
            if (player.inventory.inventoryItems[i].GetItemType() is SpellClass)
            {
                //Downcast from ItemSlot to SpellClass to access SpellClass methods
                SpellClass s = (SpellClass)player.inventory.inventoryItems[i].GetItemType();
                float bonusDamage = CalculateBonusDamage();
                s.damage += bonusDamage;

            }
        }
    }

    private float CalculateBonusDamage()
    {
        int currentCoins = player.coinNum;
        int bonusDamage = 0;

        if (currentCoins >= coinThreshold)
        {
            // Calculate bonus damage based on the number of coins
            float bonusPercentage = Mathf.Min(1.0f, (float)(currentCoins - coinThreshold) / (float)coinThreshold);
            bonusDamage = Mathf.RoundToInt(maxBonusDamage * bonusPercentage);
        }

        return bonusDamage;
    }
}
