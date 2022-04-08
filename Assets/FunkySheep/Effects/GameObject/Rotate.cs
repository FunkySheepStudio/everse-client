using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Effects
{
  public class Rotate : MonoBehaviour
  {
      // Update is called once per frame
      void Update()
      {
        transform.Rotate(Vector3.up);
      }
  }  
}
