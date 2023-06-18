using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Consumable Class", menuName = "Item/Consumable")]
public class ConsumableClass : ItemClass
{
    [Header("Consumable")]
    //data specific to the consumable class
    public float healthAdded; //example placeholder variable


    public override ItemClass GetItem() { return this; }
    //return this works because its of a type tool class AND a type of itemclass

    public override ToolClass GetTool() { return null; }

    //return this doesn't work for MiscClass or ConsumableClass because its not of type tool
    public override MiscClass GetMisc() { return null; }

    public override ConsumableClass GetConsumable() { return this; }

    public override SpellClass GetSpell() { return null; }

}
