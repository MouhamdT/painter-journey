using System.Collections.Generic;
using PainterJourney.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PainterJourney.Player
{
    /// <summary>
    /// Swings the brush on a press of Attack (J). A swing lasts attackDuration seconds and has
    /// a box of attackWidth × attackHeight in front of the player, on the side it faced at the
    /// press. Every Health inside the box takes attackDamage, at most once per swing, and never
    /// the player's own. The blue stroke shows exactly the box while the swing runs.
    /// A press during a running swing, or while the player is dead, is ignored.
    /// All numbers come from PlayerConfig.
    /// </summary>
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Health))]
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private PlayerConfig config;
        [SerializeField] private InputActionReference attackAction;
        [Tooltip("Child sprite that shows the hit box during a swing. It is hidden the rest of the time.")]
        [SerializeField] private SpriteRenderer stroke;

        private readonly List<Collider2D> overlaps = new List<Collider2D>();
        private readonly HashSet<Health> hitThisSwing = new HashSet<Health>();

        private PlayerController controller;
        private BoxCollider2D body;
        private Health ownHealth;
        private ContactFilter2D targetFilter;
        private float swingTimeLeft;
        private int swingFacing = 1;

        private bool IsSwinging => swingTimeLeft > 0f;

        private void Awake()
        {
            controller = GetComponent<PlayerController>();
            body = GetComponent<BoxCollider2D>();
            ownHealth = GetComponent<Health>();

            // Every layer, and triggers too: the test dummy (and later enemies) can be triggers.
            targetFilter = new ContactFilter2D { useTriggers = true };

            stroke.enabled = false;
        }

        private void OnEnable()
        {
            // The project-wide actions asset is shared, so the action is enabled here but not disabled.
            attackAction.action.Enable();
        }

        private void FixedUpdate()
        {
            // The box moves with the player, and a target that walks into it is hit too.
            if (IsSwinging)
            {
                HitTargetsInBox();
            }
        }

        private void Update()
        {
            if (IsSwinging)
            {
                swingTimeLeft -= Time.deltaTime;
                if (swingTimeLeft <= 0f)
                {
                    EndSwing();
                }
            }

            // One swing per press, never while a swing runs, never during the death pause.
            if (attackAction.action.WasPressedThisFrame() && !IsSwinging && !ownHealth.IsDead)
            {
                StartSwing();
            }
        }

        private void StartSwing()
        {
            // The side is locked now. Turning during the swing doesn't move the box.
            swingFacing = controller.Facing;
            swingTimeLeft = config.attackDuration;
            hitThisSwing.Clear();

            ShowStroke();

            // A target that is already inside the box is hit on the first frame of the swing.
            HitTargetsInBox();
        }

        private void EndSwing()
        {
            swingTimeLeft = 0f;
            stroke.enabled = false;
            hitThisSwing.Clear();
        }

        private void HitTargetsInBox()
        {
            Vector2 size = BoxSize();
            Physics2D.OverlapBox(BoxCenter(), size, 0f, targetFilter, overlaps);

            for (int i = 0; i < overlaps.Count; i++)
            {
                if (!overlaps[i].TryGetComponent(out Health target))
                {
                    continue;
                }

                // The box's inner edge touches the player's own collider, so its Health can show up here.
                if (target == ownHealth || target.IsDead || !hitThisSwing.Add(target))
                {
                    continue;
                }

                target.TakeDamage(config.attackDamage);
            }
        }

        private void ShowStroke()
        {
            // The stroke is a child of the player, so it rides along with the drawn player.
            // It is placed in the player's local space (not at the physics box), because the
            // drawn player is smoothed between physics steps. Its world size is the box,
            // which means dividing by the player's own scale.
            Vector3 parentScale = transform.lossyScale;
            Vector2 size = BoxSize();
            stroke.transform.localScale = new Vector3(size.x / parentScale.x, size.y / parentScale.y, 1f);

            float localOffset = body.size.x * 0.5f + size.x * 0.5f / parentScale.x;
            Vector3 local = stroke.transform.localPosition;
            local.x = body.offset.x + swingFacing * localOffset;
            local.y = body.offset.y;
            stroke.transform.localPosition = local;
            stroke.enabled = true;
        }

        private Vector2 BoxSize()
        {
            return new Vector2(config.attackWidth, config.attackHeight);
        }

        private Vector2 BoxCenter()
        {
            // The inner edge of the box touches the player's front edge, and the box is
            // centred on the player's centre in height.
            Bounds bounds = body.bounds;
            float offset = bounds.extents.x + config.attackWidth * 0.5f;
            return new Vector2(bounds.center.x + swingFacing * offset, bounds.center.y);
        }
    }
}
