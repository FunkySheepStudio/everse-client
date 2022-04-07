using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Guns
{
  public abstract class Gun : ScriptableObject
  {
    public GameObject Model;
    public GameObject Bullets;
    public float cooldown = 0.1f;
    public float lastShot = 0;

    public abstract void Shoot();
  }
}
