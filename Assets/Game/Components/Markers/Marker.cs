using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Markers
{

    public class Marker : MonoBehaviour
    {
        public GameObject UI;

        private void OnTriggerEnter(Collider other)
        {
            UI.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            UI.SetActive(false);
        }
    }
}
