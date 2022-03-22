using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace Game.WIP.Building
{
  [RequireComponent(typeof(ProBuilderMesh))]
  [RequireComponent(typeof(MeshCollider))]
  public class Wall : MonoBehaviour
  {
    public Manager building;
    ProBuilderMesh mesh;

    public List<Vector3> points = new List<Vector3>();

    private void Awake() {
      mesh = this.GetComponent<ProBuilderMesh>();
    }

    public void Create() {
      mesh.CreateShapeFromPolygon(points, 2f, false);
      GetComponent<MeshRenderer>().material = building.material;
    }
  }
}
