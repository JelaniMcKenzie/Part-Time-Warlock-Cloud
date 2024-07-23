using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "(Con)Consumable", menuName = "InventoryPlus/Consumable", order = 1)]
public class ConsumableClass : InventoryPlus.Item
{
    [Header("Consumable")]

    //a game object to instantiate, apply logic to, and immediately destroy
    public GameObject consumable;

    public void Use(WizardPlayer p)
    {
        Instantiate(consumable);
    }
}
