using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Environnement
{
    public class DayNight : MonoBehaviour
    {
        public float speed = 0.5f;
        public FunkySheep.Types.Float timeOfDay;
        public Material skybox;
        public Light sun;
        public bool simulate;
        public List<Material> nightLightsMaterials;

        // Update is called once per frame
        void Update()
        {
            if (simulate)
            {
                timeOfDay.value += Time.deltaTime * speed;
            }

            timeOfDay.value %= 24;
            if (timeOfDay.value < 0)
            {
                timeOfDay.value = 0;
            }

            float normalizedTimeOfDay = timeOfDay.value / 24;
            sun.transform.rotation = Quaternion.Euler((normalizedTimeOfDay * 360) - 90, 0, 0);

            skybox.SetFloat("_DayNight", (Mathf.Cos(normalizedTimeOfDay * 2 * Mathf.PI) + 1) / 2);

            foreach (Material material in nightLightsMaterials)
            {
                material.SetFloat("_DayNight", (Mathf.Cos(normalizedTimeOfDay * 2 * Mathf.PI) + 1) / 2);
            }
        }

    }

}
