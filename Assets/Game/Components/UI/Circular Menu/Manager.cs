using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;


namespace Game.UI.CircleMenu
{
    public class Manager : MonoBehaviour
    {
        public GameObject UI;
        public InputAction onShowCicularMenu;
        private void Awake()
        {
            onShowCicularMenu.performed += Load;
            onShowCicularMenu.canceled += UnLoad;
        }

        void Load(InputAction.CallbackContext callback)
        {
            UI = Game.Manager.Instance.UIManager.Load(UI);
        }

        void UnLoad(InputAction.CallbackContext callback)
        {
            Game.Manager.Instance.UIManager.UnLoad(UI);
        }
    }
}
