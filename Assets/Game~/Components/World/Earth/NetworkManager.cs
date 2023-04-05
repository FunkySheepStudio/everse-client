using UnityEngine;
using Mirror;

namespace Game.World
{
    public class NetworkManager : NetworkBehaviour
    {
        [SyncVar(hook = nameof(SetInitialLatitude))]
        public double syncInitialLatitude = 0;
        [SyncVar(hook = nameof(SetInitialLongitude))]
        public double syncInitialLongitude = 0;
        
        public FunkySheep.Types.Double EarthInitialLatitude;
        public FunkySheep.Types.Double EarthInitialLongitude;

        public FunkySheep.Types.Double PlayerInitialLatitude;
        public FunkySheep.Types.Double PlayerInitialLongitude;

        bool InitialLatitudeSynced;
        bool InitialLongitudeSynced;

        private void Awake()
        {
            Game.Manager.Instance.earthManager = GetComponent<FunkySheep.Earth.Manager>();
        }

        public void ServerInit(double latitude, double longitude)
        {
            syncInitialLatitude = EarthInitialLatitude.value = latitude;
            syncInitialLongitude = EarthInitialLongitude.value = longitude;
            GetComponent<FunkySheep.Earth.Manager>().Init();
            SpawnWorldTile(EarthInitialLatitude.value, EarthInitialLongitude.value);
        }

        void SetInitialLatitude(double lastLatitude, double newLattidue)
        {
            EarthInitialLatitude.value = newLattidue;
            InitialLatitudeSynced = true;
            ClientInit();
        }

        void SetInitialLongitude(double lastLongitude, double newLongitude)
        {
            EarthInitialLongitude.value = newLongitude;
            InitialLongitudeSynced = true;
            ClientInit();
        }

        void ClientInit()
        {
            if (InitialLatitudeSynced && InitialLongitudeSynced)
            {
                GetComponent<FunkySheep.Earth.Manager>().Init();
                SpawnWorldTile(PlayerInitialLatitude.value, PlayerInitialLongitude.value);
            }
        }

        public void SpawnWorldTile(double latitude, double longitude)
        {
            Vector2 spawningPosition = Game.Manager.Instance.earthManager.CalculatePosition(latitude, longitude);
            Vector3 initialWorldPosition = new Vector3(spawningPosition.x, 0, spawningPosition.y);
            Vector2Int tilePosition = Game.Manager.Instance.earthManager.CalculateTilePosition(initialWorldPosition);
            Game.Manager.Instance.earthManager.AddTile(tilePosition);
        }

        public void SpawnWorldTile(Vector2Int tilePosition)
        {
            Game.Manager.Instance.earthManager.AddTile(tilePosition);
        }
    }
}
