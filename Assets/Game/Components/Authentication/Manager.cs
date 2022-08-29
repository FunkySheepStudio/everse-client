using System;
using FunkySheep.SimpleJSON;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Game.Authentication
{
    public class Manager : MonoBehaviour
    {
        public FunkySheep.Network.Services.Create authenticate;
        public FunkySheep.Types.String login;
        public FunkySheep.Types.String password;

        public FunkySheep.Types.String id;
        public FunkySheep.Types.String nickname;

        public GameObject UI;

        private void Awake()
        {
            if (Game.Manager.Instance.UIManager)
            {
                UI = Game.Manager.Instance.UIManager.Load(UI);
                if (!login.reset)
                {
                    UI.GetComponentInChildren<UnityEngine.UI.Toggle>().isOn = true;
                    foreach (TMP_InputField textBox in UI.GetComponentsInChildren<TMP_InputField>())
                    {
                        switch (textBox.name)
                        {
                            case "txtLogin":
                                textBox.text = login.value;
                                break;
                            case "txtPassword":
                                textBox.text = password.value;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
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
                SceneManager.LoadScene("Game/Components/Netcode/Netcode", LoadSceneMode.Single);
                /*if (Game.UI.Manager.Instance.rootDocument)
                    Game.UI.Manager.Instance.rootDocument.rootVisualElement.Q<VisualElement>("CenterCenter").Remove(loginUIContainer);*/
                Game.Manager.Instance.UIManager.UnLoad(UI);
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log(authResponse["data"].ToString());
            }
        }
    }
}
