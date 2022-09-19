using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Game.Player
{
    public class Manager : NetworkBehaviour
    {
        public List<GameObject> NetworkOwnerComponents;

        private void Start()
        {

            if (isLocalPlayer)
            {
                foreach (GameObject component  in NetworkOwnerComponents)
                {
                    component.SetActive(true);
                }
            }
        }
    }
}
