using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap;
    [SerializeField]
    private TileBase floorTile, wallTop, wallSideRight, WallSideLeft, wallBottom, wallFull,
        wallInnerCornerDownLeft, wallInnerCornerDownRight,
        wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft, waterTopLeft, waterTopRight, testTile;

    public List<Vector2Int> PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        return PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    private List<Vector2Int> PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        List<Vector2Int> enemyPos = new List<Vector2Int>();
        foreach (var position in positions)
        {
            if (tile.Equals(floorTile))
            {
                float randomValue = Random.value;

                // Check if the random value is less than or equal to 0.02 (2% chance)
                if (randomValue <= 0.02f)
                {
                    enemyPos.Add(position);
                    //PaintSingleTile(tilemap, enemy, position);
                }
                PaintSingleTile(tilemap, tile, position);
            }
            else
                PaintSingleTile(tilemap, tile, position);
        }
        return enemyPos;
    }


    internal Vector2Int PaintSingleBasicWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        Vector2Int pos = Vector2Int.zero;

        if (WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile = wallTop;
            pos = position;
        }
        else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;
            pos = position;
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = WallSideLeft;
            pos = position;
        }
        else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }

        if (tile != null)
            PaintSingleTile(wallTilemap, tile, position);

        return pos;
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        if (tilemap.name == "Grass Wall" && tile == wallFull)
        {
            PaintSingleTile(floorTilemap, floorTile, position);
            return;
        }

        Vector3Int newPos = new Vector3Int(position.x, position.y, 0);
        var tilePosition = tilemap.WorldToCell(newPos);
        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    internal Vector2Int PaintSingleCornerWall(Vector2Int position, string binaryType)
    {
        int typeASInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        Vector2Int pos = Vector2Int.zero;

        if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeASInt))
        {
            tile = wallInnerCornerDownLeft;
        }
        else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeASInt))
        {
            tile = wallInnerCornerDownRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeASInt))
        {
            tile = wallDiagonalCornerDownLeft;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeASInt))
        {
            tile = wallDiagonalCornerDownRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeASInt))
        {
            tile = wallDiagonalCornerUpRight;
            pos = position;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeASInt))
        {
            tile = wallDiagonalCornerUpLeft;
            pos = position;
        }
        else if (WallTypesHelper.wallFullEightDirections.Contains(typeASInt))
        {
            tile = wallFull;
        }
        else if (WallTypesHelper.wallBottmEightDirections.Contains(typeASInt))
        {
            tile = wallBottom;
        }

        if (tile != null)
            PaintSingleTile(wallTilemap, tile, position);

        return pos;
    }

    public void fixGrassWalls(Vector2Int position)
    {
        Tilemap tilemap = wallTilemap;
        Vector3Int tilePosition = new Vector3Int(position.x, position.y, 0);
        TileBase tile = tilemap.GetTile(tilePosition);

        if (tile == wallTop)
        {
            Vector3Int abovePos = new Vector3Int(position.x, position.y + 1, 0);
            var aboveTilePosition = tilemap.WorldToCell(abovePos);
            var leftPos = new Vector3Int(position.x - 1, position.y, 0);
            var leftTilePosition = tilemap.WorldToCell(leftPos);
            var rightPos = new Vector3Int(position.x + 1, position.y, 0);
            var rightTilePosition = tilemap.WorldToCell(rightPos);

            var aboveFloorTilePosition = floorTilemap.WorldToCell(abovePos);


            if ((wallTilemap.GetTile(leftTilePosition) == wallTop || wallTilemap.GetTile(leftTilePosition) == wallDiagonalCornerUpLeft || wallTilemap.GetTile(leftTilePosition) == waterTopRight) && (wallTilemap.GetTile(aboveTilePosition) == WallSideLeft || wallTilemap.GetTile(aboveTilePosition) == wallInnerCornerDownLeft || wallTilemap.GetTile(aboveTilePosition) == wallDiagonalCornerUpLeft))
            {
                //Debug.Log("Painting waterTopLeft at position " + position);
                PaintSingleTile(tilemap, waterTopLeft, position);
            }
            else if ((wallTilemap.GetTile(rightTilePosition) == wallTop || wallTilemap.GetTile(rightTilePosition) == wallDiagonalCornerUpRight || wallTilemap.GetTile(rightTilePosition) == waterTopLeft) && (wallTilemap.GetTile(aboveTilePosition) == wallSideRight || wallTilemap.GetTile(aboveTilePosition) == wallInnerCornerDownRight || wallTilemap.GetTile(aboveTilePosition) == wallDiagonalCornerUpRight))
            {
                //Debug.Log("Painting waterTopRight at position " + position);
                PaintSingleTile(tilemap, waterTopRight, position);
            }
            else if (floorTilemap.GetTile(aboveFloorTilePosition) == floorTile)
            {
                //Debug.Log("Deleting wallTop at position " + position);
                tilemap.SetTile(tilePosition, null);
                PaintSingleTile(floorTilemap, floorTile, position);
                //fixWallTop(position);
            }
        }
        else if (tile == wallDiagonalCornerUpLeft)
        {
            //Debug.Log("The tile at position " + position + " is a WallDiagCornerUpLeft tile.");

            var rightPos = new Vector3Int(position.x + 1, position.y, 0);
            var rightTilePosition = tilemap.WorldToCell(rightPos);

            var abovePos = new Vector3Int(position.x, position.y + 1, 0);
            var aboveTilePosition = tilemap.WorldToCell(abovePos);

            if (wallTilemap.GetTile(rightTilePosition) != wallTop && wallTilemap.GetTile(rightTilePosition) != waterTopLeft && wallTilemap.GetTile(aboveTilePosition) != null)
            {
                //Debug.Log("Deleting wallDiagonalCornerUpLeft at position " + position);
                tilemap.SetTile(tilePosition, null);
                PaintSingleTile(wallTilemap, WallSideLeft, position);
            }
        }
        else if (tile == wallDiagonalCornerUpRight)
        {
            //Debug.Log("The tile at position " + position + " is a WallDiagCornerUpRight tile.");

            var leftPos = new Vector3Int(position.x - 1, position.y, 0);
            var leftTilePosition = tilemap.WorldToCell(leftPos);

            var abovePos = new Vector3Int(position.x, position.y + 1, 0);
            var aboveTilePosition = tilemap.WorldToCell(abovePos);

            if (wallTilemap.GetTile(leftTilePosition) != wallTop && wallTilemap.GetTile(leftTilePosition) != waterTopRight && wallTilemap.GetTile(aboveTilePosition) != null)
            {
                //Debug.Log("Deleting wallDiagonalCornerUpRight at position " + position);
                tilemap.SetTile(tilePosition, null);
                PaintSingleTile(wallTilemap, wallSideRight, position);
            }
        }
        else if (tile = WallSideLeft)
        {
            var leftPos = new Vector3Int(position.x - 1, position.y, 0);
            var leftTilePosition = tilemap.WorldToCell(leftPos);

            if (wallTilemap.GetTile(leftTilePosition) == wallTop)
            {
                //Debug.Log("Painting wallDiagonalCornerUpLeft at position " + position);
                PaintSingleTile(wallTilemap, waterTopLeft, position);
            }
        }
        else if (tile = wallSideRight)
        {
            var rightPos = new Vector3Int(position.x + 1, position.y, 0);
            var rightTilePosition = tilemap.WorldToCell(rightPos);

            if (wallTilemap.GetTile(rightTilePosition) == wallTop)
            {
                //Debug.Log("Painting wallDiagonalCornerUpLeft at position " + position);
                PaintSingleTile(wallTilemap, waterTopRight, position);
            }
        }
        else
        {
            //Debug.Log("The tile at position " + position + " is a " + tile + " tile.");
        }
    }

    public void fixWallTop(Vector2Int position)
    {
        Tilemap tilemap = wallTilemap;
        Vector3Int tilePosition = new Vector3Int(position.x, position.y, 0);
        TileBase tile = tilemap.GetTile(tilePosition);

        if (tile == wallTop)
        {
            Vector3Int abovePos = new Vector3Int(position.x, position.y + 1, 0);
            var aboveTilePosition = tilemap.WorldToCell(abovePos);

            var leftPos = new Vector3Int(position.x - 1, position.y, 0);
            var leftTilePosition = tilemap.WorldToCell(leftPos);

            var rightPos = new Vector3Int(position.x + 1, position.y, 0);
            var rightTilePosition = tilemap.WorldToCell(rightPos);

            var leftLeftPos = new Vector3Int(position.x - 2, position.y, 0);
            var leftLeftTilePosition = tilemap.WorldToCell(leftLeftPos);

            var rightRightPos = new Vector3Int(position.x + 2, position.y, 0);
            var rightRightTilePosition = tilemap.WorldToCell(rightRightPos);

            var aboveLeftPos = new Vector3Int(position.x - 1, position.y + 1, 0);
            var aboveLeftTilePosition = tilemap.WorldToCell(aboveLeftPos);

            var aboveFloorTilePosition = floorTilemap.WorldToCell(abovePos);

            if ((wallTilemap.GetTile(leftLeftTilePosition) == wallTop && wallTilemap.GetTile(leftTilePosition) == null) || wallTilemap.GetTile(rightRightTilePosition) == wallTop && wallTilemap.GetTile(rightTilePosition) == null)
            {
                if (wallTilemap.GetTile(leftTilePosition) == null)
                {
                    tilemap.SetTile(leftTilePosition, wallTop);
                }
                else
                {
                    tilemap.SetTile(rightTilePosition, wallTop);
                }
            }
            else if (wallTilemap.GetTile(aboveTilePosition) == wallInnerCornerDownLeft && wallTilemap.GetTile(aboveLeftTilePosition) == wallInnerCornerDownRight)
            {
                PaintSingleTile(tilemap, waterTopLeft, position);
                PaintSingleTile(tilemap, waterTopRight, (Vector2Int)leftPos);
            }
            else if ((wallTilemap.GetTile(leftTilePosition) == wallTop || wallTilemap.GetTile(leftTilePosition) == wallDiagonalCornerUpLeft || wallTilemap.GetTile(leftTilePosition) == waterTopRight) && (wallTilemap.GetTile(aboveTilePosition) == WallSideLeft || wallTilemap.GetTile(aboveTilePosition) == wallInnerCornerDownLeft || wallTilemap.GetTile(aboveTilePosition) == wallDiagonalCornerUpLeft))
            {
                //Debug.Log("Painting waterTopLeft at position " + position);
                PaintSingleTile(tilemap, waterTopLeft, position);
            }
            else if ((wallTilemap.GetTile(rightTilePosition) == wallTop || wallTilemap.GetTile(rightTilePosition) == wallDiagonalCornerUpRight || wallTilemap.GetTile(rightTilePosition) == waterTopLeft) && (wallTilemap.GetTile(aboveTilePosition) == wallSideRight || wallTilemap.GetTile(aboveTilePosition) == wallInnerCornerDownRight || wallTilemap.GetTile(aboveTilePosition) == wallDiagonalCornerUpRight))
            {
                //Debug.Log("Painting waterTopRight at position " + position);
                PaintSingleTile(tilemap, waterTopRight, position);
            }
            else if (floorTilemap.GetTile(aboveFloorTilePosition) == floorTile)
            {
                //Debug.Log("Deleting wallTop at position " + position);
                tilemap.SetTile(tilePosition, null);
                PaintSingleTile(floorTilemap, floorTile, position);
                //fixWallTop(position);
            }

            // if (wallTilemap.GetTile(aboveTilePosition) == WallSideLeft)
            // {
            //     //Debug.Log("Painting wallDiagonalCornerUpLeft at position " + position);
            //     PaintSingleTile(wallTilemap, waterTopLeft, position);
            // }
            // else if (wallTilemap.GetTile(aboveTilePosition) == wallSideRight)
            // {
            //     //Debug.Log("Painting wallDiagonalCornerUpLeft at position " + position);
            //     PaintSingleTile(wallTilemap, waterTopRight, position);
            // }
        }
        else if (tile == wallSideRight)
        {
            var leftPos = new Vector3Int(position.x - 1, position.y, 0);
            var leftTilePosition = tilemap.WorldToCell(leftPos);

            var rightPos = new Vector3Int(position.x + 1, position.y, 0);
            var rightTilePosition = tilemap.WorldToCell(rightPos);

            var upPos = new Vector3Int(position.x, position.y + 1, 0);
            var upTilePosition = tilemap.WorldToCell(upPos);

            var downPos = new Vector3Int(position.x, position.y - 1, 0);
            var downTilePosition = tilemap.WorldToCell(downPos);

            var topLeftPos = new Vector3Int(position.x - 1, position.y + 1, 0);
            var topLeftTilePosition = tilemap.WorldToCell(topLeftPos);

            var topRightPos = new Vector3Int(position.x + 1, position.y + 1, 0);
            var topRightTilePosition = tilemap.WorldToCell(topRightPos);

            var bottomLeftPos = new Vector3Int(position.x - 1, position.y - 1, 0);
            var bottomLeftTilePosition = tilemap.WorldToCell(bottomLeftPos);

            var bottomRightPos = new Vector3Int(position.x + 1, position.y - 1, 0);
            var bottomRightTilePosition = tilemap.WorldToCell(bottomRightPos);

            if (tilemap.GetTile(leftTilePosition) == null &&
                tilemap.GetTile(rightTilePosition) == null &&
                (tilemap.GetTile(upTilePosition) == null || tilemap.GetTile(upTilePosition) != wallSideRight) &&
                (tilemap.GetTile(downTilePosition) == null || tilemap.GetTile(downTilePosition) != wallSideRight) &&
                tilemap.GetTile(topLeftTilePosition) == null &&
                tilemap.GetTile(topRightTilePosition) == null &&
                tilemap.GetTile(bottomLeftTilePosition) == null &&
                tilemap.GetTile(bottomRightTilePosition) == null)
            {
                tilemap.SetTile(tilePosition, null);
                PaintSingleTile(floorTilemap, floorTile, position);
            }
            else if (wallTilemap.GetTile(rightTilePosition) == wallTop)
            {
                //Debug.Log("Painting wallDiagonalCornerUpLeft at position " + position);
                PaintSingleTile(wallTilemap, waterTopRight, position);
            }
        }
        else if (tile == WallSideLeft)
        {
            var leftPos = new Vector3Int(position.x - 1, position.y, 0);
            var leftTilePosition = tilemap.WorldToCell(leftPos);

            var rightPos = new Vector3Int(position.x + 1, position.y, 0);
            var rightTilePosition = tilemap.WorldToCell(rightPos);

            var upPos = new Vector3Int(position.x, position.y + 1, 0);
            var upTilePosition = tilemap.WorldToCell(upPos);

            var downPos = new Vector3Int(position.x, position.y - 1, 0);
            var downTilePosition = tilemap.WorldToCell(downPos);

            var topLeftPos = new Vector3Int(position.x - 1, position.y + 1, 0);
            var topLeftTilePosition = tilemap.WorldToCell(topLeftPos);

            var topRightPos = new Vector3Int(position.x + 1, position.y + 1, 0);
            var topRightTilePosition = tilemap.WorldToCell(topRightPos);

            var bottomLeftPos = new Vector3Int(position.x - 1, position.y - 1, 0);
            var bottomLeftTilePosition = tilemap.WorldToCell(bottomLeftPos);

            var bottomRightPos = new Vector3Int(position.x + 1, position.y - 1, 0);
            var bottomRightTilePosition = tilemap.WorldToCell(bottomRightPos);

            if (tilemap.GetTile(leftTilePosition) == null &&
                tilemap.GetTile(rightTilePosition) == null &&
                (tilemap.GetTile(upTilePosition) == null || tilemap.GetTile(upTilePosition) != WallSideLeft) &&
                (tilemap.GetTile(downTilePosition) == null || tilemap.GetTile(downTilePosition) != WallSideLeft) &&
                tilemap.GetTile(topLeftTilePosition) == null &&
                tilemap.GetTile(topRightTilePosition) == null &&
                tilemap.GetTile(bottomLeftTilePosition) == null &&
                tilemap.GetTile(bottomRightTilePosition) == null)
            {
                tilemap.SetTile(tilePosition, null);
                PaintSingleTile(floorTilemap, floorTile, position);
            }
            else if (wallTilemap.GetTile(leftTilePosition) == wallTop)
            {
                //Debug.Log("Painting wallDiagonalCornerUpLeft at position " + position);
                PaintSingleTile(wallTilemap, waterTopLeft, position);
            }
        }
    }
}