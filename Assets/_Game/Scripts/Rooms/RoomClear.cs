using System.Collections;
using System.Collections.Generic;
using PainterJourney.Core;
using UnityEngine;

namespace PainterJourney.Rooms
{
    /// <summary>
    /// The room is clear when every enemy in its list has died, in any order. Then it opens the
    /// door doorOpenDelay seconds later, once. Give it the enemies' Health, the door and the
    /// RoomConfig in the Inspector. The room reload brings everything back with the door closed.
    /// </summary>
    public class RoomClear : MonoBehaviour
    {
        [Tooltip("Every enemy that must die before the door opens.")]
        [SerializeField] private List<Health> enemies = new List<Health>();
        [SerializeField] private Door door;
        [SerializeField] private RoomConfig config;

        private bool cleared;

        private void OnEnable()
        {
            foreach (Health enemy in enemies)
            {
                enemy.Died += OnEnemyDied;
            }
        }

        private void OnDisable()
        {
            foreach (Health enemy in enemies)
            {
                enemy.Died -= OnEnemyDied;
            }
        }

        private void OnEnemyDied()
        {
            // Two enemies dying in the same frame both land here: the door still opens only once.
            if (cleared || !AllDead())
            {
                return;
            }

            cleared = true;
            StartCoroutine(OpenDoorAfterDelay());
        }

        private bool AllDead()
        {
            foreach (Health enemy in enemies)
            {
                if (!enemy.IsDead)
                {
                    return false;
                }
            }
            return true;
        }

        private IEnumerator OpenDoorAfterDelay()
        {
            yield return new WaitForSeconds(config.doorOpenDelay);
            door.Open();
        }
    }
}
