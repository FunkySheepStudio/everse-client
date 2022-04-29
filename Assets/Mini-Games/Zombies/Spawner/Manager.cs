using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.WIP.Zombies.Spawner
{
    public class Manager : MonoBehaviour
    {
        public GameObject zombiePrefab;

        [Range(0, 60)]
        public float spawnRate = 10000;
        float lastSpawn = 0;
       
        // Update is called once per frame
        void Update()
        {
            lastSpawn += Time.deltaTime;
            if (lastSpawn >= spawnRate)
            {
                GameObject.Instantiate(zombiePrefab, transform);
                lastSpawn = 0;
            }
        }
    }

}
