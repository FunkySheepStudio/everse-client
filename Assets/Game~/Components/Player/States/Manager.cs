using System;
using UnityEngine;
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

        public FunkySheep.Types.Vector2 initialOffset;
        public FunkySheep.Types.Float tileSize;
        public FunkySheep.Types.Vector2 earthInitialMercatorPosition;

        Double currentLatitude;
        Double currentLongitude;
        Vector2Int currentTilePosition;

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

            if (currentState != startingState && (isLocalPlayer || isServer))
            {
                CalculateCurrentTilePosition();
                CalculateCurrentGPSCoordinates();
            }
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

        public void CalculateCurrentTilePosition()
        {
            Vector2Int calculatedTilePosition = FunkySheep.Tiles.Utils.TilePosition(
              new Vector2(
                transform.position.x,
                transform.position.z
              ),
              tileSize.value,
              initialOffset.value
            );

            if (calculatedTilePosition != currentTilePosition)
            {
                currentTilePosition = calculatedTilePosition;
                UpdateWorldTiles();
            }

            /*insideTileQuarterPosition = FunkySheep.Tiles.Utils.InsideTileQuarterPosition(
              new Vector2(
                transform.position.x,
                transform.position.z
              ),
              tileSize.value,
              initialOffset.value
            );

            if (insideTileQuarterPosition != lastInsideTileQuarterPosition)
            {
                UpdateWorldTiles();
                lastInsideTileQuarterPosition = insideTileQuarterPosition;
            }*/
        }

        public void CalculateCurrentGPSCoordinates()
        {
            var calculatedGPS =  Game.Manager.Instance.earthManager.CalculateGPSCoordinates(transform.position);
            currentLatitude = calculatedGPS.latitude;
            currentLongitude = calculatedGPS.longitude;
        }

        public void UpdateWorldTiles()
        {
            Game.World.NetworkManager worldNetworkManager = Game.Manager.Instance.earthManager.GetComponent<Game.World.NetworkManager>();
            worldNetworkManager.SpawnWorldTile(currentTilePosition + Vector2Int.up);
            worldNetworkManager.SpawnWorldTile(currentTilePosition + Vector2Int.up + Vector2Int.right);
            worldNetworkManager.SpawnWorldTile(currentTilePosition + Vector2Int.right);
            worldNetworkManager.SpawnWorldTile(currentTilePosition + Vector2Int.down + Vector2Int.right);
            worldNetworkManager.SpawnWorldTile(currentTilePosition + Vector2Int.down);
            worldNetworkManager.SpawnWorldTile(currentTilePosition + Vector2Int.down + Vector2Int.left);
            worldNetworkManager.SpawnWorldTile(currentTilePosition + Vector2Int.left);
            worldNetworkManager.SpawnWorldTile(currentTilePosition + Vector2Int.up + Vector2Int.left);
        }
    }
}
