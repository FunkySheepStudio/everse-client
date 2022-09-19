using UnityEngine;
using FunkySheep.NetWind;

namespace Game.Player.Movements
{
    public class Manager : FunkySheep.NetWind.EmptyStateBehaviour
    {
        public GameObject player;
        public Inputs inputs;
        public Controllers.Controller controller;
        Vector3 position = Vector3.zero;

        private void Awake()
        {
            controller = new Controllers.CharacterControler(this);
        }

        public override void Simulate(int tick, float deltaTime)
        {
            controller.Simulate(tick, deltaTime);
            if (position != Vector3.zero)
            {
                transform.position = position;
                position = Vector3.zero;
            }
        }

        public void SetPosition(Vector3 position)
        {
            this.position = position;
        }
    }
}
