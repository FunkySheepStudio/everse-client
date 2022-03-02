using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Earth
{
    [AddComponentMenu("FunkySheep/Earth/Earth Manager")]
    public class Manager : MonoBehaviour
    {
        public FunkySheep.Types.Int32 zoomLevel;
        public FunkySheep.Types.Double initialLatitude;
        public FunkySheep.Types.Double initialLongitude;
        public FunkySheep.Types.Double latitude;
        public FunkySheep.Types.Double longitude;
        public FunkySheep.Types.Float tileSize;
        public FunkySheep.Types.Vector2Int initialMapPosition;
        public FunkySheep.Types.Vector2Int lastMapPosition;
        public FunkySheep.Types.Vector2 mercatorPosition;
        public FunkySheep.Types.Vector2 initialMercatorPosition;
        public FunkySheep.Types.Vector2Int mapPosition;
        public FunkySheep.Types.Vector2 initialOffset;
        public FunkySheep.Types.Vector2 initialWorldOffset;
        public FunkySheep.Types.Vector2 offset;
        public FunkySheep.Types.Vector2Int lastInsideCellPosition;
        public FunkySheep.Types.Vector2Int insideCellPosition;
        public FunkySheep.Events.SimpleEvent onStarted;
        public FunkySheep.Events.Vector2IntEvent onMapPositionChanged;

        private void Awake() {
          Reset();
        }

        private void Update() {
            if (insideCellPosition.value != lastInsideCellPosition.value)
            {
              Debug.Log(insideCellPosition.value);
              AddCells();
              lastInsideCellPosition.value = insideCellPosition.value;
            }
        }

        public void AddCells()
        {
          onMapPositionChanged.Raise(mapPosition.value);
          onMapPositionChanged.Raise(mapPosition.value + insideCellPosition.value * Vector2Int.up);
          onMapPositionChanged.Raise(mapPosition.value + insideCellPosition.value * Vector2Int.right);
          onMapPositionChanged.Raise(mapPosition.value + insideCellPosition.value);
        }

        public void Reset()
        {
          latitude.value = initialLatitude.value;
          longitude.value = initialLongitude.value;
          tileSize.value = (float)Map.Utils.TileSize(zoomLevel.value);
          UpdatePositions();
          initialMercatorPosition.value = mercatorPosition.value;
          initialMapPosition.value = mapPosition.value;
          initialOffset.value = offset.value;
          initialWorldOffset.value = initialOffset.value * tileSize.value;
          onStarted.Raise();
          AddCells();
        }

        public void UpdatePositions()
        {
            mercatorPosition.value = Utils.toCartesianVector2(longitude.value, latitude.value);
            mapPosition.value = Map.Utils.GpsToMap(zoomLevel.value, latitude.value, longitude.value);

            offset.value = new Vector2(
                -Map.Utils.LongitudeToInsideX(zoomLevel.value, longitude.value),
                -1 + Map.Utils.LatitudeToInsideZ(zoomLevel.value, latitude.value)
            );


            if (-offset.value.x >= 0.5f)
            {
                insideCellPosition.value.x = 1;
            } else {
                insideCellPosition.value.x = -1;
            }

            if (-offset.value.y >= 0.5f)
            {
                insideCellPosition.value.y = -1;
            } else {
                insideCellPosition.value.y = 1;
            }
        }
    }
}
