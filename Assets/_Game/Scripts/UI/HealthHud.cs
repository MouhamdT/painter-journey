using System.Collections.Generic;
using PainterJourney.Core;
using UnityEngine;
using UnityEngine.UI;

namespace PainterJourney.UI
{
    /// <summary>
    /// Shows HP as a row of pips, one per maximum HP. Full pips are orange and empty pips
    /// dark grey; the right-most full pip empties first. Listens to Health.Changed, so the
    /// game code never knows about the HUD. Pip size and spacing come from the pip template
    /// and the layout group in the scene.
    /// </summary>
    public class HealthHud : MonoBehaviour
    {
        // Placeholder colours from the health spec. Not tuning values.
        private static readonly Color FullColor = new Color32(0xE8, 0xA3, 0x3D, 0xFF);
        private static readonly Color EmptyColor = new Color32(0x3A, 0x3A, 0x3A, 0xFF);

        [SerializeField] private Health health;
        [Tooltip("An inactive pip inside the layout group. It is copied once per maximum HP.")]
        [SerializeField] private Image pipTemplate;

        private readonly List<Image> pips = new List<Image>();

        private void OnEnable()
        {
            health.Changed += Refresh;

            // Draw what Health holds now. If its owner hasn't initialized it yet, the
            // Changed event from Initialize draws the pips instead, still before the first frame.
            Refresh(health.Current, health.Max);
        }

        private void OnDisable()
        {
            health.Changed -= Refresh;
        }

        private void Refresh(int current, int max)
        {
            while (pips.Count < max)
            {
                Image pip = Instantiate(pipTemplate, pipTemplate.transform.parent);
                pip.name = "Pip" + (pips.Count + 1);
                pip.gameObject.SetActive(true);
                pips.Add(pip);
            }

            while (pips.Count > max)
            {
                int last = pips.Count - 1;
                Destroy(pips[last].gameObject);
                pips.RemoveAt(last);
            }

            for (int i = 0; i < pips.Count; i++)
            {
                pips[i].color = i < current ? FullColor : EmptyColor;
            }
        }
    }
}
