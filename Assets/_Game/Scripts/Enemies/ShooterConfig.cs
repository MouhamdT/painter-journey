using UnityEngine;

namespace PainterJourney.Enemies
{
    /// <summary>
    /// All tuning numbers for enemy type B, the Shooter, and its paint blobs.
    /// One asset lives in Assets/_Game/Config/ShooterConfig.asset.
    /// </summary>
    [CreateAssetMenu(fileName = "ShooterConfig", menuName = "Painter Journey/Shooter Config")]
    public class ShooterConfig : ScriptableObject
    {
        [Tooltip("The Shooter's HP. Each brush hit takes 1.")]
        [Min(1)]
        public int maxHealth = 2;

        [Tooltip("Seconds from the room starting to the first blob.")]
        [Min(0f)]
        public float firstShotDelay = 2f;

        [Tooltip("Seconds between blobs after the first.")]
        [Min(0.01f)]
        public float fireInterval = 2f;

        [Tooltip("Units per second, in a straight line toward where the player was at the moment of firing.")]
        [Min(0f)]
        public float blobSpeed = 5f;

        [Tooltip("Width and height of a blob in units.")]
        [Min(0.01f)]
        public float blobSize = 0.4f;

        [Tooltip("Damage to the player per blob.")]
        [Min(1)]
        public int blobDamage = 1;

        [Tooltip("Seconds it stays red after a brush hit that doesn't kill it.")]
        [Min(0f)]
        public float hitFlashDuration = 0.15f;
    }
}
