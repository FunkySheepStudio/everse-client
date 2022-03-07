using System;
using System.Collections.Generic;

namespace FunkySheep.OSM
{
    [Serializable]
    public class Bounds
    {
        public double minLatitude;
        public double minLongitude;
        public double maxLatitude;
        public double maxLongitude;
    }
}