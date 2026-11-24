using System.Collections;
using PainterJourney.Core;
using PainterJourney.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PainterJourney.Rooms
{
    /// <summary>
    /// Reloads the active scene restartDelay seconds after the player dies.
    /// Works in any room: give it the player's Health and the PlayerConfig in the Inspector.
    /// </summary>
    public class RoomRestart : MonoBehaviour
    {
        [SerializeField] private Health playerHealth;
        [SerializeField] private PlayerConfig config;

        private bool restarting;

        private void OnEnable()
        {
            playerHealth.Died += OnPlayerDied;
        }

        private void OnDisable()
        {
            playerHealth.Died -= OnPlayerDied;
        }

        private void OnPlayerDied()
        {
            // Health announces the death only once, but the reload must never start twice.
            if (restarting)
            {
                return;
            }

            restarting = true;
            StartCoroutine(ReloadAfterDelay());
        }

        private IEnumerator ReloadAfterDelay()
        {
            yield return new WaitForSeconds(config.restartDelay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
