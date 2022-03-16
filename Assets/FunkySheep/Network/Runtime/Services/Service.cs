using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Network.Services
{
  public abstract class Service : ScriptableObject
  {
    public string apiPath;

    private void OnEnable() {
      if (Network.Manager.Instance == null)
      {
        Debug.Log("No network manager found");
      } else {
        Network.Manager.Instance.Regiser(this);
      }
    }
  }  
}
