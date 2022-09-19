using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.Movements.Controllers
{
    public class Controller
    {
        public Game.Player.Movements.Manager manager;
        public UnityEngine.CharacterController characterController;
        public Controller(Game.Player.Movements.Manager manager)
        {
            this.manager = manager;
            this.characterController = manager.GetComponent<UnityEngine.CharacterController>();
        }

        public virtual void Simulate(int tick, float deltaTime)
        {

        }
    }
}
