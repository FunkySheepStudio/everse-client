using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Game.Player
{
    [RequireComponent(typeof(NetworkObject))]
    public class Manager : MonoBehaviour
    {
        public FunkySheep.Events.SimpleEvent onSpawn;
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
            }

            onSpawn.Raise();
        }
    }
}
