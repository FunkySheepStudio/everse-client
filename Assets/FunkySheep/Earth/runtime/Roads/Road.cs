using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Earth.Roads
{
  [Serializable]
  public class Road
  {
    public readonly long id;
    int size;

    public Road(long id)
    {
      this.id = id;
    }
  }
}
