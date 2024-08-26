using JetBrains.Annotations;
using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "(Spell)Spell", menuName = "InventoryPlus/Spell", order = 1)]
public class SpellClass : InventoryPlus.Item
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
    public float knockbackForce;

    [Space(30)]

    public int uses;
    public int maxUses;

    public float maxCooldown;
    public float currentCooldown;



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

    public bool isSpreadShot;
    public float spreadShotNum;

    public float effectRadius;
    public float statusDuration;
    public float dashSpellDuration;
    public float screenShakeIntensity;
    public float screenShakeLength;

    public SpellClass GetSpell() { return this; }


    public void Use(WizardPlayer P)
    {
        CameraShake cam = FindAnyObjectByType<CameraShake>();
        P = FindAnyObjectByType<WizardPlayer>();

        if (currentCooldown > 0)
        {
            Debug.Log("Spell is on cooldown. Remaining time: " + currentCooldown);
            return;
        }

        
        switch (spellType)
        {
            case SpellType.projectile:
                {
                    // Calculate the direction from the player's position to the mouse position
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 direction = mousePosition - P.transform.position; // Remove normalization
                    if (isSpreadShot)
                    {
                        // Handle spread shot logic
                        for (int i = 0; i < spreadShotNum; i++)
                        {
                            GameObject spreadShot = Instantiate(spellPrefab, P.staffTip.transform.position, Quaternion.identity);

                            if (spreadShot.TryGetComponent<PlayerProjectiles>(out var proj))
                            {
                                proj.damage = damage;
                                proj.knockbackForce = knockbackForce;
                            }

                            AudioSource.PlayClipAtPoint(useAudio, spreadShot.transform.position, 2f);
                            
                            // Get the Rigidbody2D component
                            Rigidbody2D rb2d = spreadShot.GetComponent<Rigidbody2D>();

                            // Calculate the rotation angle in degrees
                            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                            //Rotate the projectile to face the mouse position
                            spreadShot.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                            //Rotate each projectile for a fan pattern
                            spreadShot.transform.Rotate(0, 0, (i - 2) * 15);

                            spreadShot.transform.position += spreadShot.transform.right;

                            // Set the velocity
                            rb2d.velocity = (spreadShot.transform.right).normalized * projectileSpeed; // Normalize only when setting velocity

                        }

                    }
                    else
                    {
                        //single shot logic here
                        GameObject projectile = Instantiate(spellPrefab, P.staffTip.transform.position, Quaternion.identity);

                        if (projectile.TryGetComponent<PlayerProjectiles>(out var proj))
                        {
                            proj.damage = damage;
                            proj.knockbackForce = knockbackForce;
                        }

                        AudioSource.PlayClipAtPoint(useAudio, spellPrefab.transform.position, 2f);

                        // Get the Rigidbody2D component
                        Rigidbody2D rb2d = projectile.GetComponent<Rigidbody2D>();

                        // Set the velocity
                        rb2d.velocity = direction.normalized * projectileSpeed; // Normalize only when setting velocity

                        // Calculate the rotation angle in degrees
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                        //Rotate the projectile to face the mouse position
                        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    }

                    
                    cam.ShakeCamera(screenShakeIntensity, screenShakeLength);

                    //Debug.Log("Casted " + this.itemName);
                    break;
                }
            case SpellType.aoe:
                {
                    Instantiate(spellPrefab, P.transform.position, Quaternion.identity);
                    //Debug.Log("Casted aoe spell");
                    break;
                }
            case SpellType.status:
                {
                    //activate spell
                    //Debug.Log("Casted status spell");
                    break;
                }
            case SpellType.movement:
                {
                    //movement logic here

                    //instantiate a prefab. the prefab will have its own script on it, determining
                    //the actual logic for each spell

                    Instantiate(spellPrefab, P.transform.position, Quaternion.identity);
                    AudioSource.PlayClipAtPoint(useAudio, spellPrefab.transform.position);
                    //Debug.Log("Casted movement spell");
                    break;
                }
        }
        uses--;
    }


    public void UpdateCooldown(Image cooldownImage)
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown < 0)
            {
                currentCooldown = 0;
                uses = maxUses;
                Debug.Log("Spell cooldown finished.");
                cooldownImage.color = new Color(0, 0, 0, 1f); // Fully opaque when cooldown is done
            }
            else
            {
                // Update the fill amount of the cooldown image
                float fillValue = 0 + (currentCooldown / maxCooldown);
                cooldownImage.fillAmount = fillValue;
            }
        }
        else if (uses <= 0)
        {
            currentCooldown = maxCooldown;
            Debug.Log("Spell uses exceeded. Cooldown started: " + currentCooldown);
            cooldownImage.color = new Color(0, 0, 0, 229.5f);

            // Set the image to halfway opaque when cooldown starts
            cooldownImage.color = new Color(0, 0, 0, 0.5f);
            cooldownImage.fillAmount = 0f; // Start the fill at 0 when cooldown starts
            return;
        }
        else
        {
            // Set the image to fully transparent and reset fill when idle
            cooldownImage.color = new Color(0, 0, 0, 0f);
            cooldownImage.fillAmount = 1f; // Ensure fill is reset to full when not cooling down
        }
    }
}
