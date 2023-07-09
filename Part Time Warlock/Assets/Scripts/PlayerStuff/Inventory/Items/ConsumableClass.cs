using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Consumable Class", menuName = "Item/Consumable")]
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
        p.inventory.UseConsumable();
    }


}
