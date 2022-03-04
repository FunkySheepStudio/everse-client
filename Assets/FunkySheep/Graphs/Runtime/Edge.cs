using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Graphs
{
  [Serializable]
  public class Edge<T>
  {
    public T verticeA;
    public T verticeB;

    public Edge(T verticeA, T verticeB)
    {
      this.verticeA = verticeA;
      this.verticeB = verticeB;
    }
  }
}
