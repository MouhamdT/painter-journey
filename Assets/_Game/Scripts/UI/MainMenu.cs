using UnityEngine;
using UnityEngine.SceneManagement;

namespace PainterJourney.UI
{
    /// <summary>
    /// The main menu's two actions, hooked to the Start and Quit buttons' OnClick in the
    /// Inspector. Start loads a fresh Room01. Quit closes the build, and in the Editor it
    /// stops Play Mode so it can be tested. The EventSystem in the scene handles the
    /// selection, the keyboard and the mouse.
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        // The first room (next in the build list after MainMenu). Not a tuning value.
        private const string FirstRoom = "Room01";

        private bool isLeaving;

        public void StartGame()
        {
            // A second press before the scene changes (Enter twice quickly) does nothing.
            if (isLeaving)
            {
                return;
            }

            isLeaving = true;
            SceneManager.LoadScene(FirstRoom);
        }

        public void QuitGame()
        {
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
