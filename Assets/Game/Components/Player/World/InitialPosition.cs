using Unity.Netcode;

namespace Game.Player.World
{
    public class InitialPosition : NetworkBehaviour
    {
        public NetworkVariable<double> networkLatitude = new NetworkVariable<double>(default, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);
        public NetworkVariable<double> networkLongitude = new NetworkVariable<double>(default, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);

        public FunkySheep.Types.Double latitude;
        public FunkySheep.Types.Double longitude;


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
                    };
                }
#else
            if (IsOwner)
            {
                networkLatitude.Value = latitude.value;
                networkLongitude.Value = longitude.value;
            } else {
                networkLatitude.OnValueChanged += (double previous, double current) =>
                {
                    latitude.value = current;
                };

                networkLongitude.OnValueChanged += (double previous, double current) =>
                {
                    longitude.value = current;
                };
            }
#endif
        }

        private void Start()
        {
            
        }
    }

}
