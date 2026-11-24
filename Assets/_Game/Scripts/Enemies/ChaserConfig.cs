using UnityEngine;

namespace PainterJourney.Enemies
{
    /// <summary>
    /// All tuning numbers for enemy type A, the Chaser. One asset lives in Assets/_Game/Config/ChaserConfig.asset.
    /// </summary>
    [CreateAssetMenu(fileName = "ChaserConfig", menuName = "Painter Journey/Chaser Config")]
    public class ChaserConfig : ScriptableObject
    {
        [Tooltip("The Chaser's HP. Each brush hit takes 1.")]
        [Min(1)]
        public int maxHealth = 3;

        [Tooltip("Units per second while chasing, along the floor toward the player's x.")]
        [Min(0f)]
        public float moveSpeed = 4f;

        [Tooltip("It starts chasing when the player's centre is this close sideways (any height). Once it chases, it never stops.")]
        [Min(0f)]
        public float noticeRange = 6f;

        [Tooltip("Damage to the player per touch.")]
        [Min(1)]
        public int contactDamage = 1;

        [Tooltip("Seconds it stands still and can't hurt after it hurts the player.")]
        [Min(0f)]
        public float hitPause = 1f;

        [Tooltip("Seconds it stays red after a brush hit that doesn't kill it.")]
        [Min(0f)]
        public float hitFlashDuration = 0.15f;
    }
}
