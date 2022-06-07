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

        [Tooltip("Sprint speed of the character in m/s")]
        public float sprintSpeed = 5.335f;

        [Tooltip("Acceleration and deceleration")]
        public float speedChangeRate = 10.0f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float rotationSmoothTime = 0.12f;

        // player
        private float _speed;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _animationBlend;
        private float _verticalVelocity;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDMotionSpeed;

        private GameObject _mainCamera;
        private Animator _animator;
        private CharacterController _characterController;

        private bool _hasAnimator;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();

            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _hasAnimator = TryGetComponent(out _animator);
        }

        public override void Simulate(int tick, float deltaTime)
        {
            Move(deltaTime);
        }

        private void Move(float deltaTime)
        {
            Vector3 direction = new Vector3(
                inputManager.Current.movement.x,
                0,
                inputManager.Current.movement.y
                );

            // move the player
            if (!inputManager.Current.sprint)
            {
                _characterController.Move(direction * (moveSpeed * deltaTime));
            } else
            {
                _characterController.Move(direction * (sprintSpeed * deltaTime));
            }
        }

            private void MoveUpdate(float deltaTime)
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = inputManager.Current.sprint ? sprintSpeed : moveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (inputManager.Current.movement == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_characterController.velocity.x, 0.0f, _characterController.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = inputManager.analogMovement ? inputManager.Current.movement.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    deltaTime * speedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, deltaTime * speedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(inputManager.Current.movement.x, 0.0f, inputManager.Current.movement.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (inputManager.Current.movement != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    rotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _characterController.Move(targetDirection.normalized * (_speed * deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * deltaTime);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }
    }

}
