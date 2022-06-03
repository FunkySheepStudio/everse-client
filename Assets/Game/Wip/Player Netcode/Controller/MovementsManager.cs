using UnityEngine;
using FunkySheep.NetWind;

namespace Game.Player.Controller
{
    [RequireComponent(typeof(RewindableTransformState))]
    [RequireComponent(typeof(CharacterController))]
    public class MovementsManager : EmptyStateBehaviour
    {
        public Game.Player.Inputs.InputManager inputManager;
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float moveSpeed = 2.0f;

        private CharacterController _characterController;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        public override void Simulate(int tick, float deltaTime)
        {
            var input = inputManager.Current;
            _characterController.Move(input.movement * moveSpeed * deltaTime);
        }
    }

}
