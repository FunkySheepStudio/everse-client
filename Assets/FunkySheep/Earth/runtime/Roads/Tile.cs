using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Earth.Roads
{
  public class Tile : FunkySheep.Tiles.Tile
  {
    public Terrain.Tile terrainTile;
    public byte[] osmFile;
    public double[] gpsBoundaries;

    public Graphs.Graph<Vector2> graph = new Graphs.Graph<Vector2>();
    public Tile(Vector2Int postition) : base(postition)
    {
    }
  }
}
