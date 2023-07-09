using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Spell", menuName = "Item/Spell")]
public class SpellClass : ItemClass
{
    //Idea: Should individual spells have their own scripts that
            //inherit from this base spell class?
    //Idea: have the sound of the spell instantiate from this class
    //Idea: create four subclasses that inherit from the spell class
            //to handle projectile, status, aoe, and movement logic
            //rather than doing it all in one class


    public Sprite spellRune;
    public GameObject spellPrefab;
    public string spellElement;
    public float damage;
    public float cooldown;
    public bool canCast;
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

    public override SpellClass GetSpell() { return this; }

    public override void Use(Player P) 
    {
        P = FindAnyObjectByType<Player>();
        switch (spellType)
        {
            case SpellType.projectile:
                {
                    GameObject projectile = Instantiate(spellPrefab, P.staffTip.transform.position, P.staffTip.transform.rotation);
                    projectile.GetComponent<Rigidbody>().AddForce(P.staffTip.transform.up * projectileSpeed, ForceMode.Impulse);
                    Debug.Log("Casted projectile spell");
                    if (uses <= 0)
                    {
                        canCast = false;
                    }
                    break;
                }
            case SpellType.aoe:
                {
                    Instantiate(spellPrefab, P.transform.position, Quaternion.identity);
                    Debug.Log("Casted aoe spell");
                    if (uses <= 0)
                    {
                        canCast = false;
                    }
                    break;
                }
            case SpellType.status:
                {
                    //activate spell
                    Debug.Log("Casted status spell");
                    if (uses <= 0)
                    {
                        canCast = false;
                    }
                    break;
                }
            case SpellType.movement:
                {
                    //movement logic here
                    Debug.Log("Casted movement spell");
                    if (uses <= 0)
                    {
                        canCast = false;
                    }
                    break;
                }
        }
    }

    public IEnumerator SpellCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        uses = maxUses;
        canCast = true;
        
    }
}
