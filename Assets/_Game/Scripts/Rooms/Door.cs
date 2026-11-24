using PainterJourney.Core;
using PainterJourney.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PainterJourney.Rooms
{
    /// <summary>
    /// The room's exit. Closed, it is a solid dark block (a child with its own sprite and collider).
    /// Open() turns the block off and shows the yellow glow on this object, with its exit trigger.
    /// The moment the living player touches the open doorway, the next scene loads. This object
    /// and the block both carry RoomBounds, so paint blobs stop at the doorway, closed or open.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Door : MonoBehaviour
    {
        // The scene the doorway leads to (in the build list after Room01). Not a tuning value.
        private const string NextScene = "PaintingReveal";

        [Tooltip("The closed block: its sprite and solid collider turn off when the door opens.")]
        [SerializeField] private GameObject closedBlock;

        private SpriteRenderer glow;
        private BoxCollider2D exitTrigger;
        private bool isOpen;
        private bool isLeaving;

        private void Awake()
        {
            glow = GetComponent<SpriteRenderer>();
            exitTrigger = GetComponent<BoxCollider2D>();

            // Every room starts with the door closed.
            closedBlock.SetActive(true);
            glow.enabled = false;
            exitTrigger.enabled = false;
        }

        /// <summary>Opens the door. RoomClear calls this once the room is clear.</summary>
        public void Open()
        {
            if (isOpen)
            {
                return;
            }

            isOpen = true;
            closedBlock.SetActive(false);
            glow.enabled = true;

            // Turning the trigger on also reports a player who is already standing in the doorway.
            exitTrigger.enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isOpen || isLeaving)
            {
                return;
            }

            if (!other.TryGetComponent<PlayerController>(out _) || !other.TryGetComponent(out Health playerHealth))
            {
                return;
            }

            // A dead player stays frozen until the room reloads, so it never leaves.
            if (playerHealth.IsDead)
            {
                return;
            }

            isLeaving = true;
            SceneManager.LoadScene(NextScene);
        }
    }
}
