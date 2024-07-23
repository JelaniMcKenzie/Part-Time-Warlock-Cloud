using InventoryPlus;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderRushLogic : DamageSpell
{
    /// <summary>
    /// Things this code should do
    /// create a protective aura around the player
    /// make them invincible for the duration of the spell
    /// knockback enemies?
    /// </summary>
    /// 
    
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<WizardPlayer>();
        spell = (SpellClass) player.inventory.GetInventorySlot(player.inventory.hotbarUISlots[3]).GetItemType();
        damage = spell.damage;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = player.transform.position;
            StartCoroutine(DashWithSpell(spell.dashSpellDuration));
            
        }
    }

    public IEnumerator DashWithSpell(float activeTime)
    {
        player.canHit = false;
        yield return new WaitForSeconds(activeTime);
        player.canHit = true;
        Destroy(this.gameObject);

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            float knockbackForce = 5f;
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        }
    }
}
