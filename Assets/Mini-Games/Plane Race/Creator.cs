using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PlaneRace
{
    public class Creator : MonoBehaviour
    {
        public FunkySheep.Types.Vector3 playerPosition;
        public FunkySheep.Types.Quaternion playerRotation;
        public GameObject gate;
        Vector3 _lastPosition;
        // Start is called before the first frame update
        void Start()
        {
            _lastPosition = playerPosition.value;
        }

        // Update is called once per frame
        void Update()
        {
            if (Vector3.Distance(_lastPosition, playerPosition.value) >= 300)
            {
                Spawn();
                _lastPosition = playerPosition.value;
            }
        }

        void Spawn()
        {
            GameObject gateGo = GameObject.Instantiate(gate, transform);
            gateGo.transform.position = playerPosition.value;
            gateGo.transform.rotation = playerRotation.value;
        }
    }

}
