using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Markers
{
    public class Manager : MonoBehaviour
    {
        public GameObject markerAsset;
        public FunkySheep.Earth.Manager earth;
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
            markerGo.GetComponent<FunkySheep.Earth.Components.GeoCoordinates>().earth = earth;
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
                latitude.value = markerGo.GetComponent<FunkySheep.Earth.Components.GeoCoordinates>().latitude;

                FunkySheep.Types.Double longitude = ScriptableObject.CreateInstance<FunkySheep.Types.Double>();
                longitude.apiName = "longitude";
                createService.fields.Add(longitude);
                longitude.value = markerGo.GetComponent<FunkySheep.Earth.Components.GeoCoordinates>().longitude;

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
            double[] boundaries = FunkySheep.Earth.Map.Utils.CaclulateGpsBoundaries(earth.zoomLevel.value, worldTile);
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
                Destroy(marker.GetComponent<FunkySheep.Earth.Components.GeoCoordinates>());
                Vector2 marker2DPosition = earth.CalculatePosition(markers[i]["latitude"].AsDouble, markers[i]["longitude"].AsDouble);

                marker.transform.position = new Vector3(
                    marker2DPosition.x,
                    markers[i]["height"].AsFloat,
                    marker2DPosition.y
                );

                //marker.transform.parent = transform;
            }
        }
    }
}
