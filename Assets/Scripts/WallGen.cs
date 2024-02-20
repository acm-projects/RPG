using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGen
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionsList, tilemapVisualizer);
        foreach (var position in basicWallPositions)
        {
            tilemapVisualizer.PaintSingleBasicWall(position);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList, TilemapVisualizer tilemapVisualizer)
    {
        //QUICK EXPLINATION OF LOGIC FOR CHECKING FOR WALLS:
        //checking to see if the position next to a floor tile is a floor, if it is not a floor (meaning nothing is there), then that means the current tile is supposed to be a wall tile

        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition) == false && floorPositions.Contains(neighbourPosition + direction) == false)
                    wallPositions.Add(neighbourPosition);
                else
                    tilemapVisualizer.PaintSingleFloorTile(neighbourPosition);
                //logic for covering empty spots with floor instead of wall

                //TODO FIX THIS LOGIC, BECAUSE HOLE GAP COULD BE MORE THAN 3 OR SOMETHING
            }
        }
        return wallPositions;
    }
}
