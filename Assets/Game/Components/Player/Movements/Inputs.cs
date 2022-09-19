using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using FunkySheep.NetWind;


namespace Game.Player.Movements
{
    public class Inputs : RewindableInputBehaviour<Inputs.State>
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

        public PlayerInputs playerInputs;

        [Header("Runtime")]
        [SerializeField] private State currentInput;
        [SerializeField] private State cumulativeInput;
        [SerializeField] private int sampleCount;

        [Header("Movement Settings")]
        public bool analogMovement;

        private void Awake()
        {
            playerInputs = new PlayerInputs();
        }

        private void Start()
        {
            playerInputs.Player.Move.actionMap.Enable();
        }

        private void Update()
        {
            if (IsOwn)
            {
                Vector2 movement = playerInputs.Player.Move.ReadValue<Vector2>();
                if (movement.magnitude > 1f)
                    movement.Normalize();

                cumulativeInput.movement += movement;
                
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

