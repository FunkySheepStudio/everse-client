using System;
using FunkySheep.SimpleJSON;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Game.Authentication
{
    public class Manager : MonoBehaviour
    {
        public FunkySheep.Network.Services.Create authenticate;
        public FunkySheep.Types.String login;
        public FunkySheep.Types.String password;

        public FunkySheep.Types.String id;
        public FunkySheep.Types.String nickname;

        public FunkySheep.Events.SimpleEvent onAuthenticated;

        public GameObject UI;

        public void Start()
        {
#if UNITY_SERVER
#if !UNITY_EDITOR
            ServerAutoLogin();
#else
            Login();
#endif
#endif
        }

        public void Login()
        {
            authenticate.Execute();
        }

        void ServerAutoLogin()
        {
            string[] arguments = Environment.GetCommandLineArgs();

            login.value = arguments[1];
            password.value = arguments[2];
            Debug.Log("Login" + login.value);
            Debug.Log("Password" + password.value);
            authenticate.Execute();
        }

        public void onAuthReceived(JSONNode authResponse)
        {
            if (authResponse["data"]["accessToken"] != null)
            {
                id.value = authResponse["data"]["user"]["_id"];
                nickname.value = authResponse["data"]["user"]["nickname"];
                onAuthenticated.Raise();
            }
            else
            {
                Debug.Log(authResponse["data"].ToString());
            }
        }

        public void LoadNetCodeScene()
        {
            SceneManager.LoadScene("Game/Components/Netcode/Netcode", LoadSceneMode.Single);
        }
    }
}
