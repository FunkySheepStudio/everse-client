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
            public bool vehicle;
            public bool keyboard1;
            public bool keyboard2;
            public bool keyboard3;

            public void Reset()
            {
                movement = Vector2.zero;
                look = Vector2.zero;
                sprint = false;
                jump = false;
                shoot = false;
                //vehicle = false;
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
                    shoot = this.shoot,
                    vehicle = this.vehicle,
                    keyboard1 = this.keyboard1,
                    keyboard2 = this.keyboard2,
                    keyboard3 = this.keyboard3
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
                serializer.SerializeValue(ref vehicle);
                serializer.SerializeValue(ref keyboard1);
                serializer.SerializeValue(ref keyboard2);
                serializer.SerializeValue(ref keyboard3);
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

                _playerInput.actions.FindAction("Vehicle").performed += context =>
                    {
                        currentInput.vehicle = !currentInput.vehicle;
                    };
                cumulativeInput.vehicle = currentInput.vehicle;

                _playerInput.actions.FindAction("Keyboard1").performed += context =>
                {
                    currentInput.keyboard1 = !currentInput.keyboard1;
                };
                cumulativeInput.keyboard1 = currentInput.keyboard1;

                _playerInput.actions.FindAction("Keyboard2").performed += context =>
                {
                    currentInput.keyboard2 = !currentInput.keyboard2;
                };
                cumulativeInput.keyboard2 = currentInput.keyboard2;

                _playerInput.actions.FindAction("Keyboard3").performed += context =>
                {
                    currentInput.keyboard3 = !currentInput.keyboard3;
                };
                cumulativeInput.keyboard3 = currentInput.keyboard3;


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

        public void ShotInput(bool shoot)
        {
        }

        public void OnVehicle(InputValue value)
        {
            VehicleInput(value.isPressed);
        }

        public void VehicleInput(bool vehicle)
        {
        }

        public void OnKeyboard1(InputValue value)
        {
            Keyboard1Input(value.isPressed);
        }

        public void Keyboard1Input(bool keyboard1)
        {
        }

        public void OnKeyboard2(InputValue value)
        {
            Keyboard2Input(value.isPressed);
        }

        public void Keyboard2Input(bool keyboard2)
        {
        }

        public void OnKeyboard3(InputValue value)
        {
            Keyboard3Input(value.isPressed);
        }

        public void Keyboard3Input(bool keyboard1)
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

