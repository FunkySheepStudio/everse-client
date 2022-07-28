using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Game.Zombies
{
    public class RotateGenerators : MonoBehaviour
    {
        public float speed = 10;
        // Update is called once per frame
        void Update()
        {
            transform.Rotate(Vector3.forward, Time.deltaTime * speed);
        }
    }
}
