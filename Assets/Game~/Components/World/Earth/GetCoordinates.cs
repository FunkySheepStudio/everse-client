using UnityEngine;

/// <summary>
/// To be swicth in the FunkySheep EARTH package
/// </summary>
[AddComponentMenu("Game/Earth/GetCoordinates")]
public class GetCoordinates : MonoBehaviour
{
    public FunkySheep.Types.Double earthInitialLatitude;

    public FunkySheep.Types.Vector2 earthInitialMercatorPosition;
    public FunkySheep.Types.Vector2 initialOffset;
    public FunkySheep.Types.Float tileSize;
    public double calculatedLatitude;
    public double calculatedLongitude;
    public FunkySheep.Types.Double calculatedLatitudeSO;
    public FunkySheep.Types.Double calculatedLongitudeSO;
    
    public Vector2 earthInitialTilePosition;
    public Vector2Int tilePosition;
    public Vector2Int insideTileQuarterPosition;
    public Vector2Int lastInsideTileQuarterPosition;

    Vector3 lastPosition;
    Vector2Int lastTilePosition;

    private void Update()
    {
        if (lastPosition != transform.position)
        {
            CalculateGPS();
            Calculate();
            lastPosition = transform.position;
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
        calculatedLatitude = calculatedGPS.latitude;
        calculatedLongitude = calculatedGPS.longitude;

        if (calculatedLatitudeSO)
        {
            calculatedLatitudeSO.value = calculatedLatitude;
        }

        if (calculatedLongitudeSO)
        {
            calculatedLongitudeSO.value = calculatedLongitude;
        }
    }
}
