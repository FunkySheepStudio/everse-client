using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;


namespace Game.UI.CircleMenu
{
    public class Manager : MonoBehaviour
    {
        public GameObject UI;

        PlayerInputs playerInputs;
        InputAction circularMenuAction;
        private void Awake()
        {
            playerInputs = new PlayerInputs();
        }

        private void OnEnable()
        {
            circularMenuAction = playerInputs.Player.CircularMenu;
            circularMenuAction.Enable();
            circularMenuAction.started += Load;
            circularMenuAction.canceled += UnLoad;
        }

        private void Load(InputAction.CallbackContext callbackContext)
        {
            UI = Game.Manager.Instance.UIManager.Load(UI);
        }

        private void UnLoad(InputAction.CallbackContext callbackContext)
        {
            Game.Manager.Instance.UIManager.UnLoad(UI);
        }
    }
}
