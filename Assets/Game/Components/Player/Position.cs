using UnityEngine;

namespace Game.Player
{
    public class Position : MonoBehaviour
    {
        //public FunkySheep.Types.Double calculatedLatitude;
        //public FunkySheep.Types.Double calculatedLongitude;
        public FunkySheep.Earth.Manager earth;

        public FunkySheep.Types.Double calculatedLatitude;
        public FunkySheep.Types.Double calculatedLongitude;
        public Vector2Int lastTilePosition;
        public Vector2Int tilePosition;
        public Vector2Int insideTileQuarterPosition;
        public Vector2Int lastInsideTileQuarterPosition;
        public FunkySheep.Events.Vector3Event onMove;
        public FunkySheep.Network.Services.Create playerPosition;
        Vector3 lastPosition;

        private void Start()
        {
            lastPosition = transform.position;
            GetComponent<FunkySheep.Earth.Components.SetHeight>().offset = 20;
        }

        private void Update()
        {
            CalculateGPS();
            if (Vector3.Distance(lastPosition, transform.position) > 10)
            {
                Calculate();
                onMove.Raise(transform.position);
                lastPosition = transform.position;
                playerPosition.Execute();
            }
        }

        public void Calculate()
        {
            tilePosition = earth.tilesManager.TilePosition(
              new Vector2(
                transform.position.x,
                transform.position.z
              )
            );

            insideTileQuarterPosition = earth.tilesManager.InsideTileQuarterPosition(
              new Vector2(
                transform.position.x,
                transform.position.z
              )
            );

            if (insideTileQuarterPosition != lastInsideTileQuarterPosition)
            {
                earth.AddTile(tilePosition);
                earth.AddTile(tilePosition + insideTileQuarterPosition.y * Vector2Int.up);
                earth.AddTile(tilePosition + insideTileQuarterPosition.x * Vector2Int.right);
                earth.AddTile(tilePosition + insideTileQuarterPosition);
                lastInsideTileQuarterPosition = insideTileQuarterPosition;
            }
        }

        public void CalculateGPS()
        {
            var calculatedGPS = FunkySheep.Earth.Utils.toGeoCoord(
                    new Vector2(
                        earth.initialMercatorPosition.value.x + transform.position.x / Mathf.Cos(Mathf.Deg2Rad * (float)earth.initialLatitude.value),
                        earth.initialMercatorPosition.value.y + transform.position.z / Mathf.Cos(Mathf.Deg2Rad * (float)earth.initialLatitude.value)
                        )
                );
            calculatedLatitude.value = calculatedGPS.latitude;
            calculatedLongitude.value = calculatedGPS.longitude;
            //calculatedMercatorPosition.value = FunkySheep.GPS.Utils.toCartesianVector(calculatedLatitude.value, calculatedLongitude.value);
        }
    }
}