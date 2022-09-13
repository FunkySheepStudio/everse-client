using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.Location
{
    public class Manager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
#if !UNITY_SERVER
            if (!FunkySheep.Gps.Manager.Instance)
            {
                // Get the last known GPS ccordinates from server
            }   
#endif
        }
    }
}
