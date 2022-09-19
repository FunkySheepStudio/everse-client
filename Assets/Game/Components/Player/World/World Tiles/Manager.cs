using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.World.Tiles
{
    public class Item
    {
        public Vector2Int position;
        public double[] gpsBoundaries;

        public Item(Vector2Int position, double[] gpsBoundaries)
        {
            this.position = position;
            this.gpsBoundaries = gpsBoundaries;
        }
    }
    public class Manager : MonoBehaviour
    {
        public FunkySheep.Types.Int32 zoomLevel;
        public FunkySheep.Network.Services.Create createService;

        Queue<Item> pendingItems = new Queue<Item>();
        bool authenticated = false;

        private void Awake()
        {
            createService.fields.Clear();
        }

        private void Update()
        {
            if (pendingItems.Count != 0 && authenticated)
            {
                RecordTileToServer(pendingItems.Dequeue());
            }
        }

        public void onAuthenticated()
        {
            this.authenticated = true;
        }

        public void onWordTileCreated(Vector2Int position)
        {
            double[] gpsBoundaries = FunkySheep.Earth.Map.Utils.CaclulateGpsBoundaries(zoomLevel.value, position);
            pendingItems.Enqueue(new Item(position, gpsBoundaries));
        }

        public void RecordTileToServer(Item item)
        {
            FunkySheep.Types.Vector2Int position = ScriptableObject.CreateInstance<FunkySheep.Types.Vector2Int>();
            position.apiName = "longitude_max";
            position.value = item.position;
            createService.fields.Add(position);

            FunkySheep.Types.Double latitude_min = ScriptableObject.CreateInstance<FunkySheep.Types.Double>();
            latitude_min.apiName = "latitude_min";
            latitude_min.value = item.gpsBoundaries[0];
            createService.fields.Add(latitude_min);

            FunkySheep.Types.Double longitude_min = ScriptableObject.CreateInstance<FunkySheep.Types.Double>();
            longitude_min.apiName = "longitude_min";
            longitude_min.value = item.gpsBoundaries[1];
            createService.fields.Add(longitude_min);

            FunkySheep.Types.Double latitude_max = ScriptableObject.CreateInstance<FunkySheep.Types.Double>();
            latitude_max.apiName = "latitude_max";
            latitude_max.value = item.gpsBoundaries[2];
            createService.fields.Add(latitude_max);

            FunkySheep.Types.Double longitude_max = ScriptableObject.CreateInstance<FunkySheep.Types.Double>();
            longitude_max.apiName = "longitude_max";
            longitude_max.value = item.gpsBoundaries[3];
            createService.fields.Add(longitude_max);

            createService.Execute();
            createService.fields.Clear();
        }
    }

}
