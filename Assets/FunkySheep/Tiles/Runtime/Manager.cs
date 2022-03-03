using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Tiles
{
    [AddComponentMenu("FunkySheep/Tiles/Tiles Manager")]
    public class Manager : MonoBehaviour
    {
        public List<Tile> tiles = new List<Tile>();

        public Tile Add(Vector2Int position)
        {
            Tile tile = this.Get(position);
            if (tile == null)
            {
                tile = new Tile(position);
                tiles.Add(tile);
            }
            return tile;
        }

        public Tile Get(Vector2Int position)
        {
            return tiles.Find(tile => tile.position == position);
        }
    }
}
