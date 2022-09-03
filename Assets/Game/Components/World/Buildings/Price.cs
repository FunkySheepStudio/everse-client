using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Buildings
{
    public class Price : MonoBehaviour
    {
        Material floorMaterial;

        private void Awake()
        {
            floorMaterial = GetComponent<FunkySheep.Earth.Buildings.Manager>().floorMaterial;
        }

        public void Init(GameObject building)
        {
            float area = building.GetComponent<FunkySheep.Earth.Buildings.Floor>().building.area;

            Renderer rend = building.GetComponent<MeshRenderer>();
            MaterialPropertyBlock matBlock = new MaterialPropertyBlock();

            if (area < 100)
            {
                matBlock.SetColor("_PriceColor", Color.green);
            }
            else if (area < 200)
            {
                matBlock.SetColor("_PriceColor", Color.yellow);
            }
            else
            {
                matBlock.SetColor("_PriceColor", Color.red);
            }

            rend.SetPropertyBlock(matBlock);
        }

        public void Switch()
        {
            if (floorMaterial.GetFloat("_PriceShow") == 1)
            {
                floorMaterial.SetFloat("_PriceShow", 0);
            } else
            {
                floorMaterial.SetFloat("_PriceShow", 1);
            }
        }
    }
}
