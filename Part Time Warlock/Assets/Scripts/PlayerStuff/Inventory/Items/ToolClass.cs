using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Tool Class", menuName = "Item/Tool")]
public class ToolClass : ItemClass
{
    [Header("Tool")]
    //data specific to tool class
    public ToolType toolType;
    public enum ToolType
    {
        staff
        
    }
    public override ItemClass GetItem() { return this; }
    //return this works because its of a type tool class AND a type of itemclass

    public override ToolClass GetTool() { return this; }

    //return this doesn't work for MiscClass or ConsumableClass because its not of type tool
    public override MiscClass GetMisc() { return null; }
    
    public override ConsumableClass GetConsumable() { return null; }

    public override SpellClass GetSpell() { return null; }
}
