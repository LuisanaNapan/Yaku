using UnityEngine;
using UnityEngine.InputSystem;

namespace Yaku.Player
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerMovement : MonoBehaviour
    {
        private const float GroundContactSpeed = -2f;
        private const float JumpBufferDuration = 0.10f;
        private const float ReleaseGravityMultiplier = 1.65f;
        private const float FallGravityMultiplier = 1.25f;

        [SerializeField] private InputActionReference moveInput;
        [SerializeField] private InputActionReference sprintInput;
        [SerializeField] private InputActionReference jumpInput;
        [SerializeField, Min(0f)] private float walkSpeed = 2.05f;
        [SerializeField, Min(0f)] private float runSpeed = 4.5f;
        [SerializeField, Min(0f)] private float turnSpeed = 540f;
        [SerializeField, Min(0.01f)] private float jumpHeight = 1.05f;
        [SerializeField, Min(0.01f)] private float gravity = 18f;

        private CharacterController controller;
        private InputAction moveAction;
        private InputAction sprintAction;
        private InputAction jumpAction;
        private float verticalSpeed;
        private float jumpBuffer;
        private Vector3 planarVelocity;

        public float VerticalSpeed => verticalSpeed;
        public float JumpPhase => Mathf.Clamp01(1f - verticalSpeed / Mathf.Sqrt(2f * gravity * jumpHeight));

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            if (moveInput == null || moveInput.action == null ||
                sprintInput == null || sprintInput.action == null ||
                jumpInput == null || jumpInput.action == null)
            {
                Debug.LogError("PlayerMovement requires Move, Sprint and Jump input actions.", this);
                enabled = false;
                return;
            }

            // Own the action instances so disabling this component does not affect the shared asset.
            moveAction = moveInput.action.Clone();
            sprintAction = sprintInput.action.Clone();
            jumpAction = jumpInput.action.Clone();
        }

        private void OnEnable()
        {
            moveAction?.Enable();
            sprintAction?.Enable();
            jumpAction?.Enable();
        }

        private void OnDisable()
        {
            moveAction?.Disable();
            sprintAction?.Disable();
            jumpAction?.Disable();
            verticalSpeed = 0f;
            jumpBuffer = 0f;
            planarVelocity = Vector3.zero;
        }

        private void OnDestroy()
        {
            moveAction?.Dispose();
            sprintAction?.Dispose();
            jumpAction?.Dispose();
        }

        private void Update()
        {
            if (!controller.enabled)
                return;

            float deltaTime = Time.deltaTime;
            if (deltaTime <= 0f)
                return;

            SimulateMovement(ReadMovementDirection(), sprintAction.IsPressed(), jumpAction.WasPressedThisFrame(),
                jumpAction.IsPressed(), deltaTime);
        }

        private void SimulateMovement(Vector3 movement, bool sprinting, bool jumpPressed, bool jumpHeld, float deltaTime)
        {
            jumpBuffer = jumpPressed ? JumpBufferDuration : Mathf.Max(0f, jumpBuffer - deltaTime);
            if (controller.isGrounded && verticalSpeed <= 0f)
            {
                if (jumpBuffer > 0f)
                {
                    verticalSpeed = Mathf.Sqrt(2f * gravity * jumpHeight);
                    jumpBuffer = 0f;
                }
                else
                {
                    verticalSpeed = GroundContactSpeed;
                }
            }

            float acceleration = gravity;
            if (verticalSpeed < 0f && !controller.isGrounded)
                acceleration *= FallGravityMultiplier;
            else if (verticalSpeed > 0f && !jumpHeld)
                acceleration *= ReleaseGravityMultiplier;

            float speed = sprinting ? runSpeed : walkSpeed;
            float verticalDistance = (verticalSpeed - 0.5f * acceleration * deltaTime) * deltaTime;
            verticalSpeed -= acceleration * deltaTime;

            Vector3 desiredVelocity = movement * speed;
            float response = movement.sqrMagnitude > 0f ? 16f : 23f;
            planarVelocity = Vector3.MoveTowards(planarVelocity, desiredVelocity, response * deltaTime);
            Vector3 displacement = planarVelocity * deltaTime + Vector3.up * verticalDistance;
            CollisionFlags contacts = controller.Move(displacement);
            if ((contacts & CollisionFlags.Above) != 0 && verticalSpeed > 0f)
                verticalSpeed = 0f;
            else if ((contacts & CollisionFlags.Below) != 0 && verticalSpeed < 0f)
                verticalSpeed = GroundContactSpeed;

            if (movement.sqrMagnitude > 0f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * deltaTime);
            }
        }

        private Vector3 ReadMovementDirection()
        {
            Vector2 input = Vector2.ClampMagnitude(moveAction.ReadValue<Vector2>(), 1f);
            if (input.sqrMagnitude < 0.0001f)
                return Vector3.zero;

            Camera view = Camera.main;
            if (view == null)
                return Vector3.zero;

            Vector3 forward = Vector3.ProjectOnPlane(view.transform.forward, Vector3.up);
            if (forward.sqrMagnitude < 0.0001f)
                forward = Vector3.Cross(view.transform.right, Vector3.up);

            forward.Normalize();
            Vector3 right = Vector3.Cross(Vector3.up, forward);
            return forward * input.y + right * input.x;
        }
    }
}
