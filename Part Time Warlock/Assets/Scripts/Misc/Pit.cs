using InventoryPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
    private bool playerFalling = false;
    private Vector3 defaultSize = new Vector3(0.5f, 0.5f, 0f);
    private float resetDelay = 1f; // Adjust this value for how long to wait before resetting the player



    public void FallPlayer(WizardPlayer player)
    {
        if (playerFalling == false)
        {
            if (player != null)
            {
                playerFalling = true;
                player.StartShrinking(resetDelay); // Start shrinking the player
                StartCoroutine(ReverseMovement(player));
            }
        }
    }

    private IEnumerator ReverseMovement(WizardPlayer player)
    {
        // Wait for one second
        yield return new WaitForSeconds(1f);

        // Restore the player's position
        player.transform.localPosition = player.pitRespawnPos;
        player.transform.localScale = defaultSize;
        playerFalling = false;
        player.EnableMovement(true);
    }
    
}
