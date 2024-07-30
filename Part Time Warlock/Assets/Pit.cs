using InventoryPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            WizardPlayer player = other.gameObject.GetComponent<WizardPlayer>();
            if (player != null)
            {
                Vector2 entryPoint = other.gameObject.transform.position;
                player.SetRespawnPosition(entryPoint);
            }
        }
    }
}
