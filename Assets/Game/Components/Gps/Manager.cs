using UnityEngine;

namespace Game.Gps
{
    public class Manager : MonoBehaviour
    {
        [SerializeField]
        public static bool active { get; set; } = false;
        public FunkySheep.Network.Services.Get getLastUserPosition;
        public FunkySheep.Events.SimpleEvent OnInitialCoordinatesSet;
        public GameObject ui;

        public void GetLastUserPosition()
        {
            if (!active)
            {
                getLastUserPosition.Execute();
            } else
            {
                OnInitialCoordinatesSet.Raise();
            }
        }

        public void SetLastUserPosition(FunkySheep.SimpleJSON.JSONNode jsonPosition)
        {
            if (jsonPosition["data"].Count != 0)
            {
                FunkySheep.Gps.Manager gpsManager = GetComponent<FunkySheep.Gps.Manager>();
                gpsManager.latitude.value = jsonPosition["data"]["latitude"];
                gpsManager.longitude.value = jsonPosition["data"]["longitude"];
                active = true;
                ui.SetActive(false);
                OnInitialCoordinatesSet.Raise();
            } else
            {
                ui.SetActive(true);
            }
        }
    }
}
