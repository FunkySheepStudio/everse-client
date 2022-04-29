using UnityEngine;

namespace FunkySheep.Vectors
{
    public class Utils
    {
        /// <summary>
        /// Check if two Vector2 are clockwise around the origin
        /// </summary>
        /// <param name="first">First Vector2</param>
        /// <param name="second">Second Vector2</param>
        /// <param name="origin">The Vector2 orgin to compare from</param>
        /// <returns>Return 1 if clockwise, -1 if anticlockwise, 0 if aligned</returns>
        public static int IsClockWise(Vector2 first, Vector2 second, Vector2 origin)
        {
            Vector2 firstOffset = origin - first;
            Vector2 secondOffset = origin - second;

            float angleOffset = Vector2.SignedAngle(secondOffset, firstOffset);

            if (angleOffset > 0)
            {
                return 1;
            }
            else if (angleOffset < 0)
            {
                return -1;
            }
            else
            {
                return 0;
            }

        }

        /// <summary>
        /// Determine if a list of Vector2 points are Clockwise
        /// </summary>
        /// <param name="list"></param>
        /// <returns>Return a positif number if clockwise</returns>
        public static int IsClockWise(Vector2[] list)
        {
            float result = 0;
            for (int i = 0; i < list.Length; i++)
            {
                Vector2 v0 = list[i];
                Vector2 v1 = list[(i + 1) % list.Length];
                Vector2 v2 = list[(i + 2) % list.Length];
                result += Vector2.SignedAngle(v2 - v1, v0 - v1);
            }

            return (int)result;
        }

        /// <summary>
        /// Determine if a list of Vector2 points are Clockwise around origin
        /// </summary>
        /// <param name="list"></param>
        /// <returns>Return a positif number if clockwise</returns>
        public static int IsClockWise(Vector2[] list, Vector2 origin)
        {
            float result = 0;
            for (int i = 0; i < list.Length; i++)
            {
                Vector2 v0 = list[i];
                Vector2 v1 = origin;
                Vector2 v2 = list[(i + 1) % list.Length];
                result += Vector2.SignedAngle(v2 - v1, v0 - v1);
            }

            return (int)result;
        }

        /// <summary>
        /// Convert a Vector2 Array to a Vector3 Array using a Vector mask to assign missing values the Y axis is emp
        /// </summary>
        /// <param name="v2Array">The Array to convert</param>
        /// <param name="yValue">The value in the Y axis</param>
        /// <returns>The Vector 3 Array</returns>
        public static Vector3[] Vector2toVector3(Vector2[] v2Array, float yValue)
        {
            Vector3[] v3Array = new Vector3[v2Array.Length];

            for (int i = 0; i < v2Array.Length; i++)
            {
                v3Array[i] = new Vector3(
                    v2Array[i].x,
                    yValue,
                    v2Array[i].y
                );
            }

            return v3Array;
        }
    }
}
