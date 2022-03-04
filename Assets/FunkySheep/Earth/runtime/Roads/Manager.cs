using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Earth.Roads
{
  public class Manager : MonoBehaviour
  {
    public Earth.Manager earthManager;
    public FunkySheep.Graphs.Vector2.Manager graphManager;

    private void Awake() {
      graphManager = GetComponent<FunkySheep.Graphs.Vector2.Manager>();
    }

    private void Start() {
      ProcessRoads();
    }

    public void ProcessRoads() {
      foreach (Graphs.Edge<Vector2> edge in graphManager.graph.edges)
      {
        float diag = Diagolale(edge.verticeA, edge.verticeB);

        for (float i = 0; i < diag; i++)
        {
          float t = diag == 0 ? 0f : i / diag;
          Vector2 position = Vector2.Lerp(edge.verticeA, edge.verticeB, t);
          position = new Vector2(
            Mathf.Round(position.x),
            Mathf.Round(position.y)
          );
          Draw(position);
        }
      }
    }

    public void Draw(Vector2 position)
    {
      GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
      go.transform.position = new Vector3(
        position.x,
        0,
        position.y
      );
    }

    public float Diagolale(Vector2 p0, Vector2 p1)
    {
      float dx = p1.x - p0.x;
      float dy = p1.y - p0.y;
      float diagonal = Mathf.Max(Mathf.Abs(dx), Mathf.Abs(dy));
      return diagonal;
    }

    public void AddTile(Vector2Int position)
    {
      double[] gpsBoundaries = FunkySheep.Earth.Map.Utils.CaclulateGpsBoundaries(earthManager.zoomLevel.value, position);
    }
  }
}
