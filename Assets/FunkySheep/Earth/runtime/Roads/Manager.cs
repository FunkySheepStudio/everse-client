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
        Thread extractOsmThread = new Thread(() => this.ExtractOsmData(pendingTiles.Dequeue()));
        extractOsmThread.Start();
      }
    }

    private void ExtractOsmData(Tile tile)
    {
      try
      {
        FunkySheep.OSM.Data parsedData = FunkySheep.OSM.Parser.Parse(tile.osmFile);
        foreach (FunkySheep.OSM.Way way in parsedData.ways)
        {
          
          for (int i = 1; i < way.nodes.Count; i++)
          {
            FunkySheep.OSM.Node previousNode = way.nodes[i - 1];
            FunkySheep.OSM.Node node = way.nodes[i];

            if (
                previousNode.latitude >= tile.gpsBoundaries[0] && 
                node.latitude >= tile.gpsBoundaries[0] &&
                previousNode.longitude >= tile.gpsBoundaries[1] && 
                node.longitude >= tile.gpsBoundaries[1] &&
                previousNode.latitude <= tile.gpsBoundaries[2] && 
                node.latitude <= tile.gpsBoundaries[2] &&
                previousNode.longitude <= tile.gpsBoundaries[3] && 
                node.longitude <= tile.gpsBoundaries[3]
              )
            {

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
                  500,
                  previousNodePosition.y
                ),
                new Vector3(
                  nodePosition.x,
                  500,
                  nodePosition.y
                ),
                Color.red, 600
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
        }

        ProcessRoads(tile);
      }
      catch (Exception e)
      {
        Debug.Log(e);
      }
    }

    public void ProcessRoads(Tile tile) {
      int size = 10;
      
      foreach (Graphs.Edge<Vector2> edge in tile.graph.edges)
      {
        float diag = Diagolale(edge.verticeA, edge.verticeB);
        /*Vector2 perpPosition = Vector2.Perpendicular(
            edge.verticeB - edge.verticeA
          ).normalized;

        Vector2Int perpPositionInt = new Vector2Int(
          Mathf.RoundToInt(perpPosition.x),
          Mathf.RoundToInt(perpPosition.y)
        );*/

        for (float i = 0; i < diag; i++)
        {
          Vector2Int roundedPosition = GetPosition(i, diag, edge);

          if (i > 0 && i < (diag - 1))
          {
            Vector2Int lastPosition = GetPosition(i - 1, diag, edge);
            Vector2Int nextPosition = GetPosition(i + 1, diag, edge);
            tile.terrainTile.heights[roundedPosition.x, roundedPosition.y] = 
            (tile.terrainTile.heights[lastPosition.x, lastPosition.y] +
            tile.terrainTile.heights[nextPosition.x, nextPosition.y]) / 2; 
            
          }

          for (int x = -size; x <= size; x++)
          {
            for (int y = -size; y <= size; y++)
            {
              if (roundedPosition.x + x > 256 || roundedPosition.x + x < 0)
                break;
              if (roundedPosition.y + y > 256 || roundedPosition.y + y < 0)
                break;

              int distance = Mathf.Abs(x) + Mathf.Abs(y);
              tile.terrainTile.heights[roundedPosition.x + x, roundedPosition.y + y] = (distance * tile.terrainTile.heights[roundedPosition.x + x, roundedPosition.y + y] + tile.terrainTile.heights[roundedPosition.x, roundedPosition.y]) / (distance + 1);
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
    }

    public void AddTile(Terrain.Tile terrainTile)
    {
      double[] gpsBoundaries = FunkySheep.Earth.Map.Utils.CaclulateGpsBoundaries(earthManager.zoomLevel.value, terrainTile.position);
      string interpolatedUrl = InterpolatedUrl(gpsBoundaries);

      StartCoroutine(FunkySheep.Network.Downloader.Download(interpolatedUrl, (fileID, file) => {
        Tile roadTile = new Tile(terrainTile.position);
        roadTile.gpsBoundaries = gpsBoundaries;
        roadTile.terrainTile = terrainTile;
        roadTile.osmFile = file;
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
