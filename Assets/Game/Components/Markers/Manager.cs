using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Markers
{
    public class Manager : MonoBehaviour
    {
        public FunkySheep.Types.Int32 zoomLevel;
        public FunkySheep.Types.Vector2 initialMapPosition;
        public FunkySheep.Types.Float tileSize;
        public GameObject player;
        public GameObject markerAsset;
        public FunkySheep.Network.Services.Create createService;
        public FunkySheep.Network.Services.Find findService;
        UnityEngine.UIElements.UIDocument UI;
        bool creating = false;

        GameObject markerGo;

        private void Awake()
        {
            UI = GetComponent<UnityEngine.UIElements.UIDocument>();
            UI.rootVisualElement.Q<UnityEngine.UIElements.Button>().clicked += Add;
        }

        private void Update()
        {
            if (creating)
            {
                if (Physics.Linecast(transform.position + Vector3.up * 2 + transform.forward, transform.position + Vector3.up * 2 + transform.forward * 20, out RaycastHit hitInfo))
                {
                    markerGo.transform.position =
                        new Vector3(
                                hitInfo.point.x,
                                transform.position.y,
                                hitInfo.point.z
                            ) - transform.forward;
                }
                else
                {
                    markerGo.transform.position = transform.position + transform.forward * 15;
                }
            }
        }

        public void Create()
        {
            markerGo = GameObject.Instantiate(markerAsset);
            markerGo.transform.parent = transform;
            markerGo.transform.position += transform.forward * 15;
            creating = true;
        }

        public void Add()
        {
            if (!creating)
            {
                Create();
            } else
            {
                markerGo.transform.parent = null;

                FunkySheep.Types.Double latitude = ScriptableObject.CreateInstance<FunkySheep.Types.Double>();
                latitude.apiName = "latitude";
                createService.fields.Add(latitude);
                latitude.value = player.GetComponent<Game.Player.Position>().calculatedLatitude.value;

                FunkySheep.Types.Double longitude = ScriptableObject.CreateInstance<FunkySheep.Types.Double>();
                longitude.apiName = "longitude";
                createService.fields.Add(longitude);
                longitude.value = player.GetComponent<Game.Player.Position>().calculatedLongitude.value;

                FunkySheep.Types.Float height = ScriptableObject.CreateInstance<FunkySheep.Types.Float>();
                height.apiName = "height";
                createService.fields.Add(height);
                height.value = transform.position.y;


                createService.Execute();
                createService.fields.Clear();
                creating = false;
            }
        }

        public void Download(Vector2Int worldTile)
        {
            double[] boundaries = FunkySheep.Earth.Map.Utils.CaclulateGpsBoundaries(zoomLevel.value, worldTile);
            findService.query = FunkySheep.SimpleJSON.JSON.Parse("{}");
            findService.query["latitude"]["$gte"] = boundaries[0];
            findService.query["latitude"]["$lte"] = boundaries[2];

            findService.query["longitude"]["$gte"] = boundaries[1];
            findService.query["longitude"]["$lte"] = boundaries[3];

            findService.Execute();
        }

        public void OnReceived(FunkySheep.SimpleJSON.JSONNode jsonMarkers)
        {
            FunkySheep.SimpleJSON.JSONArray markers = jsonMarkers["data"]["data"].AsArray;

            for (int i = 0; i < markers.Count; i++)
            {
                GameObject marker = GameObject.Instantiate(markerAsset);
                Vector2 marker2DPosition = CalculatePosition(markers[i]["latitude"].AsDouble, markers[i]["longitude"].AsDouble);

                marker.transform.position = new Vector3(
                    marker2DPosition.x,
                    markers[i]["height"].AsFloat,
                    marker2DPosition.y
                );

                //marker.transform.parent = transform;
            }
        }

        public Vector2 CalculatePosition(double latitude, double longitude)
        {
            Vector2 position = FunkySheep.Earth.Map.Utils.GpsToMapReal(
              zoomLevel.value,
              latitude,
              longitude,
              initialMapPosition.value
            );

            position.y = -position.y;
            position *= tileSize.value;

            return position;
        }
    }
}
