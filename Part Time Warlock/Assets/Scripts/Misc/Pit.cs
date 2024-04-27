using InventoryPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
    private bool playerFalling = false;
    private Vector3 playerStartPosition;
    private Vector3 defaultSize = new Vector3(0.5f, 0.5f, 0.5f);
    private float resetDelay = 1f; // Adjust this value for how long to wait before resetting the player
    private float respawnOffset = 1f; // Adjust this value for how far the player respawns from the pit

    public void FallPlayer()
    {
        if (!playerFalling)
        {
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                playerStartPosition = player.transform.position;
                playerFalling = true;
                player.StartShrinking(resetDelay); // Start shrinking the player
                Invoke("ResetPlayerPosition", resetDelay);
            }
        }
    }

    private void ResetPlayerPosition()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            // Calculate the closest point on the pit's collider to the player's position
            Collider2D pitCollider = GetComponent<Collider2D>();
            Vector3 closestPoint = pitCollider.ClosestPoint(playerStartPosition);

            // Calculate the direction from the closest point to the player's position
            Vector3 respawnDirection = (playerStartPosition - closestPoint).normalized;

            // Calculate the respawn position based on the closest point and respawn direction
            Vector3 respawnPosition = closestPoint + respawnDirection * respawnOffset;

            // Ensure the player respawns above the pit
            respawnPosition.y = Mathf.Max(respawnPosition.y, playerStartPosition.y + respawnOffset);

            // Reset player's scale
            player.transform.localScale = defaultSize;

            // Set player's position to the respawn position
            player.transform.position = respawnPosition;

            playerFalling = false;
        }
    }
}
