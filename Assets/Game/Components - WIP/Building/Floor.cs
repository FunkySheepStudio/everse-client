using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace Game.WIP.Building
{
  [RequireComponent(typeof(ProBuilderMesh))]
  public class Floor : MonoBehaviour
  {
    public Manager building;
    ProBuilderMesh mesh;

    private void Awake() {
      mesh = this.GetComponent<ProBuilderMesh>();
    }

    public void Create() {
      List<Vector3> points = new List<Vector3>();
      for (int i = 0; i < building.points.Count; i++)
      {
        points.Add(new Vector3(
          building.points[i].x,
          0,
          building.points[i].y
        ));
      }

      mesh.CreateShapeFromPolygon(points, 0.5f, false);
      GetComponent<MeshRenderer>().material = building.material;
      CreateWalls();
    }

    public void CreateWalls()
    {
      for (int i = 0; i < building.points.Count; i++)
      {
        int iA = i;
        int iB = (i + 1) % building.points.Count;
        int iC = (i + 2) % building.points.Count;

        Vector2 pointBprime =
          building.points[iB] + 
            (
              (building.points[iA] - building.points[iB]).normalized +
              (building.points[iC] - building.points[iB]).normalized
            ).normalized;

        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.position = new Vector3(
          pointBprime.x,
          transform.position.y + 0.5f,
          pointBprime.y
        );
      }
    }
  }
}
