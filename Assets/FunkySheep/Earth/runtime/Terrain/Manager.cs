using UnityEngine;
namespace FunkySheep.Earth.Terrain
{
    public class Manager : MonoBehaviour
    {
        public FunkySheep.Earth.Manager earth;
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

            UnityEngine.Terrain terrain = terrainTile.GetComponent<UnityEngine.Terrain>();

            terrain.terrainData.heightmapResolution = mapTile.data.sprite.texture.width;
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
    }    
}
