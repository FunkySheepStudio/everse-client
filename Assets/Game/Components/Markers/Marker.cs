using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Markers
{

    public class Marker : MonoBehaviour
    {
        public FunkySheep.Events.GameObjectEvent markerEditStart;
        public FunkySheep.Events.GameObjectEvent markerEditStop;

        private void OnTriggerEnter(Collider other)
        {
            markerEditStart.Raise(gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            markerEditStop.Raise(gameObject);
        }
    }
}
