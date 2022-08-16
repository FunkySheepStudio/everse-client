using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Markers
{

    public class Marker : MonoBehaviour
    {
        public FunkySheep.Events.SimpleEvent markerEditStart;
        public FunkySheep.Events.SimpleEvent markerEditStop;

        private void OnTriggerEnter(Collider other)
        {
            markerEditStart.Raise();
        }

        private void OnTriggerExit(Collider other)
        {
            markerEditStop.Raise();
        }
    }
}
