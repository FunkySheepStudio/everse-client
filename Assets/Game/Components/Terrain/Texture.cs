using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Terrain
{
  public class Texture : MonoBehaviour
  {
    List<FunkySheep.Earth.Map.Tile> tiles = new List<FunkySheep.Earth.Map.Tile>();

    private void LateUpdate() {
      foreach (FunkySheep.Earth.Map.Tile tile in tiles.ToArray())
      {
        for (int i = 0; i < UnityEngine.Terrain.activeTerrains.Length; i++)
        {
          Vector2Int terrainPos = UnityEngine.Terrain.activeTerrains[i].GetComponent<FunkySheep.Earth.Terrain.Tile>().position;
          if (tile.mapPosition == terrainPos)
          {
            UnityEngine.Terrain.activeTerrains[i].materialTemplate.SetTexture("diffuse", tile.data.sprite.texture);
            tiles.Remove(tile);
          }
        }
      }
    }


    public void AddTexture(FunkySheep.Earth.Map.Tile tile)
    {
      tiles.Add(tile);
    }
  }
}
