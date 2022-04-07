using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Guns
{
  public class Manager : MonoBehaviour
  {
    public Gun gun;

    private void Start() {
      GameObject.Instantiate(gun.Model);
    }

    private void Update() {
      gun.Shoot();
    }
  }  
}
