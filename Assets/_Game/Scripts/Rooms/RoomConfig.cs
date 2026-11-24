using UnityEngine;

namespace PainterJourney.Rooms
{
    /// <summary>
    /// All tuning numbers for a room: clearing it and its door. One asset lives in Assets/_Game/Config/RoomConfig.asset.
    /// </summary>
    [CreateAssetMenu(fileName = "RoomConfig", menuName = "Painter Journey/Room Config")]
    public class RoomConfig : ScriptableObject
    {
        [Tooltip("Seconds from the last enemy dying to the door opening.")]
        [Min(0f)]
        public float doorOpenDelay = 0.5f;
    }
}
