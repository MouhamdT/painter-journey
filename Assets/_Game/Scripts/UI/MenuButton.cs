using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PainterJourney.UI
{
    /// <summary>
    /// Makes a menu button follow the selection. Hovering the button with the mouse selects
    /// it, so only one button is yellow at a time (the Button's colour tint shows the yellow
    /// for the selected state). The label is dark while the button is selected and white
    /// otherwise, because the colour tint only changes the background image.
    /// </summary>
    [RequireComponent(typeof(Selectable))]
    public class MenuButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler
    {
        // Placeholder colours from the main menu spec. Not tuning values.
        private static readonly Color NormalTextColor = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
        private static readonly Color SelectedTextColor = new Color32(0x1E, 0x1E, 0x1E, 0xFF);

        [SerializeField] private TMP_Text label;

        private Selectable selectable;

        private void Awake()
        {
            selectable = GetComponent<Selectable>();
            label.color = NormalTextColor;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            selectable.Select();
        }

        public void OnSelect(BaseEventData eventData)
        {
            label.color = SelectedTextColor;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            label.color = NormalTextColor;
        }
    }
}
