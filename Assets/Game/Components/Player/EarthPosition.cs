using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class EarthPosition : MonoBehaviour
    {
        public FunkySheep.Types.Double calculatedLatitude;
        public FunkySheep.Types.Double calculatedLongitude;
        public FunkySheep.Earth.Manager earth;
        Vector3 lastPosition;

        private void Start() {
            lastPosition = transform.position;
            Calculate();
            earth.Reset();
        }

        private void Update() {
            if (Vector3.Distance(lastPosition, transform.position) > 50)
            {
                Calculate();
                lastPosition = transform.position;
            }
        }

        public void Calculate() {
            var calculatedGPS = FunkySheep.Earth.Utils.toGeoCoord(
                transform.position + 
                new Vector3(
                    earth.initialMercatorPosition.value.x,
                    0,
                    earth.initialMercatorPosition.value.y
                )
            );

            calculatedLatitude.value = calculatedGPS.latitude;
            calculatedLongitude.value = calculatedGPS.longitude;

            earth.UpdatePositions();
      }
    }
}