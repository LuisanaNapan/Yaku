using UnityEngine;

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

        private void Awake()
        {
            animator = GetComponent<Animator>();
            controller = GetComponentInParent<CharacterController>();
            if (controller == null)
                enabled = false;
        }

        private void Update()
        {
            Vector3 velocity = controller.velocity;
            float planarSpeed = new Vector2(velocity.x, velocity.z).magnitude;
            animator.SetFloat(GroundSpeed, planarSpeed, 0.085f, Time.deltaTime);
            animator.SetFloat(VerticalSpeed, velocity.y);
            animator.SetBool(Grounded, controller.isGrounded);
        }
    }
}
