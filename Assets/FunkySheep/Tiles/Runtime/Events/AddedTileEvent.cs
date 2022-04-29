using UnityEngine;

namespace FunkySheep.Tiles
{
    [CreateAssetMenu(menuName = "FunkySheep/Tiles/Events/AddedTile")]
    public class AddedTileEvent : FunkySheep.Events.Event<Tile>
    {
    }
}