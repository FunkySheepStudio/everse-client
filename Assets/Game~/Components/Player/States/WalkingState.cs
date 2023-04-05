using System;
using UnityEngine;
using Mirror;

namespace Game.Player.States
{
    [Serializable]
    public class WalkingState : BaseState
    {
        public GameObject model;
        public GameObject cameraController;
        public Transform cam;

        public float speed = 5;

        PlayerInputs playerInputs;
        CharacterController characterController;
        Animator animator;
        float smoothVelocity;
        float gravity = 9.8f;
        float vSpeed = 0f;
        float jumpSpeed = 10f;

        public override void EnterState(Manager player)
        {
            if (player.isLocalPlayer)
            {
                playerInputs = new PlayerInputs();
                playerInputs.Player.Enable();
                characterController = player.GetComponent<CharacterController>();
                animator = model.GetComponent<Animator>();
                player.GetComponent<NetworkAnimator>().animator = animator;

                GameObject cameraGo = GameObject.Instantiate(cameraController, player.transform);
                Cinemachine.CinemachineFreeLook freeLookCamera = cameraGo.GetComponent<Cinemachine.CinemachineFreeLook>();
                freeLookCamera.LookAt = player.transform;
                freeLookCamera.Follow = player.transform;
            }
        }

        public override void UpdateState(Manager player)
        {
            if (player.isLocalPlayer)
            {
                ApplyGravity(player);
                RotateAndMove(player);
            }
        }

        void ApplyGravity(Manager player)
        {
            Vector3 vel = Vector3.down;
            if (characterController.isGrounded)
            {
                animator.SetBool("Grounded", true);
                animator.SetBool("Jump", false);
                vSpeed = 0; // grounded character has vSpeed = 0...
                if (playerInputs.Player.Jump.ReadValue<float>() != 0 )
                { // unless it jumps:
                    vSpeed = jumpSpeed;
                }
            }
            // apply gravity acceleration to vertical speed:
            vSpeed -= gravity * Time.deltaTime;
            vel.y = vSpeed; // include vertical speed in vel
                            // convert vel to displacement and Move the character:

            if (vel.y >= 0.1)
            {
                animator.SetBool("Jump", true);
            }

            characterController.Move(vel * Time.deltaTime);
        }

        void RotateAndMove(Manager player)
        {
            Vector2 movement = playerInputs.Player.Move.ReadValue<Vector2>();
            Vector3 drection = new Vector3(
                movement.x,
                0,
                movement.y
            ).normalized;

            if (drection.magnitude >= 0.1f)
            {
                // Rotate
                float targetAngle = Mathf.Atan2(drection.x, drection.z) * UnityEngine.Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = UnityEngine.Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref smoothVelocity, 0.1f);
                player.transform.rotation = Quaternion.Euler(0f, angle, 0f);

                // Move
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                characterController.Move(moveDir.normalized * speed * Time.deltaTime);

                animator.SetFloat("Speed", 1);
            } else
            {
                animator.SetFloat("Speed", 0);
            }
        }
    }

}
