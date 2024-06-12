using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
//using UnityEditor.VersionControl;
using UnityEngine;

public class CorridorFirstDungeonGen : SimpleRandomWalkDungeonGen
{
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;

    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercent = 0.8f;
    [SerializeField]


    protected override void RunProceduralGeneration()
    {
        CorridorFirstGen();
    }

    private void CorridorFirstGen()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        List<List<Vector2Int>> corridors = CreateCorridors(floorPositions, potentialRoomPositions);

        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        floorPositions.UnionWith(roomPositions);

        for (int i = 0; i < corridors.Count; i++)
        {
            //corridors[i] = IncreaseCorridorSizeByOne(corridors[i]);
            corridors[i] = IncreaseCorridorSizeByThree(corridors[i]);
            floorPositions.UnionWith(corridors[i]);
        }

        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGen.CreateWalls(floorPositions, tilemapVisualizer, LevelType.Underground);

    }

    //making sure all corridors lead to a room, not just nowhere 
    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var position in deadEnds)
        {
            if (roomFloors.Contains(position) == false)
            {
                var room = RunRandomWalk(randomWalkParameters, position);
                roomFloors.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (var position in floorPositions)
        {
            int neighboursCount = 0;
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                if (floorPositions.Contains(position + direction))
                    neighboursCount++;

            }
            if (neighboursCount == 1)
                deadEnds.Add(position);
        }
        return deadEnds;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent); //taking 80% of the possible rooms and making rooms there 

        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();
        //sorted the list of potential room pos and picked at random a list of actual room pos that will be used

        foreach (var roomPosition in roomsToCreate) //looping thru each position and creating a dunegon there
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition); //generates room at the chosen positions
            roomPositions.UnionWith(roomFloor); //again, hashset helps avoid collisions in uninon 
        }
        return roomPositions;
    }

    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition); //at each end of a corridor a room gens 
        List<List<Vector2Int>> cor = new List<List<Vector2Int>>();

        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength); //will generate corrdier pos
            cor.Add(corridor);
            currentPosition = corridor[corridor.Count - 1]; //setting current pos to last pos so that corriders are connected 
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }
        return cor;
    }

    public List<Vector2Int> IncreaseCorridorSizeByThree(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCor = new List<Vector2Int>();
        for (int i = 1; i < corridor.Count; i++)
            for (int x = -1; x < 2; x++)
                for (int y = -1; y < 2; y++)
                    newCor.Add(corridor[i - 1] + new Vector2Int(x, y));
        return newCor;
    }

    //OLD METHOD OF EXPANDING CORRIDOR SIZE
    /*
    public List<Vector2Int> IncreaseCorridorSizeByOne(List<Vector2Int> corridor)
    {
        //this will have duplicate values, the original corridor values + the new neighboring one

        List<Vector2Int> newCor = new List<Vector2Int>();
        Vector2Int prevDirection = Vector2Int.zero;
        //starting from 2nd pos in the list, as subtracting the 2nd from the 1st gets the direction 
        for (int i = 1; i < corridor.Count; i++)
        {
            Vector2Int directionFromCell = corridor[i] - corridor[i - 1];
            if (prevDirection != Vector2Int.zero && directionFromCell != prevDirection)
            {
                //for handling corners, making a 3 by 3
                for (int x = -1; x < 2; x++)
                    for (int y = -1; y < 2; y++)
                        newCor.Add(corridor[i - 1] + new Vector2Int(x, y));
                prevDirection = directionFromCell;
            }
            else
            {
                //if not a corner, add a new single cell in the direction + 90 degrees 
                Vector2Int newCorTileOffset = GetDirection90From(directionFromCell);
                newCor.Add(corridor[i - 1]);
                newCor.Add(corridor[i - 1] + newCorTileOffset);
                prevDirection = directionFromCell;
            }
        }
        return newCor;
    }

    private Vector2Int GetDirection90From(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
            return Vector2Int.right;  //90 degrees clockwise
        if (direction == Vector2Int.right)
            return Vector2Int.down;
        if (direction == Vector2Int.down)
            return Vector2Int.left;
        if (direction == Vector2Int.left)
            return Vector2Int.up;
        return Vector2Int.zero; //handle unexpected input
    }
    */



}