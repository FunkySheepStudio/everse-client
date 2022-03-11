using System;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Earth.Buildings
{
  [Serializable]
  public class Building
  {
    public long id;
    public List<Vector2> points = new List<Vector2>();
    public List<Vector2> heights = new List<Vector2>();
    public Vector2 position;
    public float? lowPoint = null;
    public float? hightPoint = null;

    public Building(long id)
    {
      this.id = id;
    }

    public void Initialize()
    {
      this.position = Position();
      SetFirstPoint();
      SetClockWise();
    }

    /// <summary>
    /// Calculate the center relative to the points
    /// </summary>
    /// <returns>The center of all points</returns>
    public Vector2 Position()
    {
        // Calculate the center
        Vector2 center = Vector2.zero;
        for (int i = 0; i < points.Count; i++)
        {
            center += points[i];
        }

        center /= points.Count;

        return center;
    }

    /// <summary>
    /// Calculate the building area
    /// </summary>
    /// <returns></returns>
    public float Area()
    {
        float area = 0;
        
        for (int i = 0; i < points.Count -1; i++)
        {
            area += Vector2.Distance(points[i], points[i + 1]);
        }

        return area;
    }

    /// <summary>
    /// If the Vector Array is clockwise, return it
    /// </summary>
    /// <returns></returns>
    public void SetClockWise()
    {
      // Skip the last point since it is the same as the first
      int result = FunkySheep.Vectors.Utils.IsClockWise(points[1], points[points.Count - 1] , points[0]);
      if (result < 0) {
          points.Reverse();
      }
    }

    /// <summary>
    /// Set the first point of the building (the farest from the center)
    /// </summary>
    public void SetFirstPoint()
    {
        int maxPointIndex = 0;
        Vector2 maxPoint = new Vector2(0, 0);
        for (int i = 0; i < points.Count; i++)
        {
            if (maxPoint.magnitude < points[i].magnitude)
            {
                maxPointIndex = i;
                maxPoint = points[i];
            }
        }

        Vector2[] tempPoints = new Vector2[points.Count];
        points.CopyTo(tempPoints);
        
        for (int i = 0; i < points.Count; i++)
        {
            points[i] = tempPoints[(i + maxPointIndex) %  points.Count];
        }
    }
  }
}
