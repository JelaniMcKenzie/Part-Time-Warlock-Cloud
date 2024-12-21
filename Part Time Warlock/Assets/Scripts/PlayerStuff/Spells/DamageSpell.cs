using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSpell : MonoBehaviour
{
    protected WizardPlayer player;
    protected SpellClass spell;
    [SerializeField] protected AudioClip impactSound;
    public float damage;
    public float knockbackForce;
    public float screenShakeIntensity;
    public float screenShakeLength;
    

    [SerializeField] protected CameraShake cam = null;

    private void Start()
    {
        cam = FindAnyObjectByType<CameraShake>();
        Debug.LogWarning(cam);
    }

}
