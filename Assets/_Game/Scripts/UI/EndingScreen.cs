using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PainterJourney.UI
{
    /// <summary>
    /// The Ending's text and its two actions. On opening it writes "N of M parts painted" from
    /// PaintingConfig. MainMenu and QuitGame are hooked to the buttons' OnClick in the Inspector,
    /// and the EventSystem in the scene handles the selection, the keyboard and the mouse, as on
    /// the main menu.
    /// </summary>
    public class EndingScreen : MonoBehaviour
    {
        // The first scene in the build list. Not a tuning value.
        private const string MenuScene = "MainMenu";

        [SerializeField] private PaintingConfig config;
        [SerializeField] private TMP_Text progressText;

        private bool isLeaving;

        private void Awake()
        {
            progressText.text = $"{config.revealedPart} of {config.totalParts} parts painted";
        }

        public void MainMenu()
        {
            // A second press before the scene changes (Enter twice quickly) does nothing.
            if (isLeaving)
            {
                return;
            }

            isLeaving = true;
            SceneManager.LoadScene(MenuScene);
        }

        public void QuitGame()
        {
            // The same as the main menu's Quit.
            if (isLeaving)
            {
                return;
            }

            isLeaving = true;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
