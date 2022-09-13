using UnityEngine;
using TMPro;

namespace Game.Authentication.UI
{
    public class Manager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Game.Authentication.Manager authManager = GetComponent<Game.Authentication.Manager>();

            if (!authManager.login.reset)
            {
                GetComponentInChildren<UnityEngine.UI.Toggle>().isOn = true;
                foreach (TMP_InputField textBox in GetComponentsInChildren<TMP_InputField>())
                {
                    switch (textBox.name)
                    {
                        case "txtLogin":
                            textBox.text = authManager.login.value;
                            break;
                        case "txtPassword":
                            textBox.text = authManager.password.value;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
