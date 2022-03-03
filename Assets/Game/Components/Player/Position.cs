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
    public Vector2Int insideTileQuarterPosition;
    public Vector2Int lastInsideTileQuarterPosition;
    Vector3 lastPosition;

    private void Start() {
      lastPosition = transform.position;
      earth.Reset();
      Calculate();
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

      insideTileQuarterPosition =  earth.tilesManager.InsideTileQuarterPosition(
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
  }
}