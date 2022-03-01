using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Earth
{
    [AddComponentMenu("FunkySheep/Earth/Earth Manager")]
    public class Manager : MonoBehaviour
    {
        public FunkySheep.Types.Int32 zoomLevel;
        public FunkySheep.Types.Double latitude;
        public FunkySheep.Types.Double longitude;
        public FunkySheep.Types.Float tileSize;
        public FunkySheep.Types.Vector2Int initialMapPosition;
        public FunkySheep.Types.Vector2Int lastMapPosition;
        public FunkySheep.Types.Vector2Int mapPosition;
        public FunkySheep.Types.Vector2 initialOffset;
        public FunkySheep.Types.Vector2 initialWorldOffset;
        public FunkySheep.Types.Vector2 offset;
        public FunkySheep.Types.Vector2 insideCellPosition;
        public FunkySheep.Events.Vector2IntEvent onMapPositionChanged;

        private void Awake() {
            Reset();
        }

        private void Update() {
            if (lastMapPosition.value != mapPosition.value)
            {
                onMapPositionChanged.Raise(mapPosition.value);
                lastMapPosition.value = mapPosition.value;
            }
        }

        public void Reset()
        {
            tileSize.value = (float)FunkySheep.Map.Utils.TileSize(zoomLevel.value, latitude.value);
            UpdatePositions();
            initialMapPosition.value = mapPosition.value;
            initialOffset.value = offset.value;
            initialWorldOffset.value = initialOffset.value * tileSize.value;
        }

        public void UpdatePositions()
        {
            mapPosition.value = new Vector2Int(
                FunkySheep.Map.Utils.LongitudeToX(zoomLevel.value, longitude.value),
                FunkySheep.Map.Utils.LatitudeToZ(zoomLevel.value, latitude.value)
            );

            offset.value = new Vector2(
                -FunkySheep.Map.Utils.LongitudeToInsideX(zoomLevel.value, longitude.value),
                -1 + FunkySheep.Map.Utils.LatitudeToInsideZ(zoomLevel.value, latitude.value)
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
