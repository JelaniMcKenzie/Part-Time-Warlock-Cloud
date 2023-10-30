using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Misc")]
public class MiscClass : ItemClass
{

    //data specific to the MiscClass

    public override MiscClass GetMisc() { return this; }

    public override void Use(Player p)
    {
        //base.Use(p); //calls the function as implemented in the base class
                     //in this case, the base class the item class
    }

}
