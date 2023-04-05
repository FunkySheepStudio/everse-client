using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace Game.UI.CircleMenu
{
    public class Manager : MonoBehaviour
    {
        public GameObject player;
        public GameObject menuItemPrefab;
        public GameObject root;
        public GameObject text;
        public Menu menu;
        public Menu fallBackMenu;

        PlayerInputs playerInputs;
        InputAction circularMenuAction;
        private void Awake()
        {
            playerInputs = new PlayerInputs();
            circularMenuAction = playerInputs.Player.CircularMenu;
            circularMenuAction.Enable();
            circularMenuAction.started += Show;
            circularMenuAction.canceled += Hide;
        }

        private void Start()
        {
            Load(fallBackMenu);
        }

        public void Load(Menu menu)
        {
            foreach (Transform child in root.transform)
                Destroy(child.gameObject);

            for (int i = 0; i < menu.items.Count; i++)
            {
                GameObject menuItemGo = GameObject.Instantiate(menuItemPrefab, root.transform);
                menuItemGo.GetComponent<Image>().rectTransform.Rotate(60 * i * Vector3.forward);
                menuItemGo.transform.GetChild(0).GetComponent<CircleMenu.MenuItem>().menuItem = menu.items[i];
                text.GetComponent<TMPro.TextMeshProUGUI>().text = menu.items[i].text;
            }

            foreach (Transform child in transform)
                child.gameObject.SetActive(false);
        }

        public void UnLoad()
        {
            Load(fallBackMenu);
        }

        private void Show(InputAction.CallbackContext callbackContext)
        {
            //player.GetComponent<Game.Player.Inputs.InputManager>().enabled = false;
            player.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().enabled = false;

            foreach (Transform child in transform)
                child.gameObject.SetActive(true);
        }

        private void Hide(InputAction.CallbackContext callbackContext)
        {
            //player.GetComponent<Game.Player.Inputs.InputManager>().enabled = true;
            player.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().enabled = true;
            foreach (Transform child in transform)
                child.gameObject.SetActive(false);
        }
    }
}
