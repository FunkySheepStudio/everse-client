using UnityEngine;
using Unity.Netcode;


namespace Game.Player
{
    [RequireComponent(typeof(NetworkObject))]
    public class Manager : MonoBehaviour
    {
        public Camera playerCamera;
        public UnityEngine.InputSystem.InputActionAsset playerInputActionsAsset;
        NetworkObject _networkObject;
        private void Start()
        {
            _networkObject = GetComponent<NetworkObject>();
            if (_networkObject.IsOwner)
            {
                playerCamera.gameObject.SetActive(true);
                GetComponent<UnityEngine.InputSystem.PlayerInput>().actions = playerInputActionsAsset;
            } else
            {
                GetComponent<UnityEngine.InputSystem.PlayerInput>().actions = null;
            }
        }
    }
}
