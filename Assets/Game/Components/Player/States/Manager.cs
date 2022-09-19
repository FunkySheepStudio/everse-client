using Mirror;

namespace Game.Player.States
{
    public class Manager : NetworkBehaviour
    {
        BaseState currentState;
        public StartingState startingState = new StartingState();
        public WalkingState walkingState = new WalkingState();

        public FunkySheep.Types.Double InitialLatitude;
        public FunkySheep.Types.Double InitialLongitude;

        [SyncVar]
        public double syncInitialLatitude = 0;
        [SyncVar]
        public double syncInitialLongitude = 0;

        public override void OnStartClient()
        {
            currentState = startingState;
            currentState.EnterState(this);
        }

        private void Update()
        {
            if (currentState != null)
                currentState.UpdateState(this);
        }

        public void SwitchState(BaseState state)
        {
            currentState = state;
            state.EnterState(this);
        }

        [Command]
        public void SyncInitialPosition(double latitude, double longitude)
        {
            syncInitialLatitude = latitude;
            syncInitialLongitude = longitude;

            if (isServer)
            {
                InitialLatitude.value = latitude;
                InitialLongitude.value = longitude;
                if (NetworkManager.singleton.numPlayers == 1)
                {
                    Game.Manager.Instance.earthManager.GetComponent<Game.World.NetworkManager>().ServerInit(latitude, longitude);
                }
                else
                {
                    Game.Manager.Instance.earthManager.GetComponent<Game.World.NetworkManager>().SpawnWorldTile(latitude, longitude);
                }

                SwitchState(walkingState);
            }
        }
    }
}
