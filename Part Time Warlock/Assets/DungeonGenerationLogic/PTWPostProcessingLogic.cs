using Edgar.Legacy.Core.MapLayouts;
using Edgar.Unity;
using Edgar.Unity.Examples.CurrentRoomDetection;
using Edgar.Unity.Examples.Gungeon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Cinemachine.DocumentationSortingAttribute;
using static UnityEngine.EventSystems.EventTrigger;

/// <summary>
/// A class that handles logic after the dungeon is generated.
/// (e.g. spawning enemies, moving the player to the spawn position, etc)
/// </summary>
public class PTWDungeonPostProcessingLogic : DungeonGeneratorPostProcessingComponentGrid2D
{
    [Range(0, 1)]
    public float enemySpawnChance = 0.5f;
    public GameObject[] Enemies;

    public override void Run(DungeonGeneratorLevelGrid2D level)
    {
        //implement post dungeon generation logic here
        MovePlayerToSpawn(level);
        EnableGraffiti(level);
        EnableFogOfWar(level);

        var walls = level.GetSharedTilemaps().First(x => x.name == "Walls");
        walls.gameObject.tag = "Border";

        foreach (var roomInstance in level.RoomInstances)
        {
            var room = (GungeonRoom)roomInstance.Room;
            var roomTemplateInstance = roomInstance.RoomTemplateInstance;

            // Find floor tilemap layer
            var tilemaps = RoomTemplateUtilsGrid2D.GetTilemaps(roomTemplateInstance);
            var floor = tilemaps.Single(x => x.name == "Floor").gameObject;

            // Add floor collider
            AddFloorCollider(floor);

            // Add current room detection handler
            floor.AddComponent<GungeonCurrentRoomHandler>();

            // Add room manager
            var roomManager = roomTemplateInstance.AddComponent<GungeonRoomManager>();

            if (room.Type != GungeonRoomType.Corridor)
            {
                // Set enemies and floor collider to the room manager
                roomManager.EnemyPrefabs = Enemies;
                roomManager.FloorCollider = floor.GetComponent<CompositeCollider2D>();

                // Find all the doors of neighboring corridors and save them in the room manager
                // The term "door" has two different meanings here:
                //   1. it represents the connection point between two rooms in the level
                //   2. it represents the door game object that we have inside each corridor
                foreach (var door in roomInstance.Doors)
                {
                    // Get the room instance of the room that is connected via this door
                    var corridorRoom = door.ConnectedRoomInstance;

                    // Get the room template instance of the corridor room
                    var corridorGameObject = corridorRoom.RoomTemplateInstance;

                    // Find the door game object by its name
                    var doorsGameObject = corridorGameObject.transform.Find("Door")?.gameObject;

                    // Each corridor room instance has a connection that represents the edge in the level graph
                    // We use the connection object to check if the corridor should be locked or not
                    var connection = (GungeonConnection)corridorRoom.Connection;

                    if (doorsGameObject != null)
                    {
                        // If the connection is locked, we set the Locked state and keep the game object active
                        // Otherwise we set the EnemyLocked state and deactivate the door. That means that the door is active and locked
                        // only when there are enemies in the room.
                        if (connection.IsLocked)
                        {
                            doorsGameObject.GetComponent<GungeonDoor>().State = GungeonDoor.DoorState.Locked;
                        }
                        else
                        {
                            doorsGameObject.GetComponent<GungeonDoor>().State = GungeonDoor.DoorState.EnemyLocked;
                            doorsGameObject.SetActive(false);
                        }

                        roomManager.Doors.Add(doorsGameObject);
                    }
                }
            }
        }
    }

    private void HandleBosses(DungeonGeneratorLevelGrid2D level)
    {
        foreach (var roomInstance in level.RoomInstances)
        {
            //find the empty game object called "Enemies".
            //Note: the gameobject HAS to be called "Enemies" for this code to work
            var enemiesHolder = roomInstance.RoomTemplateInstance.transform.Find("Boss");

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
                 */
                 
                var enemy = enemyTransform.gameObject;

                enemy.SetActive(true);
                //play boss music
               
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

                if (player != null)
                {
                    player.transform.position = spawnPosition.position;
                }
                else
                {
                    Debug.LogError("Could not find Player");
                }
                
                if (portal != null)
                {
                    portal.transform.position = spawnPosition.position;
                    portal.SetActive(false);
                }
                
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

    private void EnableFogOfWar(DungeonGeneratorLevelGrid2D level)
    {
        // Make sure that the FogOfWarGrid2D component exists in the scene
        if (FogOfWarGrid2D.Instance == null)
        {
            throw new InvalidOperationException("It looks like the FogOfWarGrid2D component does not exist in the scene. Make sure that it is attached to your camera.");
        }

        // To setup the FogOfWar component, we need to get the root game object that holds the level.
        var generatedLevelRoot = level.RootGameObject;

        // If we use the Wave mode, we must specify the point from which the wave spreads as we reveal a room.
        // The easiest way to do so is to get the player game object and use its transform as the wave origin.
        // Change this line if your player game object does not have the "Player" tag.
        var playerTag = "Player";
        var player = GameObject.FindGameObjectWithTag(playerTag);
        if (player == null)
        {
            throw new InvalidOperationException($"GameObject with tag '{playerTag}' was not found. Make sure that your object exists in the scene and has the correct tag.");
        }

        // Now we can setup the FogOfWar component.
        // To make it easier to work with the component, the class is a singleton and provides the Instance property.
        FogOfWarGrid2D.Instance.Setup(generatedLevelRoot, player.transform);

        // After the level is generated, we usually want to reveal the spawn room.
        // To do that, we have to find the room instance that corresponds to the Spawn room.
        // In this example, the spawn room is called "Entrance" so we find it by its name.
        var spawnRoom = level
            .RoomInstances
            .SingleOrDefault(x => x.Room.GetDisplayName() == "Entrance");

        if (spawnRoom == null)
        {
            throw new InvalidOperationException("There must be exactly one room with the name 'Spawn' for this example to work.");
        }

        // When we have the spawn room instance, we can reveal the room from the fog.
        // We use revealImmediately: true so that the first room is revealed instantly,
        // but it is optional.
        FogOfWarGrid2D.Instance.RevealRoom(spawnRoom, revealImmediately: true);
    }

}
