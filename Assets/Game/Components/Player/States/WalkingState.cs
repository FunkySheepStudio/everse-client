using System;
using UnityEngine;

namespace Game.Player.States
{
    [Serializable]
    public class WalkingState : BaseState
    {
        public override void EnterState(Manager player)
        {
            Debug.Log("Wolking");
        }

        public override void UpdateState(Manager player)
        {

        }
    }

}
