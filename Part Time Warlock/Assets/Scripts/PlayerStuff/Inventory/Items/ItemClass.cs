using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClass : ScriptableObject
{
    [Header("Item")]
    //data shared across every item
    public string itemName;
    public Sprite itemIcon;
    public bool isStackable = true;
    public int stackSize = 64;

    public virtual void Use(Player p) 
    {
        Debug.Log("used item");
    }
    public virtual ItemClass GetItem() { return this; }
    public virtual ToolClass GetTool() { return null; }
    public virtual MiscClass GetMisc() { return null; }
    public virtual ConsumableClass GetConsumable() { return null; }
    public virtual SpellClass GetSpell() { return null; }
}
