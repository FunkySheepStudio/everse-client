using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

namespace Game.Netcode
{
    [RequireComponent(typeof(Unity.Netcode.NetworkManager))]
    public class NetcodeServerManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
#if UNITY_SERVER
            NetworkManager.Singleton.StartServer();
            NetworkManager.Singleton.SceneManager.LoadScene("Scenes/World", LoadSceneMode.Additive);

#elif UNITY_EDITOR
            //NetworkManager.Singleton.StartClient();
            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene("Scenes/World", LoadSceneMode.Additive);
#else
            NetworkManager.Singleton.StartClient();
#endif
        }
    }
}
