using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
  public class Position : MonoBehaviour
  {
    public FunkySheep.Types.Double calculatedLatitude;
    public FunkySheep.Types.Double calculatedLongitude;
    public FunkySheep.Earth.Manager earth;
    public Vector2Int lastTilePosition;
    public Vector2Int tilePosition;
    Vector3 lastPosition;

    private void Start() {
      lastPosition = transform.position;
      Calculate();
      earth.Reset();
    }

    private void Update() {
      if (Vector3.Distance(lastPosition, transform.position) > 1)
      {
          Calculate();
          lastPosition = transform.position;
      }
    }
    public void Calculate() {
      tilePosition = earth.tilesManager.TilePosition(
        new Vector2(
          transform.position.x,
          transform.position.z
        )
      );

      if (tilePosition != lastTilePosition)
      {
        earth.AddTile(tilePosition);
        lastTilePosition = tilePosition;
      }

      Vector2 insideTileQuarterPosition =  earth.tilesManager.InsideTilePosition(
        new Vector2(
          transform.position.x,
          transform.position.z
        )
      );

      Debug.Log(insideTileQuarterPosition);
    }
  }
}