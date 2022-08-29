using UnityEngine.SceneManagement;
using FunkySheep.Types;

namespace Game
{
    public class Manager : Singleton<Manager>
    {
        public Game.UI.Manager UIManager;
        public void Start()
        {
#if UNITY_SERVER
        SceneManager.LoadSceneAsync("Game/Components/Authentication/Authentication", LoadSceneMode.Single);
#else
            SceneManager.LoadScene("Game/Components/UI/UI", LoadSceneMode.Single);
            SceneManager.LoadSceneAsync("Game/Components/Authentication/Authentication", LoadSceneMode.Additive);
#endif
        }
    }

}
