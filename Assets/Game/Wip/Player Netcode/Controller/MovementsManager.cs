using UnityEngine;
using FunkySheep.NetWind;

namespace Game.Player.Controller
{
    [RequireComponent(typeof(RewindableTransformState))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Inputs.InputManager))]
    public class MovementsManager : EmptyStateBehaviour
    {
        Game.Player.Inputs.InputManager _inputManager;

        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float moveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float sprintSpeed = 5.335f;

        [Tooltip("Acceleration and deceleration")]
        public float speedChangeRate = 10.0f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float rotationSmoothTime = 0.12f;

        public GameObject mainCamera;

        // player
        private float _speed;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _animationBlend;
        private float _verticalVelocity;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDMotionSpeed;

        private Animator _animator;
        private CharacterController _characterController;

        private bool _hasAnimator;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _inputManager = GetComponent<Inputs.InputManager>();
        }

        private void Start()
        {
            _hasAnimator = TryGetComponent(out _animator);
        }

        public override void Simulate(int tick, float deltaTime)
        {
            //MoveUpdate(deltaTime);
            Move(deltaTime);
            Rotate(deltaTime);
        }

        private void Move(float deltaTime)
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _inputManager.Current.sprint ? sprintSpeed : moveSpeed;

            Vector3 direction = transform.forward * _inputManager.Current.movement.y + transform.right * _inputManager.Current.movement.x;

            _characterController.Move(direction * (targetSpeed * deltaTime));
        }

        private void Rotate(float deltaTime)
        {
            _targetRotation = Mathf.Atan2(_inputManager.Current.look.x, _inputManager.Current.look.y) * Mathf.Rad2Deg +
                                  mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                rotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
    }

}
