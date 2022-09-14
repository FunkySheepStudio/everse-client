using UnityEngine.SceneManagement;
using FunkySheep.Types;

namespace Game
{
    public class Manager : Singleton<Manager>
    {
        public Game.UI.Manager UIManager;
        public FunkySheep.Earth.Manager earthManager;
        public void Start()
        {
#if UNITY_SERVER
            SceneManager.LoadSceneAsync("Game/Components/Authentication/Authentication Server", LoadSceneMode.Single);
#else
            SceneManager.LoadSceneAsync("Game/Components/Authentication/Authentication Client", LoadSceneMode.Single);
#endif
        }

        /*public static void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }*/
    }

}
