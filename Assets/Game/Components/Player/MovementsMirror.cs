using UnityEngine;

namespace Game.Player
{
    public class MovementsMirror : MonoBehaviour
    {
        public float speed = 20;
        public float curSpeed;
        public float rotateSpeed = 1;
        public CharacterController characterController;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            curSpeed = 0;

            transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
            curSpeed += speed * Input.GetAxis("Vertical");
            if (Input.GetKey("space"))
            {
                Jump();
            }
            Move();
        }

        public void Jump()
        {
            if (characterController.isGrounded)  
            {
                characterController.Move((Vector3.up + transform.forward) * Time.deltaTime * 100);
            } else
            {
                Debug.Log("Not");
            }
        }

        public void Move()
        {
            characterController.SimpleMove(transform.forward * curSpeed);
        }
    }
}