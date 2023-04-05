using UnityEngine;
using Mirror;

namespace Game.Player.World
{
    public class Position : NetworkBehaviour
    {
        public FunkySheep.Types.Double playerInitialLatitude;
        public FunkySheep.Types.Double playerInitialLongitude;

        [SyncVar]
        public double syncInitialLatitude = 0;
        [SyncVar]
        public double syncInitialLongitude = 0;

        bool isSpawnPositionSet = false;

        public override void OnStartClient()
        {
            if (isLocalPlayer)
            {
                SyncInitialPosition(playerInitialLatitude.value, playerInitialLongitude.value);
            }
        }

        [Command]
        void SyncInitialPosition(double latitude, double longitude)
        {
            syncInitialLatitude = latitude;
            syncInitialLongitude = longitude;

            if (isServer)
            {
                playerInitialLatitude.value = latitude;
                playerInitialLongitude.value = longitude;
                if (NetworkManager.singleton.numPlayers == 1)
                {
                    Game.Manager.Instance.earthManager.GetComponent<Game.World.NetworkManager>().ServerInit(latitude, longitude);
                } else
                {
                    Game.Manager.Instance.earthManager.GetComponent<Game.World.NetworkManager>().SpawnWorldTile(latitude, longitude);
                }
            }
        }

        private void Update()
        {
            if (!isSpawnPositionSet && isLocalPlayer)
            {
                SetSpawningPosition();
            }
        }

        public void SetSpawningPosition()
        {
            Vector2 spawningPosition = Game.Manager.Instance.earthManager.CalculatePosition(syncInitialLatitude, syncInitialLongitude);

            float? height = FunkySheep.Earth.Terrain.Manager.GetHeight(spawningPosition);
            if (height != null)
            {
                transform.position = new Vector3(
                    spawningPosition.x,
                    height.Value,
                    spawningPosition.y
                    );

                isSpawnPositionSet = true;
            }
        }
    }

}
