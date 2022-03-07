using System;
using System.Collections.Generic;

namespace FunkySheep.OSM
{
    [Serializable]
    public class Way : Element
    {
        public Bounds bounds = new Bounds();
        public List<Node> nodes = new List<Node>();
        public List<Tag> tags = new List<Tag>();
        public Way(int id)
        {
          this.id = id;
        }
    }
}