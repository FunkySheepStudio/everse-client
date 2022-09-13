using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

namespace Game.Netcode
{
    public class NetcodeServerManager : MonoBehaviour
    {
        private void Start()
        {
#if UNITY_SERVER
            NetworkManager.Singleton.StartServer();
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnection;
#else
            NetworkManager.Singleton.StartClient();
#endif
        }

#if UNITY_SERVER
        void OnClientDisconnection(ulong clientId)
        {
            if (Unity.Netcode.NetworkManager.Singleton.ConnectedClientsList.Count == 1)
            {
                Scene world = UnityEngine.SceneManagement.SceneManager.GetSceneByName("Game/Components/World/World 1");
                Unity.Netcode.NetworkManager.Singleton.SceneManager.UnloadScene(world);
            }
        }
#endif
    }
}
