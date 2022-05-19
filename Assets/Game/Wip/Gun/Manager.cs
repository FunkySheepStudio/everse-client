using UnityEngine;

namespace Game.Guns
{
    public class Manager : MonoBehaviour
    {
        public GameObject hand;
        public Gun gun;
        private Game.Player.Inputs.Manager _input;
        private GameObject gunGo;

        private void Awake()
        {
            _input = GetComponent<Game.Player.Inputs.Manager>();
        }

        private void Start()
        {
            gunGo = GameObject.Instantiate(gun.Model, Vector3.zero, Quaternion.identity, hand.transform);
            gunGo.transform.localPosition = Vector3.zero;
            gunGo.transform.localRotation = Quaternion.Euler(new Vector3(-40, 90, -90));
        }

        private void Update()
        {
            if (_input.shoot)
            {
                gun.Shoot(gunGo.transform.position + gunGo.transform.up * 0.20f, transform.forward);
            }
        }
    }
}
