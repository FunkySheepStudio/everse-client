using UnityEngine;
namespace FunkySheep.Earth.Terrain
{
    [AddComponentMenu("FunkySheep/Earth/Earth Terrain Manager")]
    [RequireComponent(typeof(UnityEngine.Terrain))]
    [RequireComponent(typeof(UnityEngine.TerrainCollider))]
    public class Manager : MonoBehaviour
    {
        public FunkySheep.Map.Manager map;
        UnityEngine.TerrainData terrainData;

        private void Awake() {
            float tileSize = map.GetTileSize();
            terrainData = new TerrainData();
            terrainData.size = new Vector3(
                tileSize,
                8900,
                tileSize
            );
            GetComponent<UnityEngine.Terrain>().terrainData = terrainData;
            GetComponent<UnityEngine.TerrainCollider>().terrainData = terrainData;
        }

        private void Start() {
            //LoadTerrarium(texture);
        }
        
        /// <summary>
        /// Load Terrarium texture
        /// https://github.com/tilezen/joerd/blob/master/docs/formats.md#terrarium
        /// </summary>
        /// <param name="texture"></param>
        public void LoadTerrarium(Texture2D texture)
        {
            // Set required texture options;
            Color32[] pixels = texture.GetPixels32();
            float[,] heights = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];

            for (int i = 0; i < pixels.Length; i++)
            {
                int x = (int)((float)terrainData.heightmapResolution / (float)texture.width * (i % texture.width));
                int y = (int)((float)terrainData.heightmapResolution / (float)texture.height * (i / texture.height));
                Color32 color = pixels[i];

                float height = (Mathf.Floor(color.r * 256.0f) + Mathf.Floor(color.g)  + color.b / 256) - 32768.0f;
                height /= 9000;

                // Convert the resulting color value to an elevation in meters.
                heights[
                    x,
                    y
                ] = height;
            }
            
            //terrainData.SetHeights(0, 0, heights);
            terrainData.SetHeightsDelayLOD(0, 0, heights);
            terrainData.SyncHeightmap();
        }
    }    
}
