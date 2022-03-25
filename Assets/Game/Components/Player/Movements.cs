using System.Collections;
using System.Collections.Generic;
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
    
    private void Awake() {
      characterController = GetComponent<CharacterController>();
      #if UNITY_ANDROID
        mobileController = GameObject.Instantiate(mobileController);
        mobileController.GetComponentInChildren<Game.Player.Joystick>().movements = this;
      #endif
    }

    // Update is called once per frame
    void Update()
    {
      curSpeed = 0;

      #if UNITY_EDITOR
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
        curSpeed += speed * Input.GetAxis("Vertical");
        if (Input.GetKey("space"))
        {
          Jump();
        }
      #endif
      Move();
    }

    private void FixedUpdate() {
      if (characterController.isGrounded)
      {
        animator.SetBool("isGrounded", true);
      }
    }

    public void Jump()
    {
      animator.SetBool("isGrounded", false);
      characterController.Move(Vector3.up + transform.forward);
    }

    public void Move()
    {
      animator.SetFloat("Speed", curSpeed);
      characterController.SimpleMove(transform.forward * curSpeed);
    }
  }
}