using PainterJourney.Core;
using UnityEngine;

namespace PainterJourney.Enemies
{
    /// <summary>
    /// A target for the brush before the enemies exist. The player walks through it
    /// (its collider is a trigger) and it never hurts anyone. It listens to its Health:
    /// a short red flash on a hit that doesn't kill it, and at 0 HP it disappears.
    /// The room reload brings it back. All numbers come from DummyConfig.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Health))]
    public class TestDummy : MonoBehaviour
    {
        // Placeholder colour from the brush-attack spec. Not a tuning value.
        private static readonly Color HitColor = new Color32(0xD8, 0x4A, 0x4A, 0xFF);

        [SerializeField] private DummyConfig config;

        private SpriteRenderer body;
        private Health health;
        private Color normalColor;
        private int lastHealth;
        private float flashTimeLeft;

        private void Awake()
        {
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

        private void OnHealthChanged(int current, int max)
        {
            // Only a hit that leaves the dummy standing flashes. The killing hit makes it disappear.
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
