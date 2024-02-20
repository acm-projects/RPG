using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        CreateCorridors(floorPositions, potentialRoomPositions);

        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        floorPositions.UnionWith(roomPositions);

        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGen.CreateWalls(floorPositions, tilemapVisualizer);

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

    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition); //at each end of a corridor a room gens 

        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength); //will generate corrdier pos
            currentPosition = corridor[corridor.Count - 1]; //setting current pos to last pos so that corriders are connected 
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }
    }
}