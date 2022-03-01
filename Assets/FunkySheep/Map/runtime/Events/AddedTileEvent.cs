using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Map
{
    [CreateAssetMenu(menuName = "FunkySheep/Map/Events/AddedTile")]
    public class AddedTileEvent : FunkySheep.Events.Event<Tile>
    {
    }
}