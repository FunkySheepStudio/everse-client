using UnityEngine;
using UnityEngine.Tilemaps;

namespace FunkySheep.Map
{
    [AddComponentMenu("FunkySheep/Map/Map Manager")]
    [RequireComponent(typeof(Tilemap))]
    [RequireComponent(typeof(TilemapRenderer))]
    [RequireComponent(typeof(Grid))]
    public class Manager : MonoBehaviour
    {
        public FunkySheep.Types.Int32 zoomLevel;
        public FunkySheep.Types.String url;
        public FunkySheep.Types.Vector2Int textureResolution;
        float tileSize;
        Tilemap tilemap;

        private void Awake() {
            tileSize = (float)Utils.TileSize(zoomLevel.value);

            tilemap = GetComponent<Tilemap>();
            tilemap.transform.localScale = new Vector3(
                tileSize / textureResolution.value.x,
                tileSize / textureResolution.value.y,
            1f);
        }

        public float GetTileSize()
        {
            return tileSize;
        }

        public void AddTile(int x, int y)
        {
            
        }
    }
}
