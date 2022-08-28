using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Markers
{

    public class Marker : MonoBehaviour
    {
        public Manager markersManager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Unity.Netcode.NetworkObject>().IsOwner)
            {
                markersManager.Open(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Unity.Netcode.NetworkObject>().IsOwner)
            {
                markersManager.Close(this);
            }
        }
    }
}
