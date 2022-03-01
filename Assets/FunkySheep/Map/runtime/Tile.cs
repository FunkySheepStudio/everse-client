using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Map
{
    public class Tile
    {
        public Vector3Int tilemapPosition;
        public Vector2Int mapPosition;
        public UnityEngine.Tilemaps.Tile data;
        public Tile(Vector3Int tilemapPosition, Vector2Int mapPosition, UnityEngine.Tilemaps.Tile data)
        {
        this.tilemapPosition = tilemapPosition;
        this.mapPosition = mapPosition;
        this.data = data;
        }
    }    
}
