
using System.Collections.Generic;
using UnityEngine;

namespace Game.Roads
{
    public class Road
    {
        public readonly long id;
        public List<Vector2> points = new List<Vector2>();

        public Road(long id)
        {
            this.id = id;
        }
    }
}
