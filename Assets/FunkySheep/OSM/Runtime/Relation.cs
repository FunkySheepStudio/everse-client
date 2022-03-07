using System;
using System.Collections.Generic;

namespace FunkySheep.OSM
{
    [Serializable]
    public class Relation : Element
    {
        public List<Way> ways = new List<Way>();
        public List<Tag> tags = new List<Tag>();
        public Relation(int id)
        {
            this.id = id;
        }
    }
}