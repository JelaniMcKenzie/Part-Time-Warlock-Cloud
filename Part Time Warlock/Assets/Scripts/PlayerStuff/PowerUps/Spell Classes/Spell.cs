using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : ScriptableObject
{
    public GameObject pelletPrefab;
    public AudioClip soundEffect;
    public ManaBar manaBar;
    public float mana;
    public float manaCost;
    public int maxUses;

    public virtual void Cast(Transform staffTip, Transform cameraTransform)
    {
        // Instantiate the pellet at the staff tip
        GameObject pellet = Instantiate(pelletPrefab, staffTip.position, Quaternion.identity);

        // Set the pellet's direction and velocity based on the camera and mouse position
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(cameraTransform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        pellet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        pellet.GetComponent<Rigidbody>().velocity = pellet.transform.right * GetPelletSpeed();

        // Play the spell sound effect
        AudioSource.PlayClipAtPoint(soundEffect, pelletPrefab.transform.position, 1);

        // Deduct mana and reduce spell uses
        mana -= manaCost;
        maxUses--;
        manaBar.UpdateManaBar();
    }

    protected virtual float GetPelletSpeed()
    {
        return 10f;
    }
}

