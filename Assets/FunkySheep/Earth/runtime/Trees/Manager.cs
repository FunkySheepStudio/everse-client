using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace FunkySheep.Earth.Trees
{
    [AddComponentMenu("FunkySheep/Earth/Earth Trees Manager")]
    public class Manager : MonoBehaviour
    {
        public FunkySheep.Earth.Manager earthManager;
        public FunkySheep.Earth.Map.Manager mapManager;
        public GameObject tree;
        public List<Vector3> positions = new List<Vector3>();
        public int drawDistance = 100;
        public void AddedTreeTile(Map.Tile tile)
        {
            Color32[] pixels = tile.data.sprite.texture.GetPixels32();
            Vector3 cellScale = mapManager.transform.localScale;

            Thread thread = new Thread(() => this.ProcessImage(
                tile.tilemapPosition,
                cellScale,
                pixels)
            );

            thread.Start();
        }

        public void ProcessImage(Vector3Int mapPosition, Vector3 tileScale, Color32[] pixels)
        {
            try
            {
                int lastX = -8;
                int lastY = -8;
                for (int i = 0; i < pixels.Length; i++)
                {
                    int x = i % 256;
                    int y = i / 256;

                    if ((x - lastX >= 8 || y - lastY >= 8) && pixels[i].g - pixels[i].r > 10)
                    {
                        positions.Add(
                        new Vector3(
                          earthManager.tilesManager.initialOffset.value.x * earthManager.tilesManager.tileSize.value + (mapPosition.x * tileScale.x * 256) + tileScale.x * x,
                          0,
                          earthManager.tilesManager.initialOffset.value.y * earthManager.tilesManager.tileSize.value + (mapPosition.y * tileScale.y * 256) + tileScale.y * y
                          )
                        );
                        lastX = x;
                        lastY = y;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public void AddTree(Vector3 position)
        {
            GameObject go = GameObject.Instantiate(tree);
            go.transform.position = position;

            go.transform.localScale = new Vector3(
                UnityEngine.Random.Range(3, 5),
                UnityEngine.Random.Range(3, 5),
                UnityEngine.Random.Range(3, 5)
            );
            go.transform.Rotate(
                new Vector3(
                UnityEngine.Random.Range(0, 10),
                UnityEngine.Random.Range(0, 360),
                UnityEngine.Random.Range(0, 10)
                )
            );
            go.transform.parent = transform;
        }

        public void Create(Vector3 playerPosition)
        {
            List<Vector3> closeTrees = positions.FindAll(position => Vector3.Distance(
                position,
                playerPosition
                ) < 800);
            foreach (Vector3 treePosition in closeTrees.ToList())
            {
                float? height = Terrain.Manager.GetHeight(treePosition);
                if (height != null)
                {
                    AddTree(
                        new Vector3(
                            treePosition.x,
                            height.Value,
                            treePosition.z
                        ));

                    positions.Remove(treePosition);
                }
            }
        }
    }
}
