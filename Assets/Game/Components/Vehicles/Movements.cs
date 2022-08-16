using UnityEngine;

namespace Game.Vehicles
{
    public abstract class Movements : MonoBehaviour
    {
        public CharacterController characterController;
        public Game.Player.Inputs.InputManager inputManager;
        public float speed;

        public abstract void Move(float detlatime);
    }

}
