using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Environnement
{
    public class DayNight : MonoBehaviour
    {
        [Range(-1, 1)]
        public float time = 1;
        public Material skybox;
        public Light sun;
        public bool simulate;

        // Update is called once per frame
        void Update()
        {
            float lightDay = (Mathf.Cos(Time.time / 5) + 1) * 0.5f;
            float shaderDay = (Mathf.Sin(Time.time / 5) + 1) * 0.5f;
            sun.transform.rotation = Quaternion.Euler(lightDay * 360, 0, 0);
            skybox.SetFloat("_DayNight", shaderDay);
        }

    }

}
