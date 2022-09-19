using System;

namespace Game.Player.States
{
    [Serializable]
    public abstract class BaseState
    {
        public abstract void EnterState(Manager player);
        public abstract void UpdateState(Manager player);
    }
}
