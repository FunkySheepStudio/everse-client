using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Environnement
{
    public class DayNight : MonoBehaviour
    {
        [Range(0, 1)]
        public float time = 1;
        public Material skybox;
        public Light sun;
        public bool simulate;

        // Update is called once per frame
        void Update()
        {
            if (simulate)
            {
                Simulate();
            }

            sun.transform.localRotation = Quaternion.Euler(time * 360, 0, 0);
            skybox.SetFloat("_DayNight", time);
        }

        public void Simulate()
        {
            time += Time.deltaTime / 24;
            time %= 1;
        }
    }

}
