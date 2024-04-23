using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGen
{
    public static List<Vector2Int> CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer, LevelType levelType)
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionsList);
        var cornerWallPositions = FindWallsInDirections(floorPositions, Direction2D.diagonalDirectionsList);
        List<Vector2Int> wallTopPos = CreateBasicWall(tilemapVisualizer, basicWallPositions, floorPositions);
        List<Vector2Int> wallLeft = CreateCornerWalls(tilemapVisualizer, cornerWallPositions, floorPositions);

        List<Vector2Int> allWallPositions = new List<Vector2Int>();
        allWallPositions.AddRange(basicWallPositions);
        allWallPositions.AddRange(cornerWallPositions);

        Debug.Log(levelType);

        if (levelType.Equals(LevelType.Overworld))
        {
            List<Vector2Int> wallProblemsCombined = new List<Vector2Int>();
            wallProblemsCombined.AddRange(wallTopPos);
            wallProblemsCombined.AddRange(wallLeft);
            foreach (var wallPosition in wallProblemsCombined)
            {
                tilemapVisualizer.fixGrassWalls(wallPosition);
            }
            foreach (var wallPosition in wallTopPos)
            {
                tilemapVisualizer.fixWallTop(wallPosition);
            }
        }
        return allWallPositions;
    }

    private static List<Vector2Int> CreateCornerWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> wallInnerCornerLeftPos = new List<Vector2Int>();
        foreach (var position in cornerWallPositions)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.eightDirectionsList)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            Vector2Int newPos = tilemapVisualizer.PaintSingleCornerWall(position, neighboursBinaryType);
            if (newPos != Vector2Int.zero)
            {
                wallInnerCornerLeftPos.Add(newPos);
            }
        }
        return wallInnerCornerLeftPos;
    }

    private static List<Vector2Int> CreateBasicWall(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> basicWallPositions, HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> wallTopPos = new List<Vector2Int>();
        foreach (var position in basicWallPositions)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            Vector2Int newPos = tilemapVisualizer.PaintSingleBasicWall(position, neighboursBinaryType);
            if (newPos != Vector2Int.zero)
            {
                wallTopPos.Add(newPos);
            }
        }
        return wallTopPos;
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        //QUICK EXPLINATION OF LOGIC FOR CHECKING FOR WALLS:
        //checking to see if the position next to a floor tile is a floor, if it is not a floor (meaning nothing is there), then that means the current tile is supposed to be a wall tile

        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition) == false)
                    wallPositions.Add(neighbourPosition);
            }
        }
        return wallPositions;
    }
}
