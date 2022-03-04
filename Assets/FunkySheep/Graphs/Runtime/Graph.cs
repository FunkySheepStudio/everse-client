using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Graphs
{
  [Serializable]
  public class Graph<T>
  {
    public List<T> vertices;
    public List<Edge<T>> edges;

    public bool Contains(T verticeA, T verticeB)
    {
      Edge<T> edge;
      edge = edges.Find(edge => 
        ((EqualityComparer<T>.Default.Equals(edge.verticeA, verticeA) && EqualityComparer<T>.Default.Equals(edge.verticeB, verticeB))) ||
        ((EqualityComparer<T>.Default.Equals(edge.verticeA, verticeB) && EqualityComparer<T>.Default.Equals(edge.verticeB, verticeA)))
      );

      if (edge == null)
      {
        return false;
      } else {
        return true;
      }
    }
  }  
}

