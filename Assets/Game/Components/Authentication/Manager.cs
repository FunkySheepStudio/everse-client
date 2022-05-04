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
                SceneManager.LoadSceneAsync("Scenes/Main", LoadSceneMode.Additive);
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log(authResponse["data"].ToString());
            }
        }
    }
}
