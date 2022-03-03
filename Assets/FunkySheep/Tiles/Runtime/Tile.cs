using System;
using UnityEngine;

namespace FunkySheep.Tiles
{
  [Serializable]
  public class Tile
  {
    public Vector2Int position;
    public Tile(Vector2Int position)
    {
      this.position = position;
    }
}    
}
