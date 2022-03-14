using UnityEngine;
namespace FunkySheep.Earth.Terrain
{
    public class Manager : MonoBehaviour
    {
        public FunkySheep.Earth.Manager earth;
        public FunkySheep.Earth.Terrain.AddedTileEvent addedTileEvent;
        public Material material;

        public void AddTile(Map.Tile mapTile)
        {
            GameObject terrainTileGo = new GameObject();
            terrainTileGo.transform.position = new Vector3(
                earth.tilesManager.tileSize.value * mapTile.tilemapPosition.x + earth.tilesManager.WorldOffset().x,
                0,
                earth.tilesManager.tileSize.value * mapTile.tilemapPosition.y + earth.tilesManager.WorldOffset().y
            );
            terrainTileGo.transform.parent = transform;
            terrainTileGo.name = mapTile.tilemapPosition.ToString();

            // Set the terrain tile componenet
            Tile terrainTile = terrainTileGo.AddComponent<Tile>();
            terrainTile.position = mapTile.mapPosition;
            terrainTile.addedTileEvent = addedTileEvent;

            UnityEngine.Terrain terrain = terrainTile.GetComponent<UnityEngine.Terrain>();

            terrain.terrainData.heightmapResolution = mapTile.data.sprite.texture.width / 2;
            // Set the tile size
            terrain.terrainData.size = new Vector3(
                earth.tilesManager.tileSize.value,
                8900,
                earth.tilesManager.tileSize.value
            );

            terrain.allowAutoConnect = true;
            terrain.materialTemplate = material;

            terrainTile.SetHeights(mapTile);
        }

        public static float GetHeight(Vector2 position)
        {
          foreach (UnityEngine.Terrain terrain in UnityEngine.Terrain.activeTerrains)
          {
            UnityEngine.Bounds bounds = terrain.terrainData.bounds;
            Vector2 terrainMin = new Vector2(
              bounds.min.x + terrain.transform.position.x,
              bounds.min.z + terrain.transform.position.z
            );

            Vector2 terrainMax = new Vector2(
              bounds.max.x + terrain.transform.position.x,
              bounds.max.z + terrain.transform.position.z
            );
            
            if (position.x >= terrainMin.x && position.y >= terrainMin.y && position.x <= terrainMax.x && position.y <= terrainMax.y)
            {
              return terrain.terrainData.GetInterpolatedHeight(
                (position.x - terrainMin.x) / (terrainMax.x - terrainMin.x),
                (position.y - terrainMin.y) / (terrainMax.y - terrainMin.y)
              );
            }
          }
          return 0;
        }

        public static float GetHeight(Vector3 position)
        {
          return GetHeight(new Vector2(position.x, position.z));
        }
    }    
}
