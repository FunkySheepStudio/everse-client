using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Graphs
{
  public abstract class Manager<T> : MonoBehaviour
  {
    public Graph<T> graph;

    public void Add(T vertice)
    {
      if (!graph.vertices.Contains(vertice))
      {
        graph.vertices.Add(vertice);
      }
    }

    public void Connect(T verticeA, T verticeB)
    {
      Add(verticeA);
      Add(verticeB);

      if (!graph.Contains(verticeA, verticeB))
      {
        graph.edges.Add(
          new Edge<T>(
            verticeA,
            verticeB
          )
        );
      }
    }
  }
}
