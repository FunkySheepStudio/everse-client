using UnityEngine;
using Unity.Netcode;

namespace Game.Netcode
{
    [RequireComponent(typeof(Unity.Netcode.NetworkManager))]
    public class NetcodeServerManager : MonoBehaviour
    {
        NetworkManager _networkManager;

        private void Awake()
        {
            _networkManager = GetComponent<NetworkManager>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _networkManager.StartClient();
        }
    }
}
