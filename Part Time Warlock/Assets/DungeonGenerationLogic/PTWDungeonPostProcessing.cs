using Edgar.Unity;
using Edgar.Unity.Examples.Gungeon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that handles logic after the dungeon is generated.
/// (e.g. spawning enemies, moving the player to the spawn position, etc)
/// </summary>
public class PTWDungeonPostProcessing : DungeonGeneratorPostProcessingComponentGrid2D
{
    [Range(0, 1)]
    public float enemySpawnChance = 0.5f;

    public override void Run(DungeonGeneratorLevelGrid2D level)
    {
        //implement post dungeon generation logic here
        MovePlayerToSpawn(level);
        HandleEnemies(level);
    }

    private void HandleEnemies(DungeonGeneratorLevelGrid2D level)
    {
        foreach (var roomInstance in level.RoomInstances)
        {
            //find the empty game object called "Enemies".
            //Note: the gameobject HAS to be called "Enemies" for this code to work
            var enemiesHolder = roomInstance.RoomTemplateInstance.transform.Find("Enemies");

            // Skip this room if there are no enemies
            if (enemiesHolder == null)
            {
                continue;
            }

            // Iterate through all enemies (children of the enemiesHolder)
            foreach (Transform enemyTransform in enemiesHolder)
            {
                /**
                 * currently, this code chooses to set active
                 * a random # of the enemies that we deliberately dragged
                 * into the enemies holder. Upping the spawn rate will probably
                 * change how many enemies can spawn in that room.
                 * what we could do is have small rooms contain maybe 5 enemies, 
                 * and larger rooms have 10.
                 * 
                 * Then, we have each gameobject in the enemies holder ALSO
                 * have a random chance code applied to it, so it chooses which enemy
                 * to spawn from that game object. We could accomplish this with something
                 * like an array that contains the gameobjects of all the enemies we want to spawn.
                 * 
                 */
                var enemy = enemyTransform.gameObject;

                // Roll a dice and check whether to spawn this enemy or not
                // Use the provided Random instance so that the whole generator uses the same seed and the results can be reproduced
                if (Random.NextDouble() < enemySpawnChance)
                {
                    enemy.SetActive(true);
                }
                else
                {
                    enemy.SetActive(false);
                }
            }
        }
    }

    private void MovePlayerToSpawn(DungeonGeneratorLevelGrid2D level)
    {
        foreach (var roomInstance in level.RoomInstances)
        {
            var room = roomInstance.Room;
            var roomTemplateInstance = roomInstance.RoomTemplateInstance;

            // Get spawn position if Entrance
            if (room.GetDisplayName() == "Entrance")
            {
                var spawnPosition = roomTemplateInstance.transform.Find("SpawnPosition");
                var player = GameObject.FindWithTag("Player");
                player.transform.position = spawnPosition.position;
            }
        }
    }

}
