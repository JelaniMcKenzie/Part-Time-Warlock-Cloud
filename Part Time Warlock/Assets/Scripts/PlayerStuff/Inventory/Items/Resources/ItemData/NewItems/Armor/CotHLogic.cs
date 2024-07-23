using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CotHLogic : MonoBehaviour
{
    public Player player;
    private float defaultMoveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Player>();

        if (player != null)
        {
            // Store the default moveSpeed
            defaultMoveSpeed = player.moveSpeed;

            // Multiply moveSpeed by 2 only if it hasn't been multiplied before
            if (!player.moveSpeedMultiplied)
            {
                player.moveSpeed *= 1.2f;
                player.moveSpeedMultiplied = true; // Add a boolean flag to indicate that moveSpeed has been multiplied
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Your regular Update logic (if any)
    }

    // Method to revert moveSpeed to default value
    private void RevertMoveSpeed()
    {
        if (player != null)
        {
            player.moveSpeed = defaultMoveSpeed;
            player.moveSpeedMultiplied = false; // Reset the flag
        }
    }

    // Example: You might call this method when the object is destroyed
    private void OnDestroy()
    {
        RevertMoveSpeed();
    }
}
