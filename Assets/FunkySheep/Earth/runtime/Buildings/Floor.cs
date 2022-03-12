using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace FunkySheep.Earth.Buildings
{
  [RequireComponent(typeof(ProBuilderMesh))]
  [RequireComponent(typeof(MeshCollider))]
  public class Floor : MonoBehaviour
  {
    public Building building;
    public Material material;
    ProBuilderMesh mesh;

    public float? minHeight = null;
    public float? maxHeight = null;
    private void Awake() {
      mesh = this.GetComponent<ProBuilderMesh>();
    }

    public void Create()
    {
      // Get the min and max heights
      foreach (Vector2 point in building.points)
      {
        float height = Terrain.Manager.GetHeight(point);

        if (minHeight == null || height <= minHeight)
        {
          minHeight = height;
        }

        if (maxHeight == null || height >= maxHeight)
        {
          maxHeight = height;
        }
      }

      // Create the shape
      Vector3[] newPositions = new Vector3[building.points.Count];
      for (int i = 0; i < newPositions.Length; i++)
      {
        newPositions[i].x = building.points[i].x - building.position.x;
        newPositions[i].y = 0;
        newPositions[i].z = building.points[i].y - building.position.y;
      }
      
      mesh.CreateShapeFromPolygon(newPositions, maxHeight.Value - minHeight.Value + building.area / 4, false);
      GetComponent<MeshRenderer>().material = material;
      transform.position = new Vector3(building.position.x, minHeight.Value, building.position.y);
    }
  }  
}
