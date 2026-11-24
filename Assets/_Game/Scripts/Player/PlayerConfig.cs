using UnityEngine;

namespace PainterJourney.Player
{
    /// <summary>
    /// All tuning numbers for the player's movement, health and brush attack. One asset lives in Assets/_Game/Config/PlayerConfig.asset.
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Painter Journey/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        [Tooltip("Horizontal speed in units per second, on the ground and in the air.")]
        public float moveSpeed = 7f;

        [Tooltip("How many units the feet rise when Space is held for a full jump.")]
        public float jumpHeight = 3.5f;

        [Tooltip("Gravity scale applied to the player's Rigidbody2D.")]
        public float gravityScale = 3f;

        [Tooltip("Upward speed is multiplied by this once when Space is released while rising.")]
        [Range(0f, 1f)]
        public float jumpCutMultiplier = 0.65f;

        [Tooltip("Below this world y the player is put back at its start position.")]
        public float fallResetY = -10f;

        [Header("Health")]
        [Tooltip("The player's HP at the start of the room.")]
        [Min(1)]
        public int maxHealth = 3;

        [Tooltip("Seconds the player stays red after a hit that doesn't kill it.")]
        [Min(0f)]
        public float hitFlashDuration = 0.15f;

        [Tooltip("Seconds between reaching 0 HP and the room reloading.")]
        [Min(0f)]
        public float restartDelay = 1f;

        [Header("Attack")]
        [Tooltip("Width of the brush's hit box in units, measured from the player's front edge outward.")]
        [Min(0.01f)]
        public float attackWidth = 1.5f;

        [Tooltip("Height of the brush's hit box in units, centred on the player's centre.")]
        [Min(0.01f)]
        public float attackHeight = 1.2f;

        [Tooltip("Seconds the hit box and the blue stroke stay active per swing.")]
        [Min(0.01f)]
        public float attackDuration = 0.1f;

        [Tooltip("Damage to each target hit by a swing.")]
        [Min(1)]
        public int attackDamage = 1;
    }
}
