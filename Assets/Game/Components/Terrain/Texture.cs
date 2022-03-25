using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Terrain
{
  public class Texture : MonoBehaviour
  {
    public void AddTexture(FunkySheep.Earth.Map.Tile tile)
    {
      for (int i = 0; i < UnityEngine.Terrain.activeTerrains.Length; i++)
      {
        Vector2Int terrainPos = UnityEngine.Terrain.activeTerrains[i].GetComponent<FunkySheep.Earth.Terrain.Tile>().position;
        if (terrainPos == tile.mapPosition)
        {
          TerrainLayer[] layers = new TerrainLayer[1];
          layers[0] = new TerrainLayer();
          layers[0].diffuseTexture = tile.data.sprite.texture;
          layers[0].tileSize = new Vector2(
            UnityEngine.Terrain.activeTerrains[i].terrainData.size.x,
            UnityEngine.Terrain.activeTerrains[i].terrainData.size.z
          );
          UnityEngine.Terrain.activeTerrains[i].terrainData.terrainLayers = layers;
        }
      }
    }
  }
}
