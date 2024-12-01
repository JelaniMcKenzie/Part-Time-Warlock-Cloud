using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSpell : MonoBehaviour
{
    protected WizardPlayer player;
    public SpellClass spell;
    protected CameraShake cs;
    [SerializeField] public float shakeLength, shakeIntensity;
    public float damage, knockbackForce;

}
