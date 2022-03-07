using System;
using System.Collections.Generic;

namespace FunkySheep.OSM
{
    [Serializable]
    public class Tag
    {
        public string name;
        public string value;
        public Tag(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }
}