using UnityEngine;
using UnityEngine.InputSystem;

namespace Yaku.Player
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private InputActionReference moveInput;
        [SerializeField, Min(0f)] private float walkSpeed = 2.75f;
        [SerializeField, Min(0f)] private float turnSpeed = 540f;

        private CharacterController controller;
        private InputAction moveAction;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            if (moveInput == null || moveInput.action == null)
            {
                Debug.LogError("PlayerMovement requires a Move input action.", this);
                enabled = false;
                return;
            }

            // Keep the movement action's lifetime independent of other consumers of the asset.
            moveAction = moveInput.action.Clone();
        }

        private void OnEnable() => moveAction?.Enable();

        private void OnDisable() => moveAction?.Disable();

        private void OnDestroy() => moveAction?.Dispose();

        private void Update()
        {
            Vector2 input = Vector2.ClampMagnitude(moveAction.ReadValue<Vector2>(), 1f);
            if (input.sqrMagnitude < 0.0001f || !controller.enabled)
                return;

            Camera view = Camera.main;
            if (view == null)
                return;

            Vector3 forward = Vector3.ProjectOnPlane(view.transform.forward, Vector3.up);
            if (forward.sqrMagnitude < 0.0001f)
                forward = Vector3.Cross(view.transform.right, Vector3.up);

            forward.Normalize();
            Vector3 right = Vector3.Cross(Vector3.up, forward);
            Vector3 movement = forward * input.y + right * input.x;
            float deltaTime = Time.deltaTime;

            controller.Move(movement * (walkSpeed * deltaTime));
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * deltaTime);
        }
    }
}
