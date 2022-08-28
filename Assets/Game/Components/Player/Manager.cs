using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


namespace Game.Player
{
    [RequireComponent(typeof(NetworkObject))]
    public class Manager : MonoBehaviour
    {
        public List<GameObject> NetworkOwnerComponents;
        public UnityEngine.InputSystem.InputActionAsset playerInputActionsAsset;
        NetworkObject _networkObject;

        private void Start()
        {
            _networkObject = GetComponent<NetworkObject>();

            if (_networkObject.IsOwner)
            {
                foreach (GameObject component  in NetworkOwnerComponents)
                {
                    component.SetActive(true);
                }
                GetComponent<UnityEngine.InputSystem.PlayerInput>().actions = playerInputActionsAsset;
            } else
            {
                GetComponent<UnityEngine.InputSystem.PlayerInput>().actions = null;
            }
        }
    }
}
