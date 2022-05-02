using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Grass
{
    public class Manager : MonoBehaviour
    {
        public FunkySheep.Earth.Manager earth;
        public GameObject grassAsset;

        public void AddedDiffuseTile(FunkySheep.Earth.Map.Tile tile)
        {
            Transform tileTransform = transform.Find(tile.tilemapPosition.ToString());
            if (tileTransform == null)
            {
                tileTransform = AddTile(tile.tilemapPosition);
            }

            UnityEngine.VFX.VisualEffect vfxGrass = tileTransform.gameObject.GetComponent<UnityEngine.VFX.VisualEffect>();
            vfxGrass.SetTexture("Diffuse", tile.data.sprite.texture);

        }

        public void AddedHeightTile(FunkySheep.Earth.Map.Tile tile)
        {
            Transform tileTransform = transform.Find(tile.tilemapPosition.ToString());
            if (tileTransform == null)
            {
                tileTransform = AddTile(tile.tilemapPosition);
            }

            UnityEngine.VFX.VisualEffect vfxGrass = tileTransform.gameObject.GetComponent<UnityEngine.VFX.VisualEffect>();
            vfxGrass.SetTexture("Elevation", tile.data.sprite.texture);
        }

        public Transform AddTile(Vector3Int position)
        {
            GameObject go = GameObject.Instantiate(grassAsset);
            go.name = position.ToString();
            go.transform.parent = transform;
            go.transform.position = new Vector3(
                earth.tilesManager.tileSize.value * position.x + earth.tilesManager.WorldOffset().x,
                0,
                earth.tilesManager.tileSize.value * position.y + earth.tilesManager.WorldOffset().y
            );

            go.transform.localScale = new Vector3(
                earth.tilesManager.tileSize.value / 256,
                1,
                earth.tilesManager.tileSize.value / 256
            );

            return go.transform;
        }
    }

}
