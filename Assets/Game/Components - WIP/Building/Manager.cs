using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace Game.WIP.Building
{
  public class Manager : MonoBehaviour
  {
    public List<Vector2> points;
    [Range(1, 1000)]
    public int floorCount;
    public float floorHeight;
    public Material material;
    List<GameObject> floors = new List<GameObject>();
    Vector2 center;
    ProBuilderMesh mesh;
    private void Awake() {
      mesh = this.GetComponent<ProBuilderMesh>();
    }

    private void Update() {
      if (floors.Count > floorCount)
      {
        RemFloor();
      }

      if (floors.Count < floorCount)
      {
        AddFloor();
      }
    }

    public void AddFloor()
    {
      GameObject floor = new GameObject("floor-" + floors.Count.ToString());
      floors.Add(floor);
      floor.transform.position = this.transform.position + Vector3.up * floors.Count * floorHeight;
      floor.transform.parent = transform;
      Floor floorComponent = floor.AddComponent<Floor>();
      floorComponent.building = this;
      floorComponent.Create();
    }

    public void RemFloor()
    {
      GameObject lastFloor = floors[floors.Count - 1];
      floors.Remove(lastFloor);
      Destroy(lastFloor);
    }
  }
}
