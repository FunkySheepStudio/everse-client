using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PlaneRace
{
    public class Collide : MonoBehaviour
    {
        public MeshRenderer MeshRenderer;
        public int count = 0;

        private void OnTriggerEnter(Collider other)
        {
            if (count != 0)
            {
                MeshRenderer.material.color = Color.blue;
                transform.parent.GetComponent<Game.PlaneRace.Creator>().enabled = false;
            }
            count += 1;
        }
    }
}
