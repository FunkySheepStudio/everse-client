using System.Collections.Generic;
using UnityEngine;
using FunkySheep.NetWind;

namespace Game.Vehicles
{
    public class Manager : EmptyStateBehaviour
    {
        public List<GameObject> vehicles;
        GameObject current;
        Game.Player.Inputs.InputManager inputManager;
        // Start is called before the first frame update

        private void Awake()
        {
            inputManager = GetComponent<Game.Player.Inputs.InputManager>();
        }

        public void Load(int index)
        {
            if (current == null)
            {
                GetComponent<Game.Player.Controller.MovementsManager>().enabled = false;
                current = GameObject.Instantiate(vehicles[index], transform);
                current.GetComponent<Game.Vehicles.Movements>().characterController = GetComponent<CharacterController>();
                current.GetComponent<Game.Vehicles.Movements>().inputManager = inputManager;
            }
        }

        public void UnLoad()
        {
            DestroyImmediate(current);
            current = null;
            GetComponent<Game.Player.Controller.MovementsManager>().enabled = true;
        }

        public override void Simulate(int tick, float deltaTime)
        {
            if (current == null && inputManager.Current.vehicle)
            {
                Load(0);
            }

            if (current != null && inputManager.Current.vehicle)
            {
                current.GetComponent<Movements>().Move(deltaTime);
            }

            if (current != null && !inputManager.Current.vehicle)
            {
                UnLoad();
            }
        }
    }
}
