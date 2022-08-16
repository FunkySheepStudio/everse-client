using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using FunkySheep.NetWind;

namespace Game.Vehicles
{
    public class Manager : EmptyStateBehaviour
    {
        public NetworkVariable<bool> loaded = new NetworkVariable<bool>();
        private bool lastLoaded = false;
        public NetworkVariable<int> currentIndex = new NetworkVariable<int>();
        public List<GameObject> vehicles;
        GameObject current;
        Game.Player.Inputs.InputManager inputManager;
        // Start is called before the first frame update

        private void Awake()
        {
            inputManager = GetComponent<Game.Player.Inputs.InputManager>();
        }

        private void Update()
        {
            if (!IsOwner && lastLoaded != loaded.Value)
            {
                if (loaded.Value)
                {
                    Load(currentIndex.Value);
                } else
                {
                    UnLoad();
                }

                lastLoaded = loaded.Value;
            }
        }

        public void Load(int index)
        {
            if (current == null)
            {
                GetComponent<Game.Player.Controller.MovementsManager>().enabled = false;
                current = GameObject.Instantiate(vehicles[index], transform);
                current.GetComponent<Game.Vehicles.Movements>().characterController = GetComponent<CharacterController>();
                current.GetComponent<Game.Vehicles.Movements>().inputManager = inputManager;
                if (IsServer)
                {
                    loaded.Value = true;
                    currentIndex.Value = index;
                }
            }
        }

        public void UnLoad()
        {
            DestroyImmediate(current);
            current = null;
            GetComponent<Game.Player.Controller.MovementsManager>().enabled = true;
            if (IsServer)
            {
                loaded.Value = false;
            }
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
