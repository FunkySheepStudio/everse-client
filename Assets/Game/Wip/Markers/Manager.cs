using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Markers
{
    public class Manager : MonoBehaviour
    {
        public GameObject markerAsset;
        public FunkySheep.Earth.Manager earth;
        public FunkySheep.Network.Services.Create createService;
        UnityEngine.UIElements.UIDocument UI;

        GameObject markerGo;

        private void Awake()
        {
            UI = GetComponent<UnityEngine.UIElements.UIDocument>();
            UI.rootVisualElement.Q<UnityEngine.UIElements.Button>().clicked += Add;
        }

        private void Start()
        {
            markerGo = GameObject.Instantiate(markerAsset);
            markerGo.GetComponent<FunkySheep.Earth.Components.GeoCoordinates>().earth = earth;
            markerGo.transform.parent = transform;
            markerGo.transform.position += transform.forward * 15;
        }

        private void Update()
        {
            if (Physics.Linecast(transform.position + Vector3.up * 2 + transform.forward, transform.position + Vector3.up * 2 + transform.forward * 20, out RaycastHit hitInfo))
            {
                markerGo.transform.position =
                    new Vector3(
                            hitInfo.point.x,
                            transform.position.y,
                            hitInfo.point.z
                        ) - transform.forward;
            } else
            {
                markerGo.transform.position = transform.position + transform.forward * 15;
            }
        }

        public void Add()
        {
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
        }
    }
}
