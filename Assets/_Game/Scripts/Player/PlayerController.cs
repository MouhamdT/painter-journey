using PainterJourney.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PainterJourney.Player
{
    /// <summary>
    /// Runs left and right and jumps. Instant start and stop, full air control,
    /// a jump that can be cut short by releasing Space, and no sticking to walls.
    /// Listens to its Health: a short red flash on a hit, and at 0 HP it turns dark,
    /// freezes in place and ignores input. All numbers come from PlayerConfig.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Health))]
    public class PlayerController : MonoBehaviour
    {
        // A contact counts as ground when its normal points mostly up,
        // and as a wall when it points mostly sideways. Not tuning values.
        private const float GroundNormalMinY = 0.7f;
        private const float WallNormalMinX = 0.7f;
        private const float InputDeadZone = 0.01f;

        // Placeholder colours from the health spec. Not tuning values.
        private static readonly Color HitColor = new Color32(0xD8, 0x4A, 0x4A, 0xFF);
        private static readonly Color DeadColor = new Color32(0x2A, 0x2A, 0x2A, 0xFF);

        [SerializeField] private PlayerConfig config;
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference jumpAction;
        [Tooltip("Child that shows the facing. Its local x is mirrored when the player turns.")]
        [SerializeField] private Transform facingMarker;

        private readonly ContactPoint2D[] contacts = new ContactPoint2D[16];

        private Rigidbody2D rb;
        private SpriteRenderer body;
        private Health health;
        private Color normalColor;
        private int lastHealth;
        private float flashTimeLeft;
        private bool isDead;
        private Vector2 startPosition;
        private float markerLocalX;
        private int facing = 1;
        private bool canCutJump;
        private bool jumpedSincePhysicsStep;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = config.gravityScale;
            startPosition = rb.position;
            markerLocalX = Mathf.Abs(facingMarker.localPosition.x);
            SetFacing(1);

            // The player's own sprite flashes, not the facing marker's.
            body = GetComponent<SpriteRenderer>();
            normalColor = body.color;

            health = GetComponent<Health>();
            health.Initialize(config.maxHealth);
            lastHealth = health.Current;
        }

        private void OnEnable()
        {
            // InputSystem_Actions is the project-wide actions asset, so other scripts share
            // these actions. Enabling is safe to repeat; they are not disabled here.
            moveAction.action.Enable();
            jumpAction.action.Enable();

            health.Changed += OnHealthChanged;
            health.Died += OnDied;
        }

        private void OnDisable()
        {
            health.Changed -= OnHealthChanged;
            health.Died -= OnDied;
        }

        private void FixedUpdate()
        {
            // The physics step that follows refreshes the contacts.
            jumpedSincePhysicsStep = false;
        }

        private void Update()
        {
            // Dead: frozen and dark until the room reloads. All input is ignored.
            if (isDead)
            {
                return;
            }

            UpdateHitFlash();

            if (transform.position.y < config.fallResetY)
            {
                ResetToStart();
                return;
            }

            ReadContacts(out bool grounded, out bool wallLeft, out bool wallRight);

            int direction = ReadMoveDirection();
            if (direction != 0)
            {
                SetFacing(direction);
            }

            Vector2 velocity = rb.linearVelocity;

            // Instant start and stop, the same on the ground and in the air.
            velocity.x = direction * config.moveSpeed;

            // Pushing into a wall: stand still against it instead of pressing into it.
            if ((direction > 0 && wallRight) || (direction < 0 && wallLeft))
            {
                velocity.x = 0f;
            }

            // Every jump needs a fresh press, and only from the ground.
            if (jumpAction.action.WasPressedThisFrame() && grounded && !jumpedSincePhysicsStep)
            {
                velocity.y = JumpTakeOffSpeed();
                canCutJump = true;
                jumpedSincePhysicsStep = true;
            }

            // Releasing Space while rising cuts the upward speed once.
            if (canCutJump && velocity.y <= 0f)
            {
                canCutJump = false;
            }
            if (canCutJump && jumpAction.action.WasReleasedThisFrame())
            {
                velocity.y *= config.jumpCutMultiplier;
                canCutJump = false;
            }

            rb.linearVelocity = velocity;
        }

        private void OnHealthChanged(int current, int max)
        {
            // Only a hit that leaves the player alive flashes. Heals and the killing hit don't.
            if (current < lastHealth && current > 0)
            {
                // A hit during the flash restarts it, so the red lasts until after the last hit.
                flashTimeLeft = config.hitFlashDuration;
                body.color = HitColor;
            }
            lastHealth = current;
        }

        private void OnDied()
        {
            isDead = true;
            flashTimeLeft = 0f;
            body.color = DeadColor;

            // Stop in place, also in mid-air: no falling, no sliding.
            rb.linearVelocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        private void UpdateHitFlash()
        {
            if (flashTimeLeft <= 0f)
            {
                return;
            }

            flashTimeLeft -= Time.deltaTime;
            if (flashTimeLeft <= 0f)
            {
                body.color = normalColor;
            }
        }

        private float JumpTakeOffSpeed()
        {
            // √(2 × g × jumpHeight) is exact for continuous motion. Physics runs in steps of
            // fixedDeltaTime and applies gravity before it moves the body, which loses about
            // half a step of rise (3.36 instead of 3.5 units). The g·dt/2 terms put that back,
            // so the feet really rise jumpHeight.
            float gravity = Mathf.Abs(Physics2D.gravity.y) * config.gravityScale;
            float halfStep = 0.5f * gravity * Time.fixedDeltaTime;
            return halfStep + Mathf.Sqrt(halfStep * halfStep + 2f * gravity * config.jumpHeight);
        }

        private int ReadMoveDirection()
        {
            // Only the x of Move matters, and any push counts as full speed.
            float x = moveAction.action.ReadValue<Vector2>().x;
            if (x > InputDeadZone)
            {
                return 1;
            }
            if (x < -InputDeadZone)
            {
                return -1;
            }
            return 0;
        }

        private void ReadContacts(out bool grounded, out bool wallLeft, out bool wallRight)
        {
            grounded = false;
            wallLeft = false;
            wallRight = false;

            int count = rb.GetContacts(contacts);
            for (int i = 0; i < count; i++)
            {
                Vector2 normal = contacts[i].normal;
                if (normal.y >= GroundNormalMinY)
                {
                    grounded = true;
                }
                else if (normal.x >= WallNormalMinX)
                {
                    wallLeft = true;
                }
                else if (normal.x <= -WallNormalMinX)
                {
                    wallRight = true;
                }
            }
        }

        private void SetFacing(int direction)
        {
            facing = direction;
            Vector3 local = facingMarker.localPosition;
            local.x = markerLocalX * facing;
            facingMarker.localPosition = local;
        }

        private void ResetToStart()
        {
            rb.position = startPosition;
            transform.position = startPosition;
            rb.linearVelocity = Vector2.zero;
            canCutJump = false;
            SetFacing(1);
        }
    }
}
