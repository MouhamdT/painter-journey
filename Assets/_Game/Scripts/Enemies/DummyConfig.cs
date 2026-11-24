using UnityEngine;

namespace PainterJourney.Enemies
{
    /// <summary>
    /// Tuning numbers for the test dummy in Room01. One asset lives in Assets/_Game/Config/DummyConfig.asset.
    /// The dummy is a test object; the enemy-type-a spec removes it.
    /// </summary>
    [CreateAssetMenu(fileName = "DummyConfig", menuName = "Painter Journey/Dummy Config")]
    public class DummyConfig : ScriptableObject
    {
        [Tooltip("The dummy's HP.")]
        [Min(1)]
        public int maxHealth = 3;

        [Tooltip("Seconds the dummy stays red after a hit that doesn't kill it.")]
        [Min(0f)]
        public float hitFlashDuration = 0.15f;
    }
}
