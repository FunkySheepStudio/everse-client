using UnityEngine;

namespace Game.Player
{
    public class Movements : MonoBehaviour
    {
        public float speed = 20;
        public float curSpeed;
        public float rotateSpeed = 1;
        public CharacterController characterController;
        public Animator animator;
        public GameObject mobileController;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
#if PLATFORM_ANDROID
            mobileController = GameObject.Instantiate(mobileController);
            mobileController.GetComponentInChildren<Game.Player.Joystick>().movements = this;
#endif
        }

        // Update is called once per frame
        void Update()
        {
            curSpeed = 0;

            transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime * 10, 0);
            curSpeed += speed * Input.GetAxis("Vertical");
            if (Input.GetKey("space"))
            {
                Jump();
            }
            Move();
        }

        private void FixedUpdate()
        {
            if (characterController.isGrounded)
            {
                animator.SetBool("isGrounded", true);
            }
        }

        public void Jump()
        {
            if (characterController.isGrounded)  
            {
                animator.SetBool("isGrounded", false);
                characterController.Move((Vector3.up * 50 + transform.forward));
            }
        }

        public void Move()
        {
            animator.SetFloat("Speed", curSpeed);
            characterController.SimpleMove(transform.forward * curSpeed);
        }
    }
}