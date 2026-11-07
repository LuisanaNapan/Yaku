using UnityEngine;
using UnityEngine.InputSystem;
using Yaku.Player;

namespace Yaku.Cameras
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public sealed class ThirdPersonCamera : MonoBehaviour
    {
        [SerializeField] private PlayerRig player;
        [SerializeField] private InputActionReference lookInput;

        [Header("Framing")]
        [SerializeField, Min(0.1f)] private float distance = 4.25f;
        [SerializeField] private float heightOffset = 0.12f;
        [SerializeField, Range(-80f, 80f)] private float initialPitch = 16f;
        [SerializeField, Range(-80f, 80f)] private float minPitch = -12f;
        [SerializeField, Range(-80f, 80f)] private float maxPitch = 65f;

        [Header("Response")]
        [SerializeField, Min(0f)] private float sensitivity = 0.19f;
        [SerializeField, Min(0f)] private float lookSmoothTime = 0.045f;
        [SerializeField, Min(0f)] private float followSmoothTime = 0.10f;

        private InputAction lookAction;
        private Transform target;
        private Vector3 pivot;
        private Vector3 followVelocity;
        private float desiredYaw;
        private float desiredPitch;
        private float yaw;
        private float pitch;
        private float yawVelocity;
        private float pitchVelocity;

        private Vector3 TargetCenter => target.position + Vector3.up * heightOffset;

        private void Awake()
        {
            target = player != null ? player.CameraTarget : null;
            if (target == null || lookInput == null || lookInput.action == null)
            {
                Debug.LogError("ThirdPersonCamera requires a PlayerRig with a camera target and a Look input action.", this);
                enabled = false;
                return;
            }

            lookAction = lookInput.action.Clone();
            lookAction.bindingMask = InputBinding.MaskByGroup("Keyboard&Mouse");
        }

        private void OnEnable()
        {
            if (lookAction == null)
                return;

            lookAction.Enable();
            pivot = TargetCenter;
            followVelocity = Vector3.zero;
            yawVelocity = pitchVelocity = 0f;
            desiredYaw = yaw = player.transform.eulerAngles.y;
            desiredPitch = pitch = ClampPitch(initialPitch);
            ApplyPose();
            SetCursorCaptured(Application.isFocused);
        }

        private void OnDisable()
        {
            lookAction?.Disable();
            SetCursorCaptured(false);
        }

        private void OnDestroy() => lookAction?.Dispose();

        private void OnApplicationFocus(bool focused)
        {
            if (!focused && isActiveAndEnabled)
                SetCursorCaptured(false);
        }

        private void LateUpdate()
        {
            bool canLook = UpdateCursorCapture();
            float deltaTime = Time.deltaTime;
            if (deltaTime <= 0f || target == null)
                return;

            if (canLook && lookAction.activeControl?.device is Mouse)
            {
                // Pointer delta is already accumulated for the frame.
                Vector2 delta = lookAction.ReadValue<Vector2>() * sensitivity;
                desiredYaw += delta.x;
                desiredPitch = ClampPitch(desiredPitch - delta.y);
            }

            yaw = lookSmoothTime > 0f
                ? Mathf.SmoothDamp(yaw, desiredYaw, ref yawVelocity, lookSmoothTime, Mathf.Infinity, deltaTime)
                : desiredYaw;
            pitch = lookSmoothTime > 0f
                ? Mathf.SmoothDamp(pitch, desiredPitch, ref pitchVelocity, lookSmoothTime, Mathf.Infinity, deltaTime)
                : desiredPitch;
            pivot = followSmoothTime > 0f
                ? Vector3.SmoothDamp(pivot, TargetCenter, ref followVelocity, followSmoothTime, Mathf.Infinity, deltaTime)
                : TargetCenter;

            float completedTurns = 360f * Mathf.Floor(yaw / 360f);
            yaw -= completedTurns;
            desiredYaw -= completedTurns;
            ApplyPose();
        }

        private float ClampPitch(float angle) => Mathf.Clamp(angle, Mathf.Min(minPitch, maxPitch), Mathf.Max(minPitch, maxPitch));

        private void ApplyPose()
        {
            Quaternion orbit = Quaternion.Euler(pitch, yaw, 0f);
            transform.SetPositionAndRotation(pivot - orbit * Vector3.forward * distance, orbit);
        }

        private static bool UpdateCursorCapture()
        {
            if (!Application.isFocused)
                return false;

            if (Keyboard.current?.escapeKey.wasPressedThisFrame == true)
            {
                SetCursorCaptured(false);
                return false;
            }

            if (Cursor.lockState == CursorLockMode.Locked)
                return true;

            if (Mouse.current?.leftButton.wasPressedThisFrame == true)
                SetCursorCaptured(true);

            return false;
        }

        private static void SetCursorCaptured(bool captured)
        {
            Cursor.lockState = captured ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !captured;
        }
    }
}
