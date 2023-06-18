using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Spell", menuName = "Item/Spell")]
public class SpellClass : ItemClass
{
    public Sprite spellRune;
    public GameObject spellPrefab;
    public string spellElement;
    public float damage;
    public float cooldown;
    public int uses;
    public int maxUses;

    [Space(30)]
    public SpellType spellType;
    public enum SpellType
    {
        projectile,
        aoe,
        status,
        movement
    }

    public float projectileSpeed;
    public float effectRadius;
    public float statusDuration;
    public float movementDistance;

    public override ItemClass GetItem() { return this; }
    //return this works because its of a type tool class AND a type of itemclass

    public override ToolClass GetTool() { return null; }

    //return this doesn't work for MiscClass or ConsumableClass because its not of type tool
    public override MiscClass GetMisc() { return null; }

    public override ConsumableClass GetConsumable() { return null; }

    public override SpellClass GetSpell() { return this; }

    public void Cast() 
    { 
        switch (spellType)
        {
            case SpellType.projectile:
                {
                    break;
                }
            case SpellType.aoe:
                {
                    break;
                }
            case SpellType.status:
                {
                    break;
                }
            case SpellType.movement:
                {
                    break;
                }
        }
    }
}
