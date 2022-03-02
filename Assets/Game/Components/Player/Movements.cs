using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
  public class Movements : MonoBehaviour
  {
    public float speed = 20;
    public float rotateSpeed = 1;
    private CharacterController characterController;
    
    private void Awake() {
      characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
      float curSpeed = 0;

      #if UNITY_EDITOR
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
        curSpeed += speed * Input.GetAxis("Vertical");
        if (Input.GetKey("space"))
        {
          Jump();
        }
      #endif

      characterController.SimpleMove(transform.forward * curSpeed);
    }

    public void Jump()
    {
      //animator.SetBool("isGrounded", false);
      characterController.Move(Vector3.up * 0.1f + transform.forward * 0.1f);
    }
  }
}