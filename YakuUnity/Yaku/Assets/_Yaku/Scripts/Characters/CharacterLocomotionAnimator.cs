using UnityEngine;
using Yaku.Player;

namespace Yaku.Characters
{
    [DefaultExecutionOrder(100)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public sealed class CharacterLocomotionAnimator : MonoBehaviour
    {
        private static readonly int GroundSpeed = Animator.StringToHash("GroundSpeed");
        private static readonly int VerticalSpeed = Animator.StringToHash("VerticalSpeed");
        private static readonly int Grounded = Animator.StringToHash("Grounded");

        private CharacterController controller;
        private Animator animator;
        private PlayerMovement movement;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            controller = GetComponentInParent<CharacterController>();
            movement = GetComponentInParent<PlayerMovement>();
            if (controller == null)
                enabled = false;
        }

        private void Update()
        {
            Vector3 velocity = controller.velocity;
            float planarSpeed = new Vector2(velocity.x, velocity.z).magnitude;
            animator.SetFloat(GroundSpeed, planarSpeed, 0.055f, Time.deltaTime);
            bool grounded = controller.isGrounded;
            animator.SetFloat(VerticalSpeed, grounded ? 0f : movement != null ? movement.VerticalSpeed : velocity.y);
            animator.SetBool(Grounded, grounded);
        }
    }
}
