using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        public List<GameObject> currentMarkers = new List<GameObject>();
        public FunkySheep.Network.Services.Create createService;
        public FunkySheep.Network.Services.Find findService;
        public FunkySheep.Network.Services.Patch patchService;
        bool creating = false;
        public GameObject UI;

        GameObject markerGo;

        public void Init()
        {
            Vector2Int mapPosition = new Vector2Int((int)initialMapPosition.value.x, (int)initialMapPosition.value.y);
            Download(mapPosition);
            Download(mapPosition + Vector2Int.up);
            Download(mapPosition + Vector2Int.up + Vector2Int.right);
            Download(mapPosition + Vector2Int.right);
            Download(mapPosition + Vector2Int.down + Vector2Int.right);
            Download(mapPosition + Vector2Int.down);
            Download(mapPosition + Vector2Int.down + Vector2Int.left);
            Download(mapPosition + Vector2Int.left);
            Download(mapPosition + Vector2Int.up + Vector2Int.left);
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

        public void Open(Marker marker)
        {
            UI = Game.Manager.Instance.UIManager.Load(UI);
        }

        public void Close(Marker marker)
        {
            Game.Manager.Instance.UIManager.UnLoad(UI);
        }

        public void Create()
        {
            if (!creating)
            {
                markerGo = GameObject.Instantiate(markerAsset, transform);
                markerGo.GetComponent<Marker>().markersManager = this;
                markerGo.transform.position += transform.forward * 15;
                creating = true;
            }
            else
            {
                FunkySheep.Types.String name = ScriptableObject.CreateInstance<FunkySheep.Types.String>();
                name.apiName = "name";
                createService.fields.Add(name);
                //name.value = createUIContainer.Q<TextField>("MarkerName").value;

                FunkySheep.Types.String date = ScriptableObject.CreateInstance<FunkySheep.Types.String>();
                date.apiName = "date";
                createService.fields.Add(date);
                //date.value = createUIContainer.Q<TextField>("MarkerDate").value;

                FunkySheep.Types.String time = ScriptableObject.CreateInstance<FunkySheep.Types.String>();
                time.apiName = "time";
                createService.fields.Add(time);
                //time.value = createUIContainer.Q<TextField>("MarkerTime").value;

                FunkySheep.Types.String type = ScriptableObject.CreateInstance<FunkySheep.Types.String>();
                type.apiName = "type";
                createService.fields.Add(type);
                //type.value = createUIContainer.Q<DropdownField>("MarkerType").value;

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
                //DestroyImmediate(markerGo);
                markerGo.SetActive(false);
                creating = false;
            }
        }


        public void Patch()
        {
            FunkySheep.Types.String _id = ScriptableObject.CreateInstance<FunkySheep.Types.String>();
            _id.apiName = "_id";
            patchService.fields.Add(_id);
            //_id.value = createUIContainer.Q<Label>("MarkerId").text;
            patchService.id = _id;

            FunkySheep.Types.String name = ScriptableObject.CreateInstance<FunkySheep.Types.String>();
            name.apiName = "name";
            patchService.fields.Add(name);
            //name.value = createUIContainer.Q<TextField>("MarkerName").value;

            FunkySheep.Types.String date = ScriptableObject.CreateInstance<FunkySheep.Types.String>();
            date.apiName = "date";
            patchService.fields.Add(date);
            //date.value = createUIContainer.Q<TextField>("MarkerDate").value;

            FunkySheep.Types.String time = ScriptableObject.CreateInstance<FunkySheep.Types.String>();
            time.apiName = "time";
            patchService.fields.Add(time);
            //time.value = createUIContainer.Q<TextField>("MarkerTime").value;

            FunkySheep.Types.String type = ScriptableObject.CreateInstance<FunkySheep.Types.String>();
            type.apiName = "type";
            patchService.fields.Add(type);
            //type.value = createUIContainer.Q<DropdownField>("MarkerType").value;

            patchService.Execute();
            patchService.fields.Clear();

            SceneManager.LoadSceneAsync("Scenes/Wip/Mini games/Plane Race", LoadSceneMode.Additive);

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

        public void OnServiceReception(FunkySheep.SimpleJSON.JSONNode jsonMarkers)
        {
            GameObject marker;
            Vector2 marker2DPosition;

            switch ((string)jsonMarkers["method"])
            {
                case "create":
                    marker = GameObject.Instantiate(markerAsset);
                    marker.name = jsonMarkers["data"]["_id"];
                    marker2DPosition = CalculatePosition(jsonMarkers["data"]["latitude"].AsDouble, jsonMarkers["data"]["longitude"].AsDouble);

                    marker.transform.position = new Vector3(
                        marker2DPosition.x,
                        jsonMarkers["data"]["height"].AsFloat,
                        marker2DPosition.y
                    );
                    marker.GetComponent<Marker>().markersManager = this;
                    marker.GetComponent<CapsuleCollider>().enabled = true;
                    currentMarkers.Add(marker);

                    creating = false;
                    break;
                case "find":
                    FunkySheep.SimpleJSON.JSONArray markers = jsonMarkers["data"]["data"].AsArray;

                    for (int i = 0; i < markers.Count; i++)
                    {
                        if (currentMarkers.Find(item => item.name == markers[i]["_id"]))
                            break;
                        marker = GameObject.Instantiate(markerAsset);
                        marker.name = markers[i]["_id"];
                        marker2DPosition = CalculatePosition(markers[i]["latitude"].AsDouble, markers[i]["longitude"].AsDouble);

                        marker.transform.position = new Vector3(
                            marker2DPosition.x,
                            markers[i]["height"].AsFloat,
                            marker2DPosition.y
                        );
                        marker.GetComponent<Marker>().markersManager = this;
                        marker.GetComponent<CapsuleCollider>().enabled = true;
                        currentMarkers.Add(marker);
                    }
                    break;
                default:
                    break;
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
