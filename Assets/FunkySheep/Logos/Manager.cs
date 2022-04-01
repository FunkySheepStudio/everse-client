using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Logos
{
  [AddComponentMenu("FunkySheep/Logos/Logos Manager")]
  public class Manager : MonoBehaviour
  {
    public GameObject logos;
    
    public void Add(string name, GameObject parentGo)
    {
      Transform logo = logos.transform.Find(name + ".svg");

      if (logo == null)
      {
        logo = logos.transform.Find("unknown.svg");
      }

      GameObject go = GameObject.Instantiate(logo.gameObject, parentGo.transform);
      go.transform.rotation = Quaternion.Euler(Vector3.zero);
      go.transform.position += Vector3.up * 5;
      go.transform.localScale *= 3;
    }
  }
}
