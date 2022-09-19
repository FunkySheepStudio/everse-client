using UnityEngine;

namespace Game.Guns
{
    public class Manager
    {
        /*public GameObject hand;
        public Gun gun;
        private Game.Player.Inputs.InputManager _inputManager;
        private GameObject gunGo;

        private void Awake()
        {
            _inputManager = GetComponent<Game.Player.Inputs.InputManager>();
        }

        private void Start()
        {
            gunGo = GameObject.Instantiate(gun.Model, Vector3.zero, Quaternion.identity, hand.transform);
            gunGo.transform.localPosition = Vector3.zero;
            gunGo.transform.localRotation = Quaternion.Euler(new Vector3(-40, 90, -90));
        }

        public override void Simulate(int tick, float deltaTime)
        {
            if (_inputManager.Current.shoot)
            {
                gun.Shoot(gunGo.transform.position + gunGo.transform.up * 0.20f, transform.forward);
            }
        }*/
    }
}
