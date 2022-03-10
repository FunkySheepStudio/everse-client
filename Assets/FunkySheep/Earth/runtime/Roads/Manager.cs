using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Earth.Roads
{
  public class Manager : MonoBehaviour
  {
    public FunkySheep.Types.String url;
    public FunkySheep.Types.Int32 terrainTextureResolution;
    public Earth.Manager earthManager;
    Queue<Tile> pendingTiles = new Queue<Tile>();

    private void Update() {
      if (pendingTiles.Count != 0)
      {
        Thread extractOsmThread = new Thread(() => pendingTiles.Dequeue().ExtractOsmData());
        extractOsmThread.Start();
      }
    }

    /*private void ExtractOsmData(Tile tile)
    {
      try
      {
        FunkySheep.OSM.Data parsedData = FunkySheep.OSM.Parser.Parse(tile.osmFile);
        
        System.Random rand = new System.Random(parsedData.ways.Count);
        Color mainColor = new Color(
          (float)rand.NextDouble(),
          (float)rand.NextDouble(),
          (float)rand.NextDouble()
        );

        foreach (FunkySheep.OSM.Way way in parsedData.ways)
        {
          
          for (int i = 1; i < way.nodes.Count; i++)
          {
            Color debugColor = mainColor;

            FunkySheep.OSM.Node previousNode = way.nodes[i - 1];
            FunkySheep.OSM.Node node = way.nodes[i];

            if (!IsInsideBoundaries(previousNode, tile) && !IsInsideBoundaries(node, tile))
            {
              debugColor = Color.red;
              //break;
            } else if (!IsInsideBoundaries(previousNode, tile))
            {
              previousNode = SetGpsLimits(previousNode, node, tile);
              debugColor = Color.green;
            } else if (!IsInsideBoundaries(node, tile))
            {
              node = SetGpsLimits(node, previousNode, tile);
              debugColor = Color.green;
            }

            Vector2 previousNodePosition = FunkySheep.Earth.Map.Utils.GpsToMapReal(
            earthManager.zoomLevel.value,
            previousNode.latitude,
            previousNode.longitude
            ) - earthManager.initialMapPosition.value;

            // Invert Y axis since OSM map y is inverted
            previousNodePosition.y = -previousNodePosition.y;
            previousNodePosition *= earthManager.tilesManager.tileSize.value;

            Vector2 nodePosition = FunkySheep.Earth.Map.Utils.GpsToMapReal(
              earthManager.zoomLevel.value,
              node.latitude,
              node.longitude
            ) - earthManager.initialMapPosition.value;

            // Invert Y axis since OSM map y is inverted
            nodePosition.y = -nodePosition.y;
            nodePosition *= earthManager.tilesManager.tileSize.value;

            Debug.DrawLine(
              new Vector3(
                previousNodePosition.x,
                0,
                previousNodePosition.y
              ),
              new Vector3(
                nodePosition.x,
                0,
                nodePosition.y
              ),
              debugColor, 600
            );

            Vector2 previousInsidePos = earthManager.tilesManager.InsideTilePosition(previousNodePosition) * 256;
            Vector2 insidePos = earthManager.tilesManager.InsideTilePosition(nodePosition) * 256;

            tile.graph.edges.Add(
              new Graphs.Edge<Vector2>(
                previousInsidePos,
                insidePos
              )
            );
          }
        }

        ProcessRoads(tile);
      }
      catch (Exception e)
      {
        Debug.Log(e);
      }
    }*/

    /*public bool IsInsideBoundaries(FunkySheep.OSM.Node node, Tile tile)
    {
      if (node.latitude <= tile.gpsBoundaries[0])
        return false;
      if (node.longitude <= tile.gpsBoundaries[1])
        return false;
      if (node.latitude >= tile.gpsBoundaries[2])
        return false;
      if (node.longitude >= tile.gpsBoundaries[3])
        return false;

      return true;
    }

    public bool IsInsideBoundaries(Vector2 node, Tile tile)
    {
      if (node.x <= tile.gpsBoundaries[0])
        return false;
      if (node.y <= tile.gpsBoundaries[1])
        return false;
      if (node.x >= tile.gpsBoundaries[2])
        return false;
      if (node.y >= tile.gpsBoundaries[3])
        return false;

      return true;
    }

    public FunkySheep.OSM.Node SetGpsLimits(FunkySheep.OSM.Node node, FunkySheep.OSM.Node otherNode, Tile tile)
    {
      int[,] boundaries = {
        {2,3,2,1}, //UP
        {0,1,0,3}, // DOWN
        {2,3,0,3}, //RIGHT
        {0,1,2,1}, //LEFT
      };

      Vector2 nodeVector = new Vector2(
            (float)node.latitude,
            (float)node.longitude
          );  
      Vector2 otherNodeVector = new Vector2(
        (float)otherNode.latitude,
        (float)otherNode.longitude
      );


      for (int i = 0; i < 4; i ++)
      {
        bool ok;

        Vector2 startBound = new Vector2(
          (float)tile.gpsBoundaries[boundaries[i, 2]],
          (float)tile.gpsBoundaries[boundaries[i, 3]]
        );

        Vector2 endBound = new Vector2(
          (float)tile.gpsBoundaries[boundaries[i, 0]],
          (float)tile.gpsBoundaries[boundaries[i, 1]]
        );

        Vector2 newGpsPositions = GetIntersectionPointCoordinates(
          nodeVector,
          otherNodeVector,
          startBound,
          endBound,
          out ok
        );

        if (ok)
        {
          float oldDistance = Vector2.Distance(otherNodeVector, nodeVector);
          float newDistance = Vector2.Distance(otherNodeVector, newGpsPositions);

          if (newDistance < oldDistance)
          {
            node.latitude = newGpsPositions.x;
            node.longitude = newGpsPositions.y;
          }
        }
      }

      return node;
    }

  /// <summary>
  /// Gets the coordinates of the intersection point of two lines.
  /// </summary>
  /// <param name="A1">A point on the first line.</param>
  /// <param name="A2">Another point on the first line.</param>
  /// <param name="B1">A point on the second line.</param>
  /// <param name="B2">Another point on the second line.</param>
  /// <param name="found">Is set to false of there are no solution. true otherwise.</param>
  /// <returns>The intersection point coordinates. Returns Vector2.zero if there is no solution.</returns>
  public Vector2 GetIntersectionPointCoordinates(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2, out bool found)
  {
      float tmp = (B2.x - B1.x) * (A2.y - A1.y) - (B2.y - B1.y) * (A2.x - A1.x);
  
      if (tmp == 0)
      {
          // No solution!
          found = false;
          return Vector2.zero;
      }
  
      float mu = ((A1.x - B1.x) * (A2.y - A1.y) - (A1.y - B1.y) * (A2.x - A1.x)) / tmp;
  
      found = true;
  
      return new Vector2(
          B1.x + (B2.x - B1.x) * mu,
          B1.y + (B2.y - B1.y) * mu
      );
    }

    /*public void ProcessRoads(Tile tile) {
      int size = 3;
      float[,] heights = new float[257, 257];
      int[,] weights = new int[257, 257];
      
      foreach (Graphs.Edge<Vector2> edge in tile.graph.edges)
      {
        float diag = Diagolale(edge.verticeA, edge.verticeB);
        
        for (float i = 0; i < diag; i++)
        {
          Vector2Int roundedPosition = GetPosition(i, diag, edge);

          for (int x = -size; x <= size; x++)
          {
            for (int y = -size; y <= size; y++)
            {
              if (roundedPosition.x + x > 256 || roundedPosition.x + x < 0)
                break;
              if (roundedPosition.y + y > 256 || roundedPosition.y + y < 0)
                break;

              if (heights[roundedPosition.x + x, roundedPosition.y + y] == 0)
              {
                heights[roundedPosition.x + x, roundedPosition.y + y] = tile.terrainTile.heights[roundedPosition.x, roundedPosition.y];
                weights[roundedPosition.x + x, roundedPosition.y + y] = 1;
              } else {
                heights[roundedPosition.x + x, roundedPosition.y + y] += tile.terrainTile.heights[roundedPosition.x, roundedPosition.y];
                weights[roundedPosition.x + x, roundedPosition.y + y]++;
              }
            }
          }
        }

        for (int x = 0; x < 257; x++)
        {
          for (int y = 0; y < 257; y++)
          {
            if (heights[x, y] != 0)
            {
              tile.terrainTile.heights[x, y] = heights[x, y] / weights[x, y];
            }
          }
        }
      }
      tile.terrainTile.roadsCalculated = true;
    }

    public Vector2Int GetPosition(float index, float diag, Graphs.Edge<Vector2> edge)
    {
      float t = diag == 0 ? 0f : index / diag;
      Vector2 position = Vector2.Lerp(edge.verticeA, edge.verticeB, t);

      return new Vector2Int(
        Mathf.RoundToInt(position.y),
        Mathf.RoundToInt(position.x)
      );
    }

    public float Diagolale(Vector2 p0, Vector2 p1)
    {
      float dx = p1.x - p0.x;
      float dy = p1.y - p0.y;
      float diagonal = Mathf.Max(Mathf.Abs(dx), Mathf.Abs(dy));
      return diagonal;
    }*/

    public void AddTile(Terrain.Tile terrainTile)
    {
      double[] gpsBoundaries = FunkySheep.Earth.Map.Utils.CaclulateGpsBoundaries(earthManager.zoomLevel.value, terrainTile.position);
      string interpolatedUrl = InterpolatedUrl(gpsBoundaries);

      StartCoroutine(FunkySheep.Network.Downloader.Download(interpolatedUrl, (fileID, file) => {
        Tile roadTile = new Tile(terrainTile.position);
        roadTile.earthManager = earthManager;
        roadTile.SetBoudaries(gpsBoundaries);
        roadTile.SetTerrainTile(terrainTile);
        roadTile.SetOsmFile(file);
        pendingTiles.Enqueue(roadTile);
      }));
    }

    /// <summary>
    /// Interpolate the url inserting the boundaries and the types of OSM data to download
    /// </summary>
    /// <param boundaries="boundaries">The gps boundaries to download in</param>
    /// <returns>The interpolated Url</returns>
    public string InterpolatedUrl(double[] boundaries)
    {
        string [] parameters = new string[5];
        string [] parametersNames = new string[5];

        parameters[0] = boundaries[0].ToString().Replace(',', '.');
        parametersNames[0] = "startLatitude";

        parameters[1] = boundaries[1].ToString().Replace(',', '.');
        parametersNames[1] = "startLongitude";

        parameters[2] = boundaries[2].ToString().Replace(',', '.');
        parametersNames[2] = "endLatitude";

        parameters[3] = boundaries[3].ToString().Replace(',', '.');
        parametersNames[3] = "endLongitude";

        return url.Interpolate(parameters, parametersNames);
    }
  }
}


