using PainterJourney.Core;
using UnityEngine;

namespace PainterJourney.Enemies
{
    /// <summary>
    /// Enemy type B. It stands still and fires a paint blob firstShotDelay seconds after the room
    /// starts, then one every fireInterval seconds until it dies. Each blob starts at its centre
    /// and flies at blobSpeed toward the player's centre as it was at the moment of firing.
    /// Touching it is harmless (its collider is a trigger). It listens to its Health: a short red
    /// flash on a hit that doesn't kill it, and at 0 HP it disappears and stops firing.
    /// The room reload brings it back. All numbers come from ShooterConfig.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Health))]
    public class Shooter : MonoBehaviour
    {
        // Closer than this to the player's centre counts as no direction to aim in. Not a tuning value.
        private const float MinAimDistance = 0.0001f;

        // Placeholder colour from the enemy-type-b spec. Not a tuning value.
        private static readonly Color HitColor = new Color32(0xD8, 0x4A, 0x4A, 0xFF);

        [SerializeField] private ShooterConfig config;
        [Tooltip("The player it aims at. Its position at the moment of firing is the target.")]
        [SerializeField] private Transform player;
        [Tooltip("The blob prefab, created once per shot.")]
        [SerializeField] private PaintBlob blobPrefab;

        private SpriteRenderer body;
        private Health health;
        private Color normalColor;
        private int lastHealth;
        private float flashTimeLeft;
        private float nextShotTime;

        private void Awake()
        {
            body = GetComponent<SpriteRenderer>();
            normalColor = body.color;

            health = GetComponent<Health>();
            health.Initialize(config.maxHealth);
            lastHealth = health.Current;

            // The room starts when the scene loads, so the first-shot timer starts here too.
            nextShotTime = Time.time + config.firstShotDelay;
        }

        private void OnEnable()
        {
            health.Changed += OnHealthChanged;
            health.Died += OnDied;
        }

        private void OnDisable()
        {
            health.Changed -= OnHealthChanged;
            health.Died -= OnDied;
        }

        private void Update()
        {
            UpdateHitFlash();

            if (Time.time >= nextShotTime)
            {
                Fire();
                // Counted from the planned time, not from this frame, so the rhythm doesn't drift.
                nextShotTime += config.fireInterval;
            }
        }

        private void Fire()
        {
            Vector2 origin = transform.position;
            Vector2 toPlayer = (Vector2)player.position - origin;

            // The player standing inside the Shooter: no direction to aim in, so it fires straight left.
            Vector2 direction = toPlayer.sqrMagnitude > MinAimDistance * MinAimDistance ? toPlayer.normalized : Vector2.left;

            PaintBlob blob = Instantiate(blobPrefab, origin, Quaternion.identity);
            blob.Launch(direction * config.blobSpeed, config.blobSize, config.blobDamage);
        }

        private void UpdateHitFlash()
        {
            if (flashTimeLeft <= 0f)
            {
                return;
            }

            flashTimeLeft -= Time.deltaTime;
            if (flashTimeLeft <= 0f)
            {
                body.color = normalColor;
            }
        }

        private void OnHealthChanged(int current, int max)
        {
            // Only a hit that leaves the Shooter standing flashes. The killing hit makes it disappear.
            if (current < lastHealth && current > 0)
            {
                // A hit during the flash restarts it.
                flashTimeLeft = config.hitFlashDuration;
                body.color = HitColor;
            }
            lastHealth = current;
        }

        private void OnDied()
        {
            // Blobs already in the air are not its children, so they keep flying.
            gameObject.SetActive(false);
        }
    }
}
