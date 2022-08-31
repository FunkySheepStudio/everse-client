using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

namespace Game.Netcode
{
    [RequireComponent(typeof(Unity.Netcode.NetworkManager))]
    public class NetcodeServerManager : MonoBehaviour
    {
        void Start()
        {
#if UNITY_SERVER
            NetworkManager.Singleton.StartServer();
            SceneEventProgressStatus status = NetworkManager.Singleton.SceneManager.LoadScene("Game/Components/World/World", LoadSceneMode.Additive);
            //NetworkManager.Singleton.SceneManager.LoadScene("Game/Components/Authentication/Authentication", LoadSceneMode.Additive);
            NetworkManager.Singleton.SceneManager.OnSceneEvent += NetworkManager.Singleton.GetComponent<NetcodeServerManager>().LoadScene;

#elif UNITY_EDITOR
            NetworkManager.Singleton.StartClient();
            /*NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene("Game/Components/World/World", LoadSceneMode.Additive);*/
#else
            NetworkManager.Singleton.StartClient();
#endif
        }

        public void LoadScene(SceneEvent sceneEvent)
        {
            switch (sceneEvent.SceneEventType)
            {
                case SceneEventType.LoadComplete:
                    {
                        NetworkManager.Singleton.SceneManager.OnSceneEvent -= NetworkManager.Singleton.GetComponent<NetcodeServerManager>().LoadScene;
                        NetworkManager.Singleton.SceneManager.LoadScene("Game/Components/Authentication/Authentication", LoadSceneMode.Additive);
                        break;
                    }
            }
        }
    }
}
