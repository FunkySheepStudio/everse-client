using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Earth.Terrain
{
  public class Connector : MonoBehaviour
  {
    public UnityEngine.Terrain terrain;
    public UnityEngine.TerrainData terrainData;

    public bool topConnected = false;
    public bool leftConnected = false;
    public bool cornerConnected = false;

    private void Start() {
      terrain = GetComponent<UnityEngine.Terrain>();
      terrainData = terrain.terrainData;
    }

    private void Update() {
      if (terrain.topNeighbor != null && !topConnected)
      {
        ConnectTop(terrain.topNeighbor);
        
      }

      if (terrain.leftNeighbor != null && !leftConnected)
      {
        ConnectLeft(terrain.leftNeighbor);
      }

      if (terrain.leftNeighbor != null && terrain.topNeighbor != null && terrain.leftNeighbor.topNeighbor != null && !cornerConnected)
      {
        ConnectCorners();
        enabled = false;
      }
    }
    
    void ConnectTop(UnityEngine.Terrain top)
    {
      float[,] heights = terrainData.GetHeights(0, terrainData.heightmapResolution - 1, terrainData.heightmapResolution, 1);
      float[,] heightsTop = top.terrainData.GetHeights(0, 0, terrainData.heightmapResolution, 1);
      float[,] heightsNew = heights;

      for (int y = 0; y < terrainData.heightmapResolution; y++)
      {
        heightsNew[0, y] = (heights[0, y] + heightsTop[0, y]) / 2;
      }

      terrainData.SetHeights(0, top.terrainData.heightmapResolution - 1, heightsNew);
      top.terrainData.SetHeights(0, 0, heightsNew);

      topConnected = true;
    }

    void ConnectLeft(UnityEngine.Terrain left)
    {
      float[,] heights = terrainData.GetHeights(0, 0, 1, terrainData.heightmapResolution);
      float[,] heightsLeft = left.terrainData.GetHeights(terrainData.heightmapResolution - 1, 0, 1, terrainData.heightmapResolution);
      float[,] heightsNew = heights;

      for (int x = 0; x < terrainData.heightmapResolution; x++)
      {
        heightsNew[x, 0] = (heights[x, 0] + heightsLeft[x, 0]) / 2;
      }

      terrainData.SetHeights(0, 0, heightsNew);
      left.terrainData.SetHeights(left.terrainData.heightmapResolution - 1, 0, heightsNew);

      leftConnected = true;
    }

    void ConnectCorners()
    {
      float[,] heights = terrainData.GetHeights(0, terrainData.heightmapResolution - 1, 1, 1);
      float[,] heightsLeft = terrain.leftNeighbor.terrainData.GetHeights(terrainData.heightmapResolution - 1, terrainData.heightmapResolution - 1, 1, 1);
      float[,] heightsTop = terrain.topNeighbor.terrainData.GetHeights(0, 0, 1, 1);
      float[,] heightsLeftTop = terrain.topNeighbor.leftNeighbor.terrainData.GetHeights(terrainData.heightmapResolution - 1, 0, 1, 1);

      float[,] heightsNew = heights;

      heightsNew[0, 0] = (heights[0, 0] + heightsLeft[0, 0] + + heightsTop[0, 0] + + heightsLeftTop[0, 0]) / 4;
      
      terrainData.SetHeights(0, terrainData.heightmapResolution - 1, heights);
      terrain.leftNeighbor.terrainData.SetHeights(terrainData.heightmapResolution - 1, terrainData.heightmapResolution - 1, heights);
      terrain.topNeighbor.terrainData.SetHeights(0, 0, heights);
      terrain.topNeighbor.leftNeighbor.terrainData.SetHeights(terrainData.heightmapResolution - 1, 0 , heights);
      cornerConnected = true;
    }
  }
}