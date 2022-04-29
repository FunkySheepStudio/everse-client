using UnityEngine;

namespace Game.Player
{
    public class OnTheRoad : MonoBehaviour
    {
        public float lastHit = 0;
        public Game.Player.Movements movements;

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == 20)
            {
                movements.speed = 100;
                lastHit = 0;
            }
        }

        // Update is called once per frame
        void Update()
        {
            lastHit += Time.deltaTime;
            if (lastHit > 1)
            {
                movements.speed = 50;
            }
        }
    }
}