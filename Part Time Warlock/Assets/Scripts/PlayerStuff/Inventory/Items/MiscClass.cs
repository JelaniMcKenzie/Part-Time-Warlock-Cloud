using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Misc Class", menuName = "Item/Misc")]
public class MiscClass : ItemClass
{

    //data specific to the MiscClass

    public override ItemClass GetItem() { return this; }
    //return this works because its of a type tool class AND a type of itemclass

    public override ToolClass GetTool() { return null; }

    //return this doesn't work for MiscClass or ConsumableClass because its not of type tool
    public override MiscClass GetMisc() { return this; }

    public override ConsumableClass GetConsumable() { return null; }

    public override SpellClass GetSpell() { return null; }

}
