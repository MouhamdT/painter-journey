using PainterJourney.Core;
using PainterJourney.Player;
using PainterJourney.Rooms;
using UnityEngine;

namespace PainterJourney.Enemies
{
    /// <summary>
    /// A paint blob fired by the Shooter. It flies in a straight line at the velocity it was
    /// launched with and disappears when it touches the player (who takes its damage) or a
    /// RoomBounds (the floor, the walls, the ceiling). It flies through everything else,
    /// such as the platform and the enemies. The brush stroke swats it: it disappears and
    /// nobody takes damage. It has no numbers of its own: the Shooter passes them in Launch.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PaintBlob : MonoBehaviour, ISwattable
    {
        private Rigidbody2D rb;
        private int damage;
        private Health pendingTarget;
        private bool isGone;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        /// <summary>Sets the blob's size, velocity and damage. The Shooter calls this right after Instantiate.</summary>
        public void Launch(Vector2 velocity, float size, int blobDamage)
        {
            transform.localScale = new Vector3(size, size, 1f);
            damage = blobDamage;
            rb.linearVelocity = velocity;
        }

        public void Swat()
        {
            // A swat in the same frame as a touch on the player wins: the pending damage is dropped.
            pendingTarget = null;
            Disappear();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isGone || pendingTarget != null)
            {
                return;
            }

            if (other.TryGetComponent<RoomBounds>(out _))
            {
                Disappear();
                return;
            }

            if (other.TryGetComponent<PlayerController>(out _) && other.TryGetComponent(out Health playerHealth))
            {
                // The damage waits until LateUpdate, after this frame's Update has had the chance
                // to start a swing whose stroke swats the blob.
                pendingTarget = playerHealth;
                rb.linearVelocity = Vector2.zero;
            }
        }

        private void LateUpdate()
        {
            if (isGone || pendingTarget == null)
            {
                return;
            }

            // At 0 HP the player's Health ignores the damage, but the blob still disappears.
            pendingTarget.TakeDamage(damage);
            pendingTarget = null;
            Disappear();
        }

        private void Disappear()
        {
            if (isGone)
            {
                return;
            }

            isGone = true;
            rb.linearVelocity = Vector2.zero;
            Destroy(gameObject);
        }
    }
}
