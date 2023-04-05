using Mirror;

namespace Game.Mirror
{
    public class Manager : NetworkManager
    {
        public override void Start()
        {
#if UNITY_SERVER
            StartServer();
#else
            StartClient();
#endif
        }

#if UNITY_SERVER
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            foreach (NetworkIdentity item in conn.clientOwnedObjects)
            {
                Destroy(item.gameObject);
            }

            if (numPlayers == 0)
            {
                ServerChangeScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
        }
#endif
    }
}
