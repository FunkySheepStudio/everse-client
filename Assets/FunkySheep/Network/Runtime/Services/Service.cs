using UnityEngine;
using FunkySheep.Events;

namespace FunkySheep.Network.Services
{
  public abstract class Service : ScriptableObject
  {
    public string apiPath;
    public JSONNodeEvent onReceptionEvent;
  }
}
