using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.MiniGames
{
    public class MiniGamesManager : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("Scenes/Wip/Mini games/Plane Race", LoadSceneMode.Additive);
        }

        public void StopGame()
        {
            SceneManager.UnloadSceneAsync("Scenes/Wip/Mini games/Plane Race");
        }
    }
}
