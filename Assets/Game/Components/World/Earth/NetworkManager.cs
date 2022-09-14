using UnityEngine;
using Unity.Netcode;

namespace Game.World
{
    public class NetworkManager : NetworkBehaviour
    {
        public NetworkVariable<double> networkLatitude = new NetworkVariable<double>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<double> networkLongitude = new NetworkVariable<double>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public FunkySheep.Types.Double FirstPlayerLatitude;
        public FunkySheep.Types.Double FirstPlayerLongitude;

        private void Awake()
        {
            Game.Manager.Instance.earthManager = GetComponent<FunkySheep.Earth.Manager>();
        }

        public override void OnNetworkSpawn()
        {
#if UNITY_SERVER
            networkLatitude.Value = Game.Manager.Instance.earthManager.initialLatitude.value = FirstPlayerLatitude.value;
            networkLongitude.Value = Game.Manager.Instance.earthManager.initialLongitude.value = FirstPlayerLongitude.value;
#else
            Game.Manager.Instance.earthManager.initialLatitude.value = networkLatitude.Value;
            Game.Manager.Instance.earthManager.initialLongitude.value = networkLongitude.Value;

            networkLatitude.OnValueChanged += (double previous, double current) =>
            {
                Game.Manager.Instance.earthManager.initialLatitude.value = current;
            };

            networkLongitude.OnValueChanged += (double previous, double current) =>
            {
                Game.Manager.Instance.earthManager.initialLongitude.value = current;
            };
#endif
        }
    }
}
