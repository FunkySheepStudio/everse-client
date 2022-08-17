using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.PlaneRace
{
    public class GateManager : MonoBehaviour
    {
        public int id;
        public Dictionary<ulong, int> count = new Dictionary<ulong, int>();
        public TMPro.TextMeshProUGUI textComponent;
        public GameObject gateModel;

        private void OnTriggerEnter(Collider player)
        {
            transform.parent.GetComponent<Game.PlaneRace.Manager>().passGate(gameObject, player.gameObject);
        }
    }
}
