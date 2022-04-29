using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Tiles
{
    [AddComponentMenu("FunkySheep/Tiles/Tiles Manager")]
    public class Manager : MonoBehaviour
    {
        public Tile initialTile;
        public FunkySheep.Types.Vector2 initialOffset;
        public FunkySheep.Types.Float tileSize;
        public List<Tile> tiles = new List<Tile>();

        public Tile Add(Vector2Int position)
        {
            Tile tile = this.Get(position);
            if (tile == null)
            {
                tile = new Tile(position);
                tiles.Add(tile);
            }

            if (tiles.Count == 1)
            {
                initialTile = tile;
            }

            return tile;
        }

        public Tile Get(Vector2Int position)
        {
            return tiles.Find(tile => tile.position == position);
        }

        public bool Exist(Vector2Int position)
        {
            Tile tile = this.Get(position);
            if (tile == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Position relative to the grid with the offset
        /// </summary>
        /// <param name="position">World position</param>
        /// <returns></returns>
        public Vector2 RelativePosition(Vector2 position)
        {
            Vector2 relativePosition = position - tileSize.value * initialOffset.value;
            return relativePosition;
        }

        /// <summary>
        /// Tile position in a given world position
        /// </summary>
        /// <param name="position">World position</param>
        /// <returns></returns>
        public Vector2Int TilePosition(Vector2 position)
        {
            Vector2 relativePosition = RelativePosition(position);
            Vector2Int tilePosition = new Vector2Int(
              Mathf.FloorToInt(relativePosition.x / tileSize.value),
              Mathf.FloorToInt(relativePosition.y / tileSize.value)
            );

            return tilePosition;
        }

        /// <summary>
        /// Calculate the relative value position inside a tile
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector2 InsideTilePosition(Vector2 position)
        {
            Vector2 relativePosition = RelativePosition(position);
            Vector2Int tilePosition = TilePosition(position);
            Vector2 insideTilePosition = relativePosition -
            new Vector2(
              tilePosition.x * tileSize.value,
              tilePosition.y * tileSize.value
            );

            insideTilePosition /= tileSize.value;

            return insideTilePosition;
        }

        /// <summary>
        /// Calculate the quarter position inside a tile 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector2Int InsideTileQuarterPosition(Vector2 position)
        {
            Vector2 insideTilePosition = InsideTilePosition(position);
            Vector2Int insideTileQuarterPosition = Vector2Int.zero;

            if (insideTilePosition.x >= 0.5f)
            {
                insideTileQuarterPosition.x = 1;
            }
            else
            {
                insideTileQuarterPosition.x = -1;
            }

            if (insideTilePosition.y >= 0.5f)
            {
                insideTileQuarterPosition.y = 1;
            }
            else
            {
                insideTileQuarterPosition.y = -1;
            }

            return insideTileQuarterPosition;
        }

        /// <summary>
        /// Calculate the world offset givent the offset and the size of a tile
        /// </summary>
        /// <returns></returns>
        public Vector2 WorldOffset()
        {
            Vector2 worldOffset = initialOffset.value * tileSize.value;
            return worldOffset;
        }
    }
}
