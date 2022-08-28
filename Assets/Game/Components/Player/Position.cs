using UnityEngine;

namespace Game.Player
{
    public class Position : MonoBehaviour
    {
        public FunkySheep.Types.Double earthInitialLatitude;

        public FunkySheep.Types.Vector2 earthInitialMercatorPosition;
        public FunkySheep.Types.Vector2 initialOffset;
        public FunkySheep.Types.Float tileSize;
        public FunkySheep.Types.Double calculatedLatitude;
        public FunkySheep.Types.Double calculatedLongitude;
        public FunkySheep.Events.Event<Vector2Int> onEarthTilePositionChanged;

        public Vector2 earthInitialTilePosition;
        public Vector2Int tilePosition;
        public Vector2Int insideTileQuarterPosition;
        public Vector2Int lastInsideTileQuarterPosition;
        public FunkySheep.Events.Vector3Event onMove;
        public FunkySheep.Network.Services.Create playerPosition;

        Vector3 lastPosition;
        Vector2Int lastTilePosition;
        Unity.Netcode.NetworkObject netObject;

        private void Start()
        {
            lastPosition = transform.position;
            netObject = GetComponent<Unity.Netcode.NetworkObject>();
        }

        private void Update()
        {
         
            if (netObject.IsOwner || Unity.Netcode.NetworkManager.Singleton.IsServer)
            {
                CalculateGPS();
                if (Vector3.Distance(lastPosition, transform.position) > 10)
                {
                    Calculate();
                    onMove.Raise(transform.position);
                    lastPosition = transform.position;
                    if (!Unity.Netcode.NetworkManager.Singleton.IsServer)
                    {
                        playerPosition.Execute();
                    }
                }
            }
        }

        public void Calculate()
        {
            tilePosition = FunkySheep.Tiles.Utils.TilePosition(
              new Vector2(
                transform.position.x,
                transform.position.z
              ),
              tileSize.value,
              initialOffset.value
            );

            insideTileQuarterPosition = FunkySheep.Tiles.Utils.InsideTileQuarterPosition(
              new Vector2(
                transform.position.x,
                transform.position.z
              ),
              tileSize.value,
              initialOffset.value
            );

            if (insideTileQuarterPosition != lastInsideTileQuarterPosition)
            {
                onEarthTilePositionChanged.Raise(tilePosition);
                onEarthTilePositionChanged.Raise(tilePosition + Vector2Int.up);
                onEarthTilePositionChanged.Raise(tilePosition + Vector2Int.up + Vector2Int.right);
                onEarthTilePositionChanged.Raise(tilePosition + Vector2Int.right);
                onEarthTilePositionChanged.Raise(tilePosition + Vector2Int.down + Vector2Int.right);
                onEarthTilePositionChanged.Raise(tilePosition + Vector2Int.down);
                onEarthTilePositionChanged.Raise(tilePosition + Vector2Int.down + Vector2Int.left);
                onEarthTilePositionChanged.Raise(tilePosition + Vector2Int.left);
                onEarthTilePositionChanged.Raise(tilePosition + Vector2Int.up + Vector2Int.left);
                
                /*onEarthTilePositionChanged.Raise(tilePosition + insideTileQuarterPosition.y * Vector2Int.up);
                onEarthTilePositionChanged.Raise(tilePosition + insideTileQuarterPosition.x * Vector2Int.right);
                onEarthTilePositionChanged.Raise(tilePosition + insideTileQuarterPosition);*/

                lastInsideTileQuarterPosition = insideTileQuarterPosition;
            }
        }

        public void CalculateGPS()
        {
            var calculatedGPS = FunkySheep.Earth.Utils.toGeoCoord(
                    new Vector2(
                        earthInitialMercatorPosition.value.x + transform.position.x / Mathf.Cos(Mathf.Deg2Rad * (float)earthInitialLatitude.value),
                        earthInitialMercatorPosition.value.y + transform.position.z / Mathf.Cos(Mathf.Deg2Rad * (float)earthInitialLatitude.value)
                        )
                );
            calculatedLatitude.value = calculatedGPS.latitude;
            calculatedLongitude.value = calculatedGPS.longitude;
            //calculatedMercatorPosition.value = FunkySheep.GPS.Utils.toCartesianVector(calculatedLatitude.value, calculatedLongitude.value);
        }
    }
}