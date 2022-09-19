using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Game.Player.States
{
    [Serializable]
    public class StartingState : BaseState
    {
        public List<GameObject> NetworkOwnerComponents;
        bool isSpawnPositionSet = false;

        public override void EnterState(Manager player)
        {
            if (player.isLocalPlayer)
            {
                player.SyncInitialPosition(player.InitialLatitude.value, player.InitialLongitude.value);
            }
        }

        public override void UpdateState(Manager player)
        {
            if (!isSpawnPositionSet && player.isLocalPlayer)
            {
                SetSpawningPosition(player);
            }
        }

        public void SetSpawningPosition(Manager player)
        {
            Vector2 spawningPosition = Game.Manager.Instance.earthManager.CalculatePosition(player.syncInitialLatitude, player.syncInitialLongitude);

            float? height = FunkySheep.Earth.Terrain.Manager.GetHeight(spawningPosition);
            if (height != null)
            {
                player.transform.position = new Vector3(
                    spawningPosition.x,
                    height.Value,
                    spawningPosition.y
                    );

                isSpawnPositionSet = true;
                ActivateComponents(player);
            }
        }

        void ActivateComponents(Manager player)
        {
            foreach (GameObject component in NetworkOwnerComponents)
            {
                component.SetActive(true);
            }

            player.SwitchState(player.walkingState);
        }
    }

}
