using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Roads
{
  public class PlayerDetection : MonoBehaviour
  {
    public GameObject player;
    public List<Vector3> points = new List<Vector3>();
    
    private void LateUpdate() {

      int layerMask = 1 << 10;

      for (int i = 0; i < points.Count; i++)
      {
        if (i != 0)
        {
          // Bit shift the index of the layer (8) to get a bit mask

          RaycastHit hit;
          Debug.DrawLine(points[i - 1], points[i]);
          // Does the ray intersect any objects excluding the player layer
          if (Physics.Linecast(points[i - 1], points[i], out hit, layerMask))
          {
            if (player.GetComponent<Game.Player.OnTheRoad>() == null)
            {
              player.AddComponent<Game.Player.OnTheRoad>();
            } else {
              player.GetComponent<Game.Player.OnTheRoad>().enabled = true;
            }
            player.GetComponent<Game.Player.OnTheRoad>().lastHit = 0;
          }
        }
      }
    }
  }  
}
