using UnityEngine;

namespace Game.Guns
{
    [CreateAssetMenu(menuName = "Game/Guns/Uzi")]
    public class Uzi : Gun
    {
        public override void Shoot(Vector3 from, Vector3 to)
        {
            lastShot += Time.deltaTime;
            if (lastShot >= cooldown)
            {
                GameObject bullet = GameObject.Instantiate(Bullets);
                bullet.transform.position = from;
                bullet.GetComponent<UziBullets>().direction = to;
                lastShot = 0;
            }
        }
    }
}
