using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSpell : Spell
{
    [SerializeField] public GameObject lightningPellet = null;
    [SerializeField] public AudioClip lightningSound = null;
    public LightningSpell()
    {
        pelletPrefab = lightningPellet;
        soundEffect = lightningSound;
        manaCost = 0.5f;
        maxUses = 3;
    }

    protected override float GetPelletSpeed()
    {
        return 30f;
    }
}
