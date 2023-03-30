using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpell : Spell
{
    [SerializeField] public GameObject firePellet = null;
    [SerializeField] public AudioClip fireSound = null;
    public FireSpell()
    {
        pelletPrefab = firePellet;
        soundEffect = fireSound;
        manaCost = 0.3333333333333333333f;
        maxUses = 5;
    }

    protected override float GetPelletSpeed()
    {
        return 20f;
    }
}
