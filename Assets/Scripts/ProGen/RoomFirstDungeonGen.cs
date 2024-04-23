using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFirstDungeonGen : SimpleRandomWalkDungeonGen
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0, 10)]
    private int offset = 1; //making the offset as naturally the BPS algo will make it so that the rooms might collide 
    [SerializeField]
    private bool randomWalkRooms = false; //checking if it is gonna be square rooms or random shaped ones
    [SerializeField]
    private GameObject startSquare;
    [SerializeField]
    private GameObject endSquare;

    [SerializeField]
    private GameObject playerObj;
    Vector2Int startPos, endPos;

    [SerializeField]
    private GameObject enemy1, enemy2, enemy3, enemy4, enemy5;

    [SerializeField]
    private GameObject bigEnemy;

    private LevelType levelType = LevelType.Underground;

    GameObject[] enemyArr;
    List<Vector2Int> enemyPos, wallPos;
    void Start()
    {
        enemyArr = new GameObject[] { enemy1, enemy2, enemy3, enemy4, enemy5 };

        for (int i = 0; i < enemyPos.Count; i++)
        {
            // Select a random enemy from enemyArr
            GameObject enemyToSpawn = enemyArr[Random.Range(0, enemyArr.Length)];

            // Convert Vector2Int to Vector3
            Vector3 spawnPosition = new Vector3(enemyPos[i].x, enemyPos[i].y, 0);

            if (wallPos.Contains(enemyPos[i]))
            {
                Debug.Log("Enemy spawned on wall");
                continue;
            }
            else
            {
                // Spawn the enemy at the position
                Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
            }
        }

        playerObj.transform.position = new Vector3(startPos.x, startPos.y, 0);
        Instantiate(bigEnemy, new Vector3(endPos.x, endPos.y, 0), Quaternion.identity);

        Debug.Log("Room First Dungeon Generation");
    }

    public void RunGeneration()
    {
        RunProceduralGeneration();
    }
    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        //calling BPS algo, passing the start point and ints created above
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        if (randomWalkRooms)
            floor = CreateRoomsRandomly(roomsList);
        else
            floor = CreateSimpleRooms(roomsList);

        Debug.Log(floor.Count);
        // foreach (Vector2Int position in floor)
        // {
        //     Debug.Log($"Position: ({position.x}, {position.y})");
        // }

        //need to know the center of each room to connect em 
        //put all the centers in a list -> choose a random center point -> find the nearest point and connect the two centers -> delete points from list -> continue until all rooms are connected
        //this way it isn't all over the place, instead there is a linear path and a clear start and end room
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
            //Debug.Log(room); // Print each room (you can customize this line)
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);
        enemyPos = tilemapVisualizer.PaintFloorTiles(floor);
        Debug.Log(enemyPos.Count);

        if (bigEnemy != null)
            levelType = LevelType.Overworld;

        wallPos = WallGen.CreateWalls(floor, tilemapVisualizer, levelType);

        //startSquare.transform.position = new Vector3(startPos.x, startPos.y, 10);
        //playerObj.transform.position = new Vector3(startPos.x, startPos.y, 0);
        endSquare.transform.position = new Vector3(endPos.x, endPos.y, 0);
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);

            foreach (var position in roomFloor)
                if (position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset) && position.y >= (roomBounds.yMin - offset) && position.y <= (roomBounds.yMax - offset))
                    floor.Add(position);

            if (i == 0)
                //Debug.Log(roomCenter);
                startPos = roomCenter;
            if (i == roomsList.Count - 1)
                //Debug.Log(roomCenter);
                endPos = roomCenter;
        }
        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != destination.y)
        {
            if (destination.y > position.y)
                position += Vector2Int.up;
            else if (destination.y < position.y)
                position += Vector2Int.down;
            corridor.Add(position);
        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
                position += Vector2Int.right;
            else if (destination.x < position.x)
                position += Vector2Int.left;
            corridor.Add(position);
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        //simple algo to find closest distance between 2 center points 
        Vector2Int closest = Vector2Int.zero;
        float dist = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < dist)
            {
                dist = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomsList)
            for (int col = offset; col < room.size.x - offset; col++)
                for (int row = offset; row < room.size.y - offset; row++) //applying offset to room size
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
        //if we want to decorate the floor in the future (maybe for stretch goal), would have to save each floor in each room in a seperate HashSet so that it can be further changed
        //as right now all the floors for every room is in one HashSet
        return floor;
    }
}
