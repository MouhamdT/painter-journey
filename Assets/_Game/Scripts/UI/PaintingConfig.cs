using UnityEngine;

namespace PainterJourney.UI
{
    /// <summary>
    /// All numbers for the painting: which part is revealed, how many parts there are, and the
    /// timing of the brush wipe. One asset lives in Assets/_Game/Config/PaintingConfig.asset.
    /// </summary>
    [CreateAssetMenu(fileName = "PaintingConfig", menuName = "Painter Journey/Painting Config")]
    public class PaintingConfig : ScriptableObject
    {
        [Tooltip("Which panel this screen reveals (1 = leftmost).")]
        [Min(1)]
        public int revealedPart = 1;

        [Tooltip("Number of panels in the painting (4 rooms plus the boss).")]
        [Min(1)]
        public int totalParts = 5;

        [Tooltip("Seconds from the scene starting to the brush wipe starting.")]
        [Min(0f)]
        public float wipeDelay = 0.5f;

        [Tooltip("Seconds the wipe takes to cover the panel from top to bottom.")]
        [Min(0.01f)]
        public float wipeDuration = 1.5f;
    }
}
