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
    public FunkySheep.Graphs.Vector2.Manager graphManager;
    public List<Road> roads = new List<Road>();
    Queue<Tile> pendingTiles = new Queue<Tile>();

    private void Awake() {
      graphManager = GetComponent<FunkySheep.Graphs.Vector2.Manager>();
    }

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
          Road road = this.roads.Find(road => road.id == way.id);
          if (road == null)
          {
            road = new Road(way.id);

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

                tile.graph.edges.Add(
                  new Graphs.Edge<Vector2>(previousNodePosition, nodePosition)
                );

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
                  Color.red, 600);
              }

            }
            roads.Add(road);
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
      int size = 3;

      foreach (Graphs.Edge<Vector2> edge in tile.graph.edges)
      {
        float diag = Diagolale(edge.verticeA, edge.verticeB);
        Vector2 perpPosition = Vector2.Perpendicular(
            edge.verticeB - edge.verticeA
          ).normalized;

        Vector2Int perpPositionInt = new Vector2Int(
          Mathf.RoundToInt(perpPosition.x),
          Mathf.RoundToInt(perpPosition.y)
        );

        for (float i = 0; i < diag; i++)
        {
          Vector2Int roundedPosition = GetPosition(i, diag, edge);

          if (i > 0)
          {
            // Smooth the road on it's axis
            tile.terrainTile.heights[roundedPosition.x, roundedPosition.y] =
            (
              tile.terrainTile.heights[GetPosition(i - 1, diag, edge).x, GetPosition(i - 1, diag, edge).y] +
              tile.terrainTile.heights[GetPosition(i + 1, diag, edge).x, GetPosition(i + 1, diag, edge).y]
            ) / 2;
          }

          for (int j = 1; j <= size; j++)
          {
            Vector2Int nextSizeLeft = roundedPosition + perpPositionInt * j;
            Vector2Int nextSizeRight = roundedPosition - perpPositionInt * j;
            tile.terrainTile.heights[nextSizeLeft.x, nextSizeLeft.y] = tile.terrainTile.heights[roundedPosition.x, roundedPosition.y];
            tile.terrainTile.heights[nextSizeRight.x, nextSizeRight.y] = tile.terrainTile.heights[roundedPosition.x, roundedPosition.y];


            // Handle the case where the offset is (1, 1) in absolute so we set the intermediates values
            if (i > 0 && Mathf.Abs(perpPositionInt.x) == Mathf.Abs(perpPositionInt.y))
            {
              Vector2Int lastPosition = GetPosition(i - 1, diag, edge);

              float mediumHeight = (tile.terrainTile.heights[roundedPosition.x, roundedPosition.y] + tile.terrainTile.heights[lastPosition.x, lastPosition.y]) / 2;
              tile.terrainTile.heights[nextSizeLeft.x - 1, nextSizeLeft.y] =  mediumHeight;
              tile.terrainTile.heights[nextSizeRight.x - 1, nextSizeRight.y] =  mediumHeight;

              tile.terrainTile.heights[nextSizeLeft.x, nextSizeLeft.y - 1] =  mediumHeight;
              tile.terrainTile.heights[nextSizeRight.x, nextSizeRight.y - 1] =  mediumHeight;
            }
          }
        }
      }
      tile.terrainTile.terrain.terrainData.SetHeightsDelayLOD(0, 0, tile.terrainTile.heights);
      tile.terrainTile.terrain.terrainData.SyncHeightmap();
    }

    public Vector2Int GetPosition(float index, float diag, Graphs.Edge<Vector2> edge)
    {
      float t = diag == 0 ? 0f : index / diag;
      Vector2 position = Vector2.Lerp(edge.verticeA, edge.verticeB, t);

      return new Vector2Int(
        Mathf.RoundToInt(position.x),
        Mathf.RoundToInt(position.y)
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
