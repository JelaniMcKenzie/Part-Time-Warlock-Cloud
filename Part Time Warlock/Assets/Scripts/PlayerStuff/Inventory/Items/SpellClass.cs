using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


[CreateAssetMenu(menuName = "Inventory System/Spell")]
public class SpellClass : ItemClass
{
    //Idea: Should individual spells have their own scripts that
            //inherit from this base spell class?
    //Idea: have the sound of the spell instantiate from this class
    //Idea: create four subclasses that inherit from the spell class
            //to handle projectile, status, aoe, and movement logic
            //rather than doing it all in one class


    public Sprite spellRune; //consider removing as itemIcon does the same thing
    public GameObject spellPrefab;
    public string spellElement;
    public float damage;
    public int uses;
    public int maxUses;

    public float maxCooldown;
    private float currentCooldown;

    [SerializeField] public AudioClip spellSound = null;

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
    public float movementDistance; //for dash spells specifically (e.g. how far do you dash with this spell)

    public override SpellClass GetSpell() { return this; }


    public override void Use(Player P) 
    {
        if (currentCooldown > 0)
        {
            Debug.Log("Spell is on cooldown. Remaining time: " + currentCooldown);
            return;
        }

        P = FindAnyObjectByType<Player>();
        switch (spellType)
        {
            case SpellType.projectile:
                {
                    GameObject projectile = Instantiate(spellPrefab, P.staffTip.transform.position, Quaternion.identity);

                    // Calculate the direction from the player's position to the mouse position
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 direction = (mousePosition - P.transform.position).normalized;

                    // Calculate the angle
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                    // Rotate the projectile
                    projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    // Get the Rigidbody2D component
                    Rigidbody2D rb2d = projectile.GetComponent<Rigidbody2D>();

                    // Set the velocity
                    rb2d.velocity = direction * projectileSpeed;

                    Debug.Log("Casted " + this.itemName);
                    break;
                }
            case SpellType.aoe:
                {
                    Instantiate(spellPrefab, P.transform.position, Quaternion.identity);
                    Debug.Log("Casted aoe spell");
                    break;
                }
            case SpellType.status:
                {
                    //activate spell
                    Debug.Log("Casted status spell");
                    break;
                }
            case SpellType.movement:
                {
                    //movement logic here
                    Debug.Log("Casted movement spell");
                    break;
                }
        }
        uses--;
    }

    public void UpdateCooldown()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown < 0)
            {
                currentCooldown = 0;
                uses = maxUses;
                Debug.Log("Spell cooldown finished.");
            }
        }
        else if (uses <= 0)
        {
            currentCooldown = maxCooldown;
            Debug.Log("Spell uses exceeded. Cooldown started: " + currentCooldown);
            return;
        }
    }
}
