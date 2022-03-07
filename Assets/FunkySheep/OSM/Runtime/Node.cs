using System;
using UnityEngine;
namespace FunkySheep.OSM
{
    [Serializable]
    public class Node : Element
    {
        public double latitude;
        public double longitude;

        public Vector2 position;

      public Node(long id, double latitude, double longitude)
      {
        this.id = id;
        this.latitude = latitude;
        this.longitude = longitude;
      }
    }
}