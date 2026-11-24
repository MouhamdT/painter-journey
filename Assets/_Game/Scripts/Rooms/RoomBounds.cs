using UnityEngine;

namespace PainterJourney.Rooms
{
    /// <summary>
    /// Marks a solid edge of the room (the floor, the walls and the ceiling) that stops paint blobs.
    /// Platforms don't get it, so blobs fly through them. It has no behaviour of its own.
    /// </summary>
    [DisallowMultipleComponent]
    public class RoomBounds : MonoBehaviour
    {
    }
}
