using UnityEngine;
using Unity.Netcode;


namespace Game.Player
{
    [RequireComponent(typeof(NetworkObject))]
    public class Manager : MonoBehaviour
    {
        public Camera playerCamera;
        NetworkObject _networkObject;
        private void Start()
        {
            _networkObject = GetComponent<NetworkObject>();
            if (_networkObject.IsOwner)
            {
                playerCamera.gameObject.SetActive(true);
            }
        }
    }
}
