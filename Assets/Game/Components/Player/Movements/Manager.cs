using UnityEngine;
using FunkySheep.NetWind;

namespace Game.Player.Movements
{
    public class Manager : FunkySheep.NetWind.EmptyStateBehaviour
    {
        public GameObject player;
        public Inputs inputs;

        bool setPosition = false;
        Vector3 position = Vector3.zero;

        public override void Simulate(int tick, float deltaTime)
        {
            if (IsOwn || IsServer)
            {
                Move(deltaTime);
                if (setPosition)
                {
                    player.transform.position = position;
                    setPosition = false;
                }
            }
        }

        public void SetPosition(Vector3 position)
        {
            this.position = position;
            setPosition = true;
        }

        void Move(float deltaTime)
        {
            if (inputs.Current.movement != Vector2.zero)
            {
                player.transform.position += new Vector3(
                inputs.Current.movement.x,
                0,
                inputs.Current.movement.y) * deltaTime;
            }
        }
    }
}
