using InventoryPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheCoolestLogic : MonoBehaviour
{
    private WizardPlayer player;
    private SpellClass spell;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<WizardPlayer>();
        
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
                if (s.itemAttribute == "Ice")
                {
                    s.maxCooldown /= 2f;
                    break;
                }

            }
        }
    }
}
