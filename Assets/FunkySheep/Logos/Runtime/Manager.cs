using UnityEngine;

namespace FunkySheep.Logos
{
    [AddComponentMenu("FunkySheep/Logos/Logos Manager")]
    public class Manager : MonoBehaviour
    {
        public GameObject logos;
        public Material material;

        public GameObject Add(string name, GameObject parentGo)
        {
            Transform logo = logos.transform.Find(name + ".svg");

            if (logo == null)
            {
                logo = logos.transform.Find("unknown.svg");
            }

            GameObject go = GameObject.Instantiate(logo.gameObject, parentGo.transform);
            go.transform.rotation = Quaternion.Euler(Vector3.zero);
            go.transform.localScale *= 5;
            go.AddComponent<FunkySheep.Effects.Rotate>();
            go.GetComponent<MeshRenderer>().material = material;
            return go;
        }

        public void Add(string name, GameObject parentGo, Vector3 position)
        {
            GameObject go = Add(name, parentGo);
            go.transform.position = position;
        }
    }
}
