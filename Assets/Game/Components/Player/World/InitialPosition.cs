using UnityEngine;
using Unity.Netcode;

namespace Game.Player.World
{
    public class InitialPosition : NetworkBehaviour
    {
        public NetworkVariable<double> networkLatitude = new NetworkVariable<double>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<double> networkLongitude = new NetworkVariable<double>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public FunkySheep.Types.Double latitude;
        public FunkySheep.Types.Double longitude;

        public GameObject player;

        bool netSync = false;
        bool isSpawn = false;

        Vector3 initialPosition = Vector3.zero;

        public override void OnNetworkSpawn()
        {
#if UNITY_SERVER
            if (Unity.Netcode.NetworkManager.Singleton.ConnectedClientsList.Count == 1)
            {
                networkLatitude.OnValueChanged += (double previous, double current) =>
                {
                    latitude.value = current;
                };

                networkLongitude.OnValueChanged += (double previous, double current) =>
                {
                    longitude.value = current;
                    NetworkManager.SceneManager.LoadScene("Game/Components/World/World 1", UnityEngine.SceneManagement.LoadSceneMode.Additive);
                    netSync = true;
                };
            } else
            {
                networkLongitude.OnValueChanged += (double previous, double current) =>
                {
                    netSync = true;
                };
            }
#else
            if (IsOwner)
            {
                networkLatitude.Value = latitude.value;
                networkLongitude.Value = longitude.value;
                netSync = true;
            } else {
                gameObject.SetActive(false);
            }
#endif
        }

        public void SetSpawningPosition()
        {
            if (Game.Manager.Instance.earthManager && netSync && !isSpawn)
            {
                Vector2 spawningPosition = Game.Manager.Instance.earthManager.CalculatePosition(networkLatitude.Value, networkLongitude.Value);
                initialPosition = new Vector3(spawningPosition.x, 0, spawningPosition.y);
                Vector2Int tilePosition = Game.Manager.Instance.earthManager.CalculateTilePosition(initialPosition);
                Game.Manager.Instance.earthManager.AddTile(tilePosition);
                isSpawn = true;
            }
        }

        public void SetSpawningHeight()
        {
            if (isSpawn)
            {
                float? height = FunkySheep.Earth.Terrain.Manager.GetHeight(initialPosition);
                if (height != null)
                {
                    Vector3 newInitialPosition = initialPosition += Vector3.up * height.Value;
                    player.GetComponent<Game.Player.Movements.Manager>().SetPosition(newInitialPosition);

                    gameObject.SetActive(false);
                }
            }
        }

        private void Update()
        {
            SetSpawningPosition();
            SetSpawningHeight();
        }
    }

}
