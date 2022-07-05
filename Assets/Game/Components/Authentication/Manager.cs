using System;
using FunkySheep.SimpleJSON;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Unity.Netcode;

namespace Game.Authentication
{
    public class Manager : MonoBehaviour
    {
        public FunkySheep.Network.Services.Create authenticate;
        public FunkySheep.Types.String login;
        public FunkySheep.Types.String password;

        public FunkySheep.Types.String id;
        public FunkySheep.Types.String nickname;

        Button btnLogin;
        TextField txtLogin;
        TextField txtPassword;

        private void Awake()
        {
            UIDocument UI = GetComponent<UIDocument>();
            btnLogin = UI.rootVisualElement.Q<Button>("btnLogin");
            btnLogin.clicked += Login;
            txtLogin = UI.rootVisualElement.Q<TextField>("txtLogin");
            txtPassword = UI.rootVisualElement.Q<TextField>("txtPassword");

            txtLogin.value = login.value;
            txtPassword.value = password.value;
        }

        private void Start()
        {
#if UNITY_SERVER
            AutoLogin();
#endif
        }

#if UNITY_SERVER
        void AutoLogin()
        {
            string[] arguments = Environment.GetCommandLineArgs();
            login.value = arguments[1];
            password.value = arguments[2];
            authenticate.Execute();
        }
#endif

        void Login()
        {
            login.value = txtLogin.value;
            password.value = txtPassword.value;
            authenticate.Execute();
        }

        public void onAuthReceived(JSONNode authResponse)
        {
            if (authResponse["data"]["accessToken"] != null)
            {
                id.value = authResponse["data"]["user"]["_id"];
                nickname.value = authResponse["data"]["user"]["nickname"];
#if UNITY_SERVER
                    NetworkManager.Singleton.SceneManager.LoadScene("Scenes/World", LoadSceneMode.Additive);
#endif
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log(authResponse["data"].ToString());
            }
        }
    }
}
