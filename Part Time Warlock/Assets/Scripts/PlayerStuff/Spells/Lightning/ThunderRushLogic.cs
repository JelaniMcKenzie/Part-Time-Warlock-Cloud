using InventoryPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderRushLogic : MonoBehaviour
{
    /// <summary>
    /// Things this code should do
    /// create a protective aura around the player
    /// make them invincible for the duration of the spell
    /// knockback enemies?
    /// </summary>
    /// 
    public Player player;
    SpellClass dashSpell;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Player>();
        dashSpell = (SpellClass) player.inventory.GetInventorySlot(player.inventory.hotbarUISlots[3]).GetItemType();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = player.transform.position;
            StartCoroutine(DashWithSpell(dashSpell.dashSpellDuration));
            
        }
    }

    public IEnumerator DashWithSpell(float activeTime)
    {
        yield return new WaitForSeconds(activeTime);
        Destroy(this.gameObject);

    }
}
