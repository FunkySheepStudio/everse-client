using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Earth.Roads
{
  public class Tile : FunkySheep.Tiles.Tile
  {
    public FunkySheep.Earth.Manager earthManager;
    Terrain.Tile terrainTile;
    float terrainResolution;
    byte[] osmFile;
    double[] gpsBoundaries;
    Vector2 worldBoundaryStart;
    Vector2 worldBoundaryEnd;

    public Graphs.Graph<Vector2> graph = new Graphs.Graph<Vector2>();
    public Tile(Vector2Int postition) : base(postition)
    {
    }

    public void SetBoudaries(double[] gpsBoundaries)
    {
      this.gpsBoundaries = gpsBoundaries;

      worldBoundaryStart = FunkySheep.Earth.Map.Utils.GpsToMapReal(
        earthManager.zoomLevel.value,
        this.gpsBoundaries[0],
        this.gpsBoundaries[1]
      ) - earthManager.initialMapPosition.value;

      worldBoundaryStart.y = -worldBoundaryStart.y;
      worldBoundaryStart *= earthManager.tilesManager.tileSize.value;

      worldBoundaryEnd = FunkySheep.Earth.Map.Utils.GpsToMapReal(
        earthManager.zoomLevel.value,
        this.gpsBoundaries[2],
        this.gpsBoundaries[3]
      ) - earthManager.initialMapPosition.value;

      worldBoundaryEnd.y = -worldBoundaryEnd.y;
      worldBoundaryEnd *= earthManager.tilesManager.tileSize.value;
    }

    public void SetTerrainTile(Terrain.Tile terrainTile)
    {
      this.terrainTile = terrainTile;
      terrainResolution = terrainTile.terrain.terrainData.heightmapResolution;
    }

    public void SetOsmFile(byte[] osmFile)
    {
      this.osmFile = osmFile;
    }

    public void ExtractOsmData()
    {
      try
      {
        FunkySheep.OSM.Data parsedData = FunkySheep.OSM.Parser.Parse(osmFile);
        foreach (FunkySheep.OSM.Way way in parsedData.ways)
        {
          
          for (int i = 1; i < way.nodes.Count; i++)
          {
            Node previousNode = new Node(
              way.nodes[i - 1].latitude,
              way.nodes[i - 1].longitude
            );

            Node node = new Node(
              way.nodes[i].latitude,
              way.nodes[i].longitude
            );

            ProcessSegment(previousNode, node);
          }
        }
      }
      catch (Exception e)
      {
        Debug.Log(e);
      }
    }

    public void ProcessSegment(Node previousNode, Node node)
    {
      previousNode.SetWorldPosition(earthManager);
      node.SetWorldPosition(earthManager);
      float step = earthManager.tilesManager.tileSize.value / terrainResolution;

      Vector2 previousNodeGridPosition = new Vector2(
        Mathf.RoundToInt(previousNode.worldPosition.x / step) * step,
        Mathf.RoundToInt(previousNode.worldPosition.y / step) * step
      );

      Vector2 nodeGridPosition = new Vector2(
        Mathf.RoundToInt(node.worldPosition.x / step) * step,
        Mathf.RoundToInt(node.worldPosition.y / step) * step
      );

      float diag = Diagolale(previousNodeGridPosition, nodeGridPosition);

      for (float i = 0; i < diag; i++)
      {
        float t = diag == 0 ? 0f : i / diag;
        Vector2 gridPoint = Vector2.Lerp(previousNodeGridPosition, nodeGridPosition, t);

        gridPoint = new Vector2(
          Mathf.RoundToInt(gridPoint.x / step) * step,
          Mathf.RoundToInt(gridPoint.y / step) * step
        );

        // We check the point position inside the boudaries
        if (
          gridPoint.x > worldBoundaryStart.x &&
          gridPoint.y > worldBoundaryStart.y &&
          gridPoint.x < worldBoundaryEnd.x &&
          gridPoint.y < worldBoundaryEnd.y
        )
        {
          Vector3 leftup = new Vector3(
            gridPoint.x - step / 2,
            0,
            gridPoint.y - step / 2
          );

          Vector3 downright = new Vector3(
            gridPoint.x + step / 2,
            0,
            gridPoint.y + step / 2
          );

          Vector3 leftright = new Vector3(
            gridPoint.x - step / 2,
            0,
            gridPoint.y + step / 2
          );

          Vector3 downleft = new Vector3(
            gridPoint.x + step / 2,
            0,
            gridPoint.y - step / 2
          );

          Debug.DrawLine(
            leftup,
            downright,
            Color.green,
            600
          );

          Debug.DrawLine(
            leftright,
            downleft,
            Color.red,
            600
          );
        }
      }
    }

    /// <summary>
    /// Calculate the maximum diagonale
    /// </summary>
    /// <param name="p0"></param>
    /// <param name="p1"></param>
    /// <returns></returns>
    public float Diagolale(Vector2 p0, Vector2 p1)
    {
      float dx = p1.x - p0.x;
      float dy = p1.y - p0.y;
      float diagonal = Mathf.Max(Mathf.Abs(dx), Mathf.Abs(dy));
      return diagonal;
    }

    /// <summary>
    /// Interpolate a position in grid between 2 points
    /// https://en.wikipedia.org/wiki/Line_drawing_algorithm#:~:text=In%20computer%20graphics%2C%20a%20line,rasterize%20lines%20in%20one%20color.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="diag"></param>
    /// <param name="edge"></param>
    /// <returns></returns>
    public Vector2Int GetPosition(float index, float diag, Vector2 start, Vector2 end)
    {
      float t = diag == 0 ? 0f : index / diag;
      Vector2 position = Vector2.Lerp(start, end, t);

      return new Vector2Int(
        Mathf.RoundToInt(position.y),
        Mathf.RoundToInt(position.x)
      );
    }
  }
}
