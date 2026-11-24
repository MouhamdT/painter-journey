using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace PainterJourney.UI
{
    /// <summary>
    /// Runs the painting reveal. After wipeDelay seconds a SpriteMask grows down from the top
    /// edge of the revealed panel over wipeDuration seconds, at an even speed. The coloured art
    /// is "Visible Inside Mask", so only the mask is scaled and the art is never stretched.
    /// When the wipe ends, the part text and the prompt appear, and from then on a new press of
    /// the continue action (Jump, so Space) loads the Ending. All numbers come from PaintingConfig.
    /// </summary>
    public class PaintingReveal : MonoBehaviour
    {
        // The scene that follows (in the build list after PaintingReveal). Not a tuning value.
        private const string NextScene = "Ending";

        [SerializeField] private PaintingConfig config;
        [SerializeField] private InputActionReference continueAction;
        [Tooltip("The grey panels, left to right. The coloured art moves onto the revealed one.")]
        [SerializeField] private Transform[] panels;
        [Tooltip("The coloured art and its mask. Placed over the revealed panel.")]
        [SerializeField] private Transform revealedArt;
        [Tooltip("Pivot at the panel's top edge. Its y scale in the scene is the panel's full height; the wipe grows it from 0 to that.")]
        [SerializeField] private Transform wipePivot;
        [SerializeField] private TMP_Text partText;
        [SerializeField] private TMP_Text promptText;

        private float fullHeight;
        private bool promptVisible;
        private int promptFrame;
        private bool isLeaving;

        private void Awake()
        {
            // Put the coloured art over the panel this screen reveals.
            int index = Mathf.Clamp(config.revealedPart, 1, panels.Length) - 1;
            Vector3 artPosition = revealedArt.position;
            artPosition.x = panels[index].position.x;
            revealedArt.position = artPosition;

            // Start with nothing uncovered and no text.
            fullHeight = wipePivot.localScale.y;
            SetWipe(0f);

            partText.text = $"Part {config.revealedPart} of {config.totalParts}";
            partText.gameObject.SetActive(false);
            promptText.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            // InputSystem_Actions is the project-wide actions asset. Enabling is safe to repeat.
            continueAction.action.Enable();
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(config.wipeDelay);

            float elapsed = 0f;
            while (elapsed < config.wipeDuration)
            {
                SetWipe(elapsed / config.wipeDuration);
                yield return null;
                elapsed += Time.deltaTime;
            }

            SetWipe(1f);
            partText.gameObject.SetActive(true);
            promptText.gameObject.SetActive(true);
            promptVisible = true;
            promptFrame = Time.frameCount;
        }

        private void Update()
        {
            // Only a press that starts after the prompt appeared counts: a press during the
            // wipe, or a key held since before the prompt, does nothing.
            if (!promptVisible || isLeaving || Time.frameCount <= promptFrame)
            {
                return;
            }

            if (continueAction.action.WasPressedThisFrame())
            {
                isLeaving = true;
                SceneManager.LoadScene(NextScene);
            }
        }

        private void SetWipe(float progress)
        {
            Vector3 scale = wipePivot.localScale;
            scale.y = fullHeight * Mathf.Clamp01(progress);
            wipePivot.localScale = scale;
        }
    }
}
