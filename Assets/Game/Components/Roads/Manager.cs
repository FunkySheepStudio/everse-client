using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Roads
{
  public class Manager : MonoBehaviour
  {
    public FunkySheep.Types.String urlTemplate;
    public FunkySheep.Earth.Manager earthManager;
    public Material material;
    Queue<Road> roads = new Queue<Road>();

    public void AddTile(FunkySheep.Earth.Terrain.Tile terrainTile)
    {
      DownLoad(terrainTile.position);
    }
    
    public void DownLoad(Vector2Int position)
    {
      double[] gpsBoundaries = FunkySheep.Earth.Map.Utils.CaclulateGpsBoundaries(earthManager.zoomLevel.value, position);
      string interpolatedUrl = InterpolatedUrl(gpsBoundaries);
      StartCoroutine(FunkySheep.Network.Downloader.Download(interpolatedUrl, (fileID, file) => {
        Thread extractOsmThread = new Thread(() => ExtractOsmData(file));
        extractOsmThread.Start();
      }));
    }

    public void ExtractOsmData(byte[] osmFile)
    {
      try
      {
        FunkySheep.OSM.Data parsedData = FunkySheep.OSM.Parser.Parse(osmFile);
        foreach (FunkySheep.OSM.Way way in parsedData.ways)
        {
          Road road = new Road(way.id);

          for (int i = 0; i < way.nodes.Count; i++)
          {
            Vector2 point = earthManager.CalculatePosition(way.nodes[i].latitude, way.nodes[i].longitude);
            road.points.Add(point);
          }

          roads.Enqueue(road);
        }
      }
      catch (Exception e)
      {
        Debug.Log(e);
      }
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

        return urlTemplate.Interpolate(parameters, parametersNames);
    }

    private void Update() {
      if (roads.Count != 0)
      {
        Create(roads.Dequeue());
      }
    }

    public void Create(Road road)
    {
      GameObject roadGo = new GameObject(road.id.ToString());
      roadGo.transform.parent = transform;
      LineRenderer roadline = roadGo.AddComponent<LineRenderer>();

      List<Vector3> points = new List<Vector3>();
      foreach (Vector2 point in road.points)
      {
        float height = FunkySheep.Earth.Terrain.Manager.GetHeight(point);
        Vector3 point3d = new Vector3(
          point.x,
          height + 1,
          point.y
        );

        points.Add(point3d);
      }    

      roadline.material = material;
      roadline.startWidth = roadline.endWidth = 10;
      roadline.positionCount = points.Count;
      roadline.SetPositions(points.ToArray());
    }
  }
}
