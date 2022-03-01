using System;
using UnityEngine;
namespace FunkySheep.Earth.Terrain
{
    [AddComponentMenu("FunkySheep/Earth/Earth Terrain Manager")]
    [RequireComponent(typeof(UnityEngine.Terrain))]
    [RequireComponent(typeof(UnityEngine.TerrainCollider))]
    public class Tile : MonoBehaviour
    {
        UnityEngine.Terrain terrain;

        private void Awake() {
            terrain = GetComponent<UnityEngine.Terrain>();
            terrain.terrainData = new TerrainData();
            GetComponent<UnityEngine.TerrainCollider>().terrainData = terrain.terrainData;
        }
        
        /// <summary>
        /// Load Terrarium texture
        /// https://github.com/tilezen/joerd/blob/master/docs/formats.md#terrarium
        /// </summary>
        /// <param name="texture"></param>
        public void SetHeights(Map.Tile tile)
        {
            Texture2D texture = tile.data.sprite.texture;
            // Set required texture options;
            Color32[] pixels = texture.GetPixels32();

            float[,] heights = new float[terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution];

            for (int x = 0; x < terrain.terrainData.heightmapResolution; x++)
            {
                for (int y = 0; y < terrain.terrainData.heightmapResolution; y++)
                {
                    float xRatio = (float)texture.width / (float)terrain.terrainData.heightmapResolution;
                    float yRatio = (float)texture.height / (float)terrain.terrainData.heightmapResolution;

                    Color32 color = pixels[
                        Convert.ToInt32(y * yRatio) + 
                        Convert.ToInt32(x * xRatio) * texture.width];

                    float height = (Mathf.Floor(color.r * 256.0f) + Mathf.Floor(color.g)  + color.b / 256) - 32768.0f;
                    height /= 8900;

                    // Convert the resulting color value to an elevation in meters.
                    heights[
                        x,
                        y
                    ] = height;
                }
            }

            //terrainData.SetHeights(0, 0, heights);
            terrain.terrainData.SetHeightsDelayLOD(0, 0, heights);
            terrain.terrainData.SyncHeightmap();
        }
    }    
}
