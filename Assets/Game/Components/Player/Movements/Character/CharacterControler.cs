using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.Movements.Controllers
{
    public class CharacterControler : Controller
    {
        public CharacterControler(Manager manager) : base(manager) {}
        float speed = 6;

        public override void Simulate(int tick, float deltaTime)
        {
            Move(deltaTime);
        }

        void Move(float deltaTime)
        {
            if (manager.inputs.Current.movement != Vector2.zero)
            {
                Vector3 direction = new Vector3(
                    manager.inputs.Current.movement.x,
                    0,
                    manager.inputs.Current.movement.y
                );

                characterController.Move(direction * deltaTime * speed);
                
            }
        }
    }
}
