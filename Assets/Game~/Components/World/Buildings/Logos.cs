using UnityEngine;

namespace Game.Buildings
{
    [RequireComponent(typeof(FunkySheep.Logos.Manager))]
    public class Logos : MonoBehaviour
    {
        FunkySheep.Logos.Manager logosManager;
        private void Start()
        {
            logosManager = GetComponent<FunkySheep.Logos.Manager>();
        }

        public void Add(GameObject building)
        {
            FunkySheep.Earth.Buildings.Floor floor = building.GetComponent<FunkySheep.Earth.Buildings.Floor>();
            FunkySheep.OSM.Tag buildingType = floor.building.tags.Find(tag => tag.name == "amenity");
            if (buildingType != null && floor.building.hightPoint != null)
            {
                logosManager.Add(
                  buildingType.value,
                  building,
                  new Vector3(
                    floor.building.position.x,
                    floor.building.hightPoint.Value + 20,
                    floor.building.position.y
                  )
                );
            }
        }
    }
}
