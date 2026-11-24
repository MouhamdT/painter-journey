using System;
using UnityEngine;

namespace PainterJourney.Core
{
    /// <summary>
    /// The only place HP changes, for the player and the enemies.
    /// It has no number field of its own: the owner passes its maximum from its own config
    /// (for the player, PlayerConfig.maxHealth) by calling Initialize in Awake.
    /// </summary>
    [DisallowMultipleComponent]
    public class Health : MonoBehaviour
    {
        /// <summary>Raised when HP changes, with the current and the maximum HP.</summary>
        public event Action<int, int> Changed;

        /// <summary>Raised once, when HP reaches 0.</summary>
        public event Action Died;

        public int Current { get; private set; }
        public int Max { get; private set; }
        public bool IsDead { get; private set; }

        /// <summary>Sets the maximum and fills HP to it. The owner calls this once, in Awake.</summary>
        public void Initialize(int maxHealth)
        {
            Max = Mathf.Max(1, maxHealth);
            Current = Max;
            IsDead = false;
            Changed?.Invoke(Current, Max);
        }

        public void TakeDamage(int amount)
        {
            if (amount <= 0 || IsDead || Max <= 0)
            {
                return;
            }

            Current = Mathf.Max(0, Current - amount);
            Changed?.Invoke(Current, Max);

            if (Current == 0)
            {
                IsDead = true;
                Died?.Invoke();
            }
        }

        public void Heal(int amount)
        {
            if (amount <= 0 || IsDead || Max <= 0)
            {
                return;
            }

            int healed = Mathf.Min(Max, Current + amount);
            if (healed == Current)
            {
                return;
            }

            Current = healed;
            Changed?.Invoke(Current, Max);
        }

        // Test-only items in the component's ⋮ menu (Play Mode).
        [ContextMenu("Take 1 damage")]
        private void TakeOneDamage()
        {
            TakeDamage(1);
        }

        [ContextMenu("Heal 1")]
        private void HealOne()
        {
            Heal(1);
        }
    }
}
