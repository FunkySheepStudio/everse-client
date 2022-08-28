using System;
using FunkySheep.SimpleJSON;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.Authentication
{
    public class Manager : MonoBehaviour
    {
        public FunkySheep.Network.Services.Create authenticate;
        public FunkySheep.Types.String login;
        public FunkySheep.Types.String password;

        public FunkySheep.Types.String id;
        public FunkySheep.Types.String nickname;

        public VisualTreeAsset loginUI;

        Button btnLogin;
        TextField txtLogin;
        TextField txtPassword;

        TemplateContainer loginUIContainer;

        private void Awake()
        {
            loginUIContainer = loginUI.Instantiate();
            if (Game.UI.Manager.Instance.rootDocument)
                Game.UI.Manager.Instance.rootDocument.rootVisualElement.Q<VisualElement>("CenterCenter").Add(loginUIContainer);
            
            btnLogin = loginUIContainer.Q<Button>("btnLogin");
            btnLogin.clicked += Login;
            txtLogin = loginUIContainer.Q<TextField>("txtLogin");
            txtPassword = loginUIContainer.Q<TextField>("txtPassword");

            txtLogin.value = login.value;
            txtPassword.value = password.value;
        }

        private void Start()
        {
#if UNITY_SERVER
#if !UNITY_EDITOR
        ServerAutoLogin();
#else
        Login();
#endif
#endif
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
                SceneManager.LoadScene("Game/Components/Netcode/Netcode", LoadSceneMode.Single);
                if (Game.UI.Manager.Instance.rootDocument)
                    Game.UI.Manager.Instance.rootDocument.rootVisualElement.Q<VisualElement>("CenterCenter").Remove(loginUIContainer);
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log(authResponse["data"].ToString());
            }
        }
    }
}
