using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Tool")]
public class ToolClass : ItemClass
{
    [Header("Tool")]
    //data specific to tool class

    public ToolType toolType;
    public enum ToolType
    {
        staff
        
    }

    public override ToolClass GetTool() { return this; }

    public override void Use(Player p)
    {
        base.Use(p);
        Debug.Log("swing tool");
    }
}
