using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Markers
{

    public class Marker : MonoBehaviour
    {
        public Manager markersManager;

        private void OnTriggerEnter(Collider other)
        {
            markersManager.Open(this);
        }

        private void OnTriggerExit(Collider other)
        {
            markersManager.Close(this);
        }
    }
}
