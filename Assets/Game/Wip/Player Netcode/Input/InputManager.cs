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

            public void Reset()
            {
                movement = Vector2.zero;
            }

            public State AverageOver(int sampleCount)
            {
                sampleCount = sampleCount > 0 ? sampleCount : 1;

                return new State()
                {
                    movement = this.movement / sampleCount
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
            }
            // ~INetworkSerializable
        }

        PlayerInput _playerInput;

        [Header("Runtime")]
        [SerializeField] private State currentInput;
        [SerializeField] private State cumulativeInput;
        [SerializeField] private int sampleCount;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void MoveInput(Vector2 newMoveDirection)
        {
            if (IsOwn)
            {
                cumulativeInput.movement += newMoveDirection;
                sampleCount++;
            }
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