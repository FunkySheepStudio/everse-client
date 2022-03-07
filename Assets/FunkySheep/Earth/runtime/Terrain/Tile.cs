using System.Threading;
using UnityEngine;

namespace FunkySheep.Earth.Terrain
{
    [AddComponentMenu("FunkySheep/Earth/Earth Terrain Manager")]
    [RequireComponent(typeof(UnityEngine.Terrain))]
    [RequireComponent(typeof(UnityEngine.TerrainCollider))]
    public class Tile : MonoBehaviour
    {
        public Vector2Int position;
        public FunkySheep.Earth.Terrain.AddedTileEvent addedTileEvent;
        public UnityEngine.Terrain terrain;
        Color32[] pixels;
        public float[,] heights;
        Texture2D texture;

        bool heightsCalculated = false;
        public bool heightUpdated = false;


        private void Awake() {
            terrain = GetComponent<UnityEngine.Terrain>();
            terrain.terrainData = new TerrainData();
            GetComponent<UnityEngine.TerrainCollider>().terrainData = terrain.terrainData;
        }

        private void Update() {
            if (heightsCalculated)
            {
                 //terrainData.SetHeights(0, 0, heights);
                terrain.terrainData.SetHeightsDelayLOD(0, 0, heights);
                terrain.terrainData.SyncHeightmap();
                gameObject.AddComponent<Connector>();
                heightsCalculated = false;
                addedTileEvent.Raise(this);
                enabled = false;
            }
        }
        
        /// <summary>
        /// Load Terrarium texture
        /// https://github.com/tilezen/joerd/blob/master/docs/formats.md#terrarium
        /// </summary>
        /// <param name="texture"></param>
        public void SetHeights(Map.Tile tile)
        {
            texture = tile.data.sprite.texture;
            pixels = texture.GetPixels32();
            heights = new float[terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution];

            Thread thread = new Thread(() => this.ProcessHeights());

            thread.Start();
        }

        public void SetHeights(float[,] heights)
        {
          terrain.terrainData.SetHeightsDelayLOD(0, 0, heights);
          terrain.terrainData.SyncHeightmap();
        }

        public void ProcessHeights()
        {

            for (int x = 0; x < Mathf.Sqrt(pixels.Length); x++)
            {
                for (int y = 0; y < Mathf.Sqrt(pixels.Length); y++)
                {
                    float height = GetHeightFromColor(x, y);
                    // Convert the resulting color value to an elevation in meters.
                    heights[
                        x,
                        y
                    ] += height * 0.25f;

                    heights[
                        x + 1,
                        y
                    ] += height * 0.25f;

                    heights[
                        x,
                        y + 1
                    ] += height * 0.25f;

                    heights[
                        x + 1,
                        y + 1
                    ] += height * 0.25f;
                }
            }
            heightsCalculated = true;
        }

        public float GetHeightFromColor(int x, int y)
        {
            Color32 color = pixels[
                y + 
                x * (int)Mathf.Sqrt(pixels.Length)];

            float height = (Mathf.Floor(color.r * 256.0f) + Mathf.Floor(color.g)  + color.b / 256) - 32768.0f;
            height /= 8900;

            return height;
        }
    }    
}
