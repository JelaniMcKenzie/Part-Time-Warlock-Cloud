using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpell : Spell
{
    [SerializeField] public GameObject icePellet = null;
    [SerializeField] public AudioClip iceSound = null;
    public IceSpell()
    {
        pelletPrefab = icePellet;
        soundEffect = iceSound;
        manaCost = 0.2f;
        maxUses = 10;
    }
}
