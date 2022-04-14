using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
  public class OnTheRoad : MonoBehaviour
  {
    public float speed = 40;
    public Vector3 direction;
    CharacterController characterController;

    private void Awake() {
      characterController = GetComponent<CharacterController>();
    }
    
    // Update is called once per frame
    void Update()
    {
      Move();
    }

    public void Move()
    {
      transform.position = Vector3.Lerp(transform.position, direction, Time.deltaTime);
    }
  }
}