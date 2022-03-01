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
                earth.tileSize.value * mapTile.tilemapPosition.x,
                0,
                earth.tileSize.value * mapTile.tilemapPosition.y
            );
            terrainTileGo.transform.parent = transform;
            terrainTileGo.name = mapTile.tilemapPosition.ToString();

            // Set the terrain tile componenet
            Tile terrainTile = terrainTileGo.AddComponent<Tile>();

            UnityEngine.Terrain terrain = terrainTile.GetComponent<UnityEngine.Terrain>();

            // Set the tile size
            terrain.terrainData.size = new Vector3(
                earth.tileSize.value,
                8900,
                earth.tileSize.value
            );

            terrain.allowAutoConnect = true;
            terrain.materialTemplate = material;

            terrainTile.SetHeights(mapTile);

            // Set the terrain Connector
            terrainTileGo.AddComponent<Connector>();
        }
    }    
}
