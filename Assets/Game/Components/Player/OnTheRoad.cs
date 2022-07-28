using UnityEngine;

namespace Game.Player
{
    public class OnTheRoad : MonoBehaviour
    {
        public float lastHit = 0;
        Game.Player.Controller.MovementsManager movements;

        private void Awake()
        {
            movements = GetComponent<Game.Player.Controller.MovementsManager>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == 20)
            {
                movements.MoveSpeed = 100;
                movements.SprintSpeed = 200;
                lastHit = 0;
            }
        }

        // Update is called once per frame
        void Update()
        {
            lastHit += Time.deltaTime;
            if (lastHit > 1)
            {
                movements.MoveSpeed = 50;
                movements.SprintSpeed = 100;
            }
        }
    }
}