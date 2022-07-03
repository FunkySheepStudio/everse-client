using UnityEngine;
using Unity.Netcode;

namespace Game.Netcode
{
    [RequireComponent(typeof(Unity.Netcode.NetworkManager))]
    public class NetcodeServerManager : MonoBehaviour
    {
        public bool isServer = false;
        NetworkManager _networkManager;

        private void Awake()
        {
            _networkManager = GetComponent<NetworkManager>();
            Application.targetFrameRate = -1;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (isServer)
            {
                _networkManager.StartServer();
            } else {
                _networkManager.StartClient();
            }
        }
    }
}
