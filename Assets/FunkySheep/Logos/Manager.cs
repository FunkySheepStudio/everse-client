using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Logos
{
  [AddComponentMenu("FunkySheep/Logos/Logos Manager")]
  public class Manager : MonoBehaviour
  {
    public GameObject logos;
    
    public GameObject Add(string name, GameObject parentGo)
    {
      Transform logo = logos.transform.Find(name + ".svg");

      if (logo == null)
      {
        logo = logos.transform.Find("unknown.svg");
      }

      GameObject go = GameObject.Instantiate(logo.gameObject, parentGo.transform);
      go.transform.rotation = Quaternion.Euler(Vector3.zero);
      go.transform.localScale *= 3;
      return go;
    }

    public void Add(string name, GameObject parentGo, Vector3 position)
    {
      GameObject go = Add(name, parentGo);
      go.transform.position = position;
    }
  }
}
