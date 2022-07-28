using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using FunkySheep.NetWind;


namespace Game.Player.Inputs
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : RewindableInputBehaviour<InputManager.State>
    {
        [Serializable]
        public struct State : INetworkSerializable
        {
            public Vector2 movement;
            public Vector2 look;
            public bool sprint;
            public bool jump;
            public bool shoot;

            public void Reset()
            {
                movement = Vector2.zero;
                look = Vector2.zero;
                sprint = false;
                jump = false;
                shoot = false;
            }

            public State AverageOver(int sampleCount)
            {
                sampleCount = sampleCount > 0 ? sampleCount : 1;

                return new State()
                {
                    movement = this.movement / sampleCount,
                    look = this.look / sampleCount,
                    sprint = this.sprint,
                    jump = this.jump,
                    shoot = this.shoot
                };
            }

            public override string ToString()
            {
                return $"Input({movement};)";
            }

            // INetworkSerializable
            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
            {
                serializer.SerializeValue(ref movement);
                serializer.SerializeValue(ref look);
                serializer.SerializeValue(ref sprint);
                serializer.SerializeValue(ref jump);
                serializer.SerializeValue(ref shoot);
            }
            // ~INetworkSerializable
        }

        PlayerInput _playerInput;

        [Header("Runtime")]
        [SerializeField] private State currentInput;
        [SerializeField] private State cumulativeInput;
        [SerializeField] private int sampleCount;

        [Header("Movement Settings")]
        public bool analogMovement;

        private void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        private void Update()
        {
            if (IsOwn)
            {
                cumulativeInput.movement = _playerInput.actions.FindAction("Move").ReadValue<Vector2>();
                cumulativeInput.look = _playerInput.actions.FindAction("Look").ReadValue<Vector2>();
                cumulativeInput.sprint = _playerInput.actions.FindAction("Sprint").IsPressed();
                cumulativeInput.jump = _playerInput.actions.FindAction("Jump").IsPressed();
                cumulativeInput.shoot= _playerInput.actions.FindAction("Shoot").IsPressed();
                sampleCount++;
            }
        }

        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void MoveInput(Vector2 newMoveDirection)
        {
        }

        public void OnLook(InputValue value)
        {
            LookInput(value.Get<Vector2>());
        }

        public void LookInput(Vector2 value)
        {
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }

        public void SprintInput(bool sprint)
        {
        }

        public void OnJump(InputValue value)
        {
            JumptInput(value.isPressed);
        }

        public void JumptInput(bool jump)
        {
        }

        public void OnShoot(InputValue value)
        {
            ShotInput(value.isPressed);
        }

        public void ShotInput(bool jump)
        {
        }

        protected override State CaptureInput()
        {
            currentInput = cumulativeInput.AverageOver(sampleCount);
            cumulativeInput.Reset();
            sampleCount = 0;

            return currentInput;
        }

        protected override void ApplyInput(State input)
        {
            currentInput = input;
            cumulativeInput.Reset();
            sampleCount = 0;
        }

        protected override void CommitInput(State state, int tick)
        {
            CommitInputServerRpc(state, tick);
        }

        [ServerRpc]
        private void CommitInputServerRpc(State state, int tick)
        {
            HandleInputCommit(state, tick);
        }

        public State Current => currentInput;
    }
}
