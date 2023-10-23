using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a scriptable object that defines what an item is in our game.
/// Has inheritance to have branched versions of items such as consumables
/// and equipment
/// </summary>
public class ItemClass : ScriptableObject
{
    [Header("Item")]
    //data shared across every item
    public string itemName;
    public Sprite itemIcon;
    public bool isStackable = true;
    public int stackSize = 64;
    public int price;

    [TextArea(4, 4)]
    public string description;

    public virtual void Use(Player p) 
    {
        Debug.Log("used item");
    }
    public virtual ItemClass GetItem() { return this; }
    public virtual ToolClass GetTool() { return null; }
    public virtual MiscClass GetMisc() { return null; }
    public virtual ConsumableClass GetConsumable() { return null; }
    public virtual SpellClass GetSpell() { return null; }

    internal void Use(Player_Attributes p)
    {
        throw new NotImplementedException();
    }
}
