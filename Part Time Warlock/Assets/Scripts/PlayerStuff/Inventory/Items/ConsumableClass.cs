using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Consumable")]
public class ConsumableClass : ItemClass
{
    [Header("Consumable")]
    //data specific to the consumable class
    public float healthAdded; //example placeholder variable

    public override ConsumableClass GetConsumable() { return this; }

    public override void Use(Player p)
    {
        base.Use(p);
        Debug.Log("Eat Consumable");
    }


}
