using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGeneration : MonoBehaviour
{
    [Header("Insert The different Tiles")]
    [SerializeField]
    private Tile[] floorTile; //array because of reandomizing the floor sprites
    [SerializeField]
    private Tile[] topWallTile; //for the decorations/windows thats why its an array
    [SerializeField]
    private Tile[] botWallTile; //same reason for top walls
    [SerializeField]
    private Tile[] sideWallTile; //see topwall explanation
    [SerializeField]
    private Tile pitTile;

    [Header("Insert the different Tile Maps")]
    [SerializeField]
    private Tilemap floorMap;
    [SerializeField]
    private Tilemap wallMap;
    [SerializeField]
    private Tilemap pitMap;

    [Header("Insert Player Prefab")]
    [SerializeField]
    private GameObject player;

    [Header("Dungeon Generation Settings")]
    [Tooltip("The percentage for a route to deviate from the main route")]
    [SerializeField]
    private int deviationRate = 10;
    [Tooltip("The percentage for a room to spawn")]
    [SerializeField]
    private int roomRate = 15;
    [Tooltip("The number of sections a dungeon will generate on the main route")]
    [SerializeField]
    private int maxRouteLength;
    [Tooltip("The max amount of routes a dungeon can deviate from (always have a limit on this)")]
    [SerializeField]
    private int maxRoutes = 20;
    [Tooltip("The range of sizes a room can be")]
    [Range(3, 10)]
    private int roomSize;


    private int routeCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        int x = 0;
        int y = 0;
        int routeLength = 0;
        GenerateSquare(x, y, 1);
        Vector2Int previousPos = new Vector2Int(x, y);
        y += 3; //each hallway is made of a 3x3 tile set 
        GenerateSquare(x, y, 1);
        NewRoute(x, y, routeLength, previousPos);

        FillWalls();
    }

    private void FillWalls()
    {
        BoundsInt bounds = floorMap.cellBounds;
        for (int xMap = bounds.xMin - 10; xMap <= bounds.xMax + 10; xMap++)
        {
            for (int yMap = bounds.yMin - 10; yMap <= bounds.yMax + 10; yMap++)
            {
                Vector3Int pos = new Vector3Int(xMap, yMap, 0);
                Vector3Int posBelow = new Vector3Int(xMap, yMap - 1, 0);
                Vector3Int posAbove = new Vector3Int(xMap, yMap + 1, 0);
                TileBase tile = floorMap.GetTile(pos);
                TileBase tileBelow = floorMap.GetTile(posBelow);
                TileBase tileAbove = floorMap.GetTile(posAbove);
                if (tile == null)
                {
                    pitMap.SetTile(pos, pitTile);
                    if (tileBelow != null)
                    {
                        wallMap.SetTile(pos, topWallTile[Random.Range(0, topWallTile.Length)]);
                    }
                    else if (tileAbove != null)
                    {
                        wallMap.SetTile(pos, botWallTile[Random.Range(0, botWallTile.Length)]);
                    }
                }
            }
        }
    }


    private void NewRoute(int x, int y, int routeLength, Vector2Int previousPos)
    {
        if (routeCount < maxRoutes)
        {
            routeCount++;
            while (++routeLength < maxRouteLength)
            {
                //Initialize
                bool routeUsed = false;
                int xOffset = x - previousPos.x; //0
                int yOffset = y - previousPos.y; //3
                int roomSize = 1; //Hallway size
                if (Random.Range(1, 100) <= roomRate)
                    roomSize = Random.Range(3, 6);
                previousPos = new Vector2Int(x, y);

                //Go Straight
                if (Random.Range(1, 100) <= deviationRate)
                {
                    if (routeUsed)
                    {
                        GenerateSquare(previousPos.x + xOffset, previousPos.y + yOffset, roomSize);
                        NewRoute(previousPos.x + xOffset, previousPos.y + yOffset, Random.Range(routeLength, maxRouteLength), previousPos);
                    }
                    else
                    {
                        x = previousPos.x + xOffset;
                        y = previousPos.y + yOffset;
                        GenerateSquare(x, y, roomSize);
                        routeUsed = true;
                    }
                }

                //Go left
                if (Random.Range(1, 100) <= deviationRate)
                {
                    if (routeUsed)
                    {
                        GenerateSquare(previousPos.x - yOffset, previousPos.y + xOffset, roomSize);
                        NewRoute(previousPos.x - yOffset, previousPos.y + xOffset, Random.Range(routeLength, maxRouteLength), previousPos);
                    }
                    else
                    {
                        y = previousPos.y + xOffset;
                        x = previousPos.x - yOffset;
                        GenerateSquare(x, y, roomSize);
                        routeUsed = true;
                    }
                }
                //Go right
                if (Random.Range(1, 100) <= deviationRate)
                {
                    if (routeUsed)
                    {
                        GenerateSquare(previousPos.x + yOffset, previousPos.y - xOffset, roomSize);
                        NewRoute(previousPos.x + yOffset, previousPos.y - xOffset, Random.Range(routeLength, maxRouteLength), previousPos);
                    }
                    else
                    {
                        y = previousPos.y - xOffset;
                        x = previousPos.x + yOffset;
                        GenerateSquare(x, y, roomSize);
                        routeUsed = true;
                    }
                }

                if (!routeUsed)
                {
                    x = previousPos.x + xOffset;
                    y = previousPos.y + yOffset;
                    GenerateSquare(x, y, roomSize);
                }
            }
        }
    }


    private void GenerateSquare(int x, int y, int radius)
    {
        for (int tileX = x - radius; tileX <= x + radius; tileX++)
        {
            for (int tileY = y - radius; tileY <= y + radius; tileY++)
            {
                Vector3Int tilePos = new Vector3Int(tileX, tileY, 0);
                floorMap.SetTile(tilePos, floorTile[Random.Range(0, floorTile.Length)]); //produces a random tile from the array of tiles
            }
        }
    }
}
