using PainterJourney.Core;
using PainterJourney.Player;
using UnityEngine;

namespace PainterJourney.Enemies
{
    /// <summary>
    /// Enemy type A. It stands still until the player's centre is within noticeRange sideways,
    /// then walks along the floor toward the player's x at moveSpeed until it dies. It never jumps.
    /// A touch hurts the player for contactDamage, then it stands still for hitPause seconds and
    /// can't hurt. The player passes through it (its collider is a trigger). It listens to its
    /// Health: a short red flash on a hit that doesn't kill it, and at 0 HP it disappears.
    /// The room reload brings it back. All numbers come from ChaserConfig.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Health))]
    public class Chaser : MonoBehaviour
    {
        // Closer than this to the player's x counts as arrived, so it stands still with a
        // velocity of exactly 0 instead of creeping. Not a tuning value.
        private const float ArriveDistance = 0.01f;

        // The pause counts down in physics steps, and the sum of the steps is a little off
        // (50 × 0.0199999 leaves 0.0000005), which would add a whole extra step to the pause.
        // Not a tuning value.
        private const float TimerTolerance = 0.0001f;

        // Placeholder colour from the enemy-type-a spec. Not a tuning value.
        private static readonly Color HitColor = new Color32(0xD8, 0x4A, 0x4A, 0xFF);

        [SerializeField] private ChaserConfig config;
        [Tooltip("The player it watches and chases. Only its x is used.")]
        [SerializeField] private Transform player;

        private Rigidbody2D rb;
        private SpriteRenderer body;
        private Health health;
        private Color normalColor;
        private int lastHealth;
        private float flashTimeLeft;
        private bool isChasing;
        private float pauseTimeLeft;

        private bool IsPausing => pauseTimeLeft > TimerTolerance;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            body = GetComponent<SpriteRenderer>();
            normalColor = body.color;

            health = GetComponent<Health>();
            health.Initialize(config.maxHealth);
            lastHealth = health.Current;
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

        private void FixedUpdate()
        {
            float toPlayer = player.position.x - rb.position.x;

            // Exactly at noticeRange counts as noticed. Once it chases, it never goes back.
            if (!isChasing && Mathf.Abs(toPlayer) <= config.noticeRange)
            {
                isChasing = true;
            }

            if (IsPausing)
            {
                pauseTimeLeft -= Time.fixedDeltaTime;
            }

            rb.linearVelocity = new Vector2(ChaseSpeed(toPlayer), 0f);
        }

        private void Update()
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

        private void OnTriggerStay2D(Collider2D other)
        {
            // Stay (not Enter): after the pause, a player still inside is hurt again at once.
            if (IsPausing || health.IsDead)
            {
                return;
            }

            if (!other.TryGetComponent<PlayerController>(out _) || !other.TryGetComponent(out Health playerHealth))
            {
                return;
            }

            playerHealth.TakeDamage(config.contactDamage);
            pauseTimeLeft = config.hitPause;
        }

        private float ChaseSpeed(float toPlayer)
        {
            if (!isChasing || IsPausing || Mathf.Abs(toPlayer) <= ArriveDistance)
            {
                return 0f;
            }

            // On the last step it moves only the distance that is left, so it stops right
            // under the player instead of stepping past and turning back (no shaking).
            float speed = Mathf.Min(config.moveSpeed, Mathf.Abs(toPlayer) / Time.fixedDeltaTime);
            return Mathf.Sign(toPlayer) * speed;
        }

        private void OnHealthChanged(int current, int max)
        {
            // Only a hit that leaves the Chaser standing flashes. The killing hit makes it disappear.
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
            gameObject.SetActive(false);
        }
    }
}
