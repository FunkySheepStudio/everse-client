using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Guns
{
  [CreateAssetMenu(menuName = "Game/Guns/Uzi")]
  public class Uzi : Gun
  {
    public override void Shoot()
    {
      lastShot += Time.deltaTime;
      if (lastShot >= cooldown)
      {
        GameObject bullet = GameObject.Instantiate(Bullets);
        lastShot = 0;
      }
    }
  }
}
