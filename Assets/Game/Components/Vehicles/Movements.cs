using UnityEngine;

namespace Game.Vehicles
{
    public abstract class Movements : MonoBehaviour
    {
        public CharacterController characterController;
        public Game.Player.Inputs.InputManager inputManager;

        public abstract void Move(float detlatime);
    }

}
