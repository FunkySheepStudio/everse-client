using UnityEngine;

namespace Game.Guns
{
    public class Manager : MonoBehaviour
    {
        public Gun gun;

        private void Start()
        {
            GameObject.Instantiate(gun.Model);
        }

        private void Update()
        {
            if (Input.GetButton("Fire1"))
            {
                gun.Shoot(transform.position, transform.forward);
            }
        }
    }
}
