using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Buildings
{
  [RequireComponent(typeof(FunkySheep.Logos.Manager))]
  public class Logos : MonoBehaviour
  {
    FunkySheep.Logos.Manager logosManager;
    private void Start() {
      logosManager = GetComponent<FunkySheep.Logos.Manager>();
    }

    public void Add(GameObject building)
    {
      FunkySheep.Earth.Buildings.Floor floor = building.GetComponent<FunkySheep.Earth.Buildings.Floor>();
      FunkySheep.OSM.Tag buildingType = floor.building.tags.Find(tag => tag.name == "amenity");
      if (buildingType != null)
      {
        logosManager.Add(buildingType.value, building);
      }
    }
  }  
}
