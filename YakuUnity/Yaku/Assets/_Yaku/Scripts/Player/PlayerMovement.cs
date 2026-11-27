using UnityEngine;
using UnityEngine.InputSystem;

namespace Yaku.Player
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerMovement : MonoBehaviour
    {
        private const float GroundContactSpeed = -2f;

        [SerializeField] private InputActionReference moveInput;
        [SerializeField] private InputActionReference sprintInput;
        [SerializeField] private InputActionReference jumpInput;
        [SerializeField, Min(0f)] private float walkSpeed = 2.75f;
        [SerializeField, Min(0f)] private float runSpeed = 4.5f;
        [SerializeField, Min(0f)] private float turnSpeed = 540f;
        [SerializeField, Min(0.01f)] private float jumpHeight = 1.05f;
        [SerializeField, Min(0.01f)] private float gravity = 18f;

        private CharacterController controller;
        private InputAction moveAction;
        private InputAction sprintAction;
        private InputAction jumpAction;
        private float verticalSpeed;

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

            Vector3 movement = ReadMovementDirection();
            if (controller.isGrounded && verticalSpeed <= 0f)
            {
                verticalSpeed = jumpAction.WasPressedThisFrame()
                    ? Mathf.Sqrt(2f * gravity * jumpHeight)
                    : GroundContactSpeed;
            }

            float speed = sprintAction.IsPressed() ? runSpeed : walkSpeed;
            float verticalDistance = (verticalSpeed - 0.5f * gravity * deltaTime) * deltaTime;
            verticalSpeed -= gravity * deltaTime;

            Vector3 displacement = movement * (speed * deltaTime) + Vector3.up * verticalDistance;
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
