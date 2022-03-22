using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace Game.WIP.Building
{
  [RequireComponent(typeof(ProBuilderMesh))]
  [RequireComponent(typeof(MeshCollider))]
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

    void CreateWalls()
    {
      List<Vector3> innerPoints = GetInnerPoints();
      for (int i = 0; i < building.points.Count; i++)
      {
        int iA = i;
        int iB = (i + 1) % building.points.Count;
        int iC = (i + 2) % building.points.Count;

        Vector3 pointB = new Vector3(
          building.points[iB].x,
          0,
          building.points[iB].y
        );

        Vector3 pointC = new Vector3(
          building.points[iC].x,
          0,
          building.points[iC].y
        );

        GameObject go = new GameObject(i.ToString());
        go.transform.position = transform.position;
        go.transform.parent = transform;
        Wall wallComponent = go.AddComponent<Wall>();
        wallComponent.building = building;

        wallComponent.points.Add(pointB);
        wallComponent.points.Add(pointC);
        wallComponent.points.Add(innerPoints[iB]);
        wallComponent.points.Add(innerPoints[iA]);

        wallComponent.Create();
      }
    }

    public List<Vector3> GetInnerPoints()
    {
      List<Vector3> innerPoints = new List<Vector3>();
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
            ).normalized * 0.5f;

        Vector3 pointBprime3D = new Vector3(
          pointBprime.x,
          transform.position.y + 1f,
          pointBprime.y
        );

        // Call Raycast
        if (!Physics.Raycast(pointBprime3D, Vector3.down, 1.2f))
        {
          pointBprime =
          building.points[iB] -
            (
              (building.points[iA] - building.points[iB]).normalized +
              (building.points[iC] - building.points[iB]).normalized
            ).normalized * 0.5f;

          pointBprime3D = new Vector3(
            pointBprime.x,
            transform.position.y + 1f,
            pointBprime.y
          );
        }
        
        pointBprime3D.y = 0;
        innerPoints.Add(pointBprime3D);
      }

      return innerPoints;
    }
  }
}
