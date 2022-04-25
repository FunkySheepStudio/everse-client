using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
  public class OnTheRoad : MonoBehaviour
  {
    CharacterController characterController;
    Game.Player.Movements movements;
    public float lastHit = 0;

    private void Awake() {
      characterController = GetComponent<CharacterController>();
      movements = GetComponent<Game.Player.Movements>();
    }

    private void OnEnable() {
      lastHit = 0;
      movements.speed = 100;
    }
    
    // Update is called once per frame
    void Update()
    {
      lastHit += Time.deltaTime;
      if (lastHit > 3)
      {
        movements.speed = 50;
        this.enabled = false;
      }
    }
  }
}