using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Environnement
{
    public class DayNight : MonoBehaviour
    {
        public FunkySheep.Types.Float timeOfDay;
        public Material skybox;
        public Light sun;
        public bool simulate;

        // Update is called once per frame
        void Update()
        {
            timeOfDay.value %= 24;
            if (timeOfDay.value < 0)
            {
                timeOfDay.value = 0;
            }

            float normalizedTimeOfDay = timeOfDay.value / 24;
            sun.transform.rotation = Quaternion.Euler((normalizedTimeOfDay * 360) - 90, 0, 0);

            skybox.SetFloat("_DayNight", (Mathf.Cos(normalizedTimeOfDay * 2 * Mathf.PI) + 1) / 2);
        }

    }

}
