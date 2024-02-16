using Edgar.Unity;
using Edgar.Unity.Examples.CurrentRoomDetection;
using Edgar.Unity.Examples.Gungeon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.EventSystems.EventTrigger;

/// <summary>
/// A class that handles logic after the dungeon is generated.
/// (e.g. spawning enemies, moving the player to the spawn position, etc)
/// </summary>
public class PTWDungeonPostProcessingLogic : DungeonGeneratorPostProcessingComponentGrid2D
{
    [Range(0, 1)]
    public float enemySpawnChance = 0.5f;

    public override void Run(DungeonGeneratorLevelGrid2D level)
    {
        //implement post dungeon generation logic here
        MovePlayerToSpawn(level);
        HandleEnemies(level);
        EnableGraffiti(level);

        var walls = level.GetSharedTilemaps().First(x => x.name == "Walls");
        walls.gameObject.tag = "Border";

        foreach (var roomInstance in level.RoomInstances)
        {
            var roomTemplateInstance = roomInstance.RoomTemplateInstance;

            // Find floor tilemap layer
            var tilemaps = RoomTemplateUtilsGrid2D.GetTilemaps(roomTemplateInstance);
            var floor = tilemaps.Single(x => x.name == "Floor").gameObject;

            // Add floor collider
            AddFloorCollider(floor);

            // Add current room detection handler
            floor.AddComponent<CurrentRoomDetectionTriggerHandler>();
        }
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

    private void EnableGraffiti(DungeonGeneratorLevelGrid2D level)
    {
        foreach (var roomInstance in level.RoomInstances)
        {
            var graffitiHolder = roomInstance.RoomTemplateInstance.transform.Find("Graffiti");

            if (graffitiHolder == null)
            {
                
                continue;
            }

            foreach (Transform graffitiTransform in graffitiHolder)
            {
                Debug.Log("AAAAAA");
                var graffiti = graffitiTransform.gameObject;
                if (Random.NextDouble() < enemySpawnChance)
                {
                    graffiti.SetActive(true);
                }
                else
                {
                    graffiti.SetActive(false);
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
                var portal = GameObject.FindWithTag("Portal");
                player.transform.position = spawnPosition.position;
                portal.transform.position = spawnPosition.position;
                portal.SetActive(false);

            }
        }
    }

    private void AddFloorCollider(GameObject floor)
    {
        var tilemapCollider2D = floor.AddComponent<TilemapCollider2D>();
        tilemapCollider2D.usedByComposite = true;

        var compositeCollider2d = floor.AddComponent<CompositeCollider2D>();
        compositeCollider2d.geometryType = CompositeCollider2D.GeometryType.Polygons;
        compositeCollider2d.isTrigger = true;
        compositeCollider2d.generationType = CompositeCollider2D.GenerationType.Manual;

        floor.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

}
