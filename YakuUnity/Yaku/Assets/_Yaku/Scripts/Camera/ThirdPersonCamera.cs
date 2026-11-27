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

        [Header("Obstacles")]
        [SerializeField] private LayerMask obstacleLayers = ~0;
        [SerializeField, Min(0.01f)] private float collisionRadius = 0.24f;
        [SerializeField, Min(0f)] private float obstacleClearance = 0.12f;
        [SerializeField, Min(0f)] private float retractSmoothTime = 0.06f;
        [SerializeField, Min(0f)] private float recoverySmoothTime = 0.28f;

        private readonly RaycastHit[] obstacleHits = new RaycastHit[16];
        private Camera view;
        private float orbitDistance;
        private float distanceVelocity;
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
            view = GetComponent<Camera>();
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
            orbitDistance = distance;
            distanceVelocity = 0f;
            ApplyPose(0f);
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
            ApplyPose(deltaTime);
        }

        private float ClampPitch(float angle) => Mathf.Clamp(angle, Mathf.Min(minPitch, maxPitch), Mathf.Max(minPitch, maxPitch));

        private void ApplyPose(float deltaTime)
        {
            Quaternion orbit = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 outward = orbit * Vector3.back;
            float nearHalfHeight = view.nearClipPlane * Mathf.Tan(view.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float nearRadius = new Vector3(nearHalfHeight * view.aspect, nearHalfHeight, view.nearClipPlane).magnitude;
            float safeDistance = FindClearDistance(outward, Mathf.Max(collisionRadius, nearRadius));
            float desiredDistance = safeDistance < distance ? Mathf.Max(0f, safeDistance - obstacleClearance) : distance;
            float smoothTime = desiredDistance < orbitDistance ? retractSmoothTime : recoverySmoothTime;
            float nextDistance = deltaTime > 0f && smoothTime > 0f
                ? Mathf.SmoothDamp(orbitDistance, desiredDistance, ref distanceVelocity, smoothTime, Mathf.Infinity, deltaTime)
                : desiredDistance;

            // A newly intersecting obstacle takes precedence over damping.
            orbitDistance = Mathf.Min(nextDistance, safeDistance);
            if (nextDistance >= safeDistance)
                distanceVelocity = 0f;
            transform.SetPositionAndRotation(pivot + outward * orbitDistance, orbit);
        }

        private float FindClearDistance(Vector3 outward, float radius)
        {
            int count = Physics.SphereCastNonAlloc(pivot, radius, outward, obstacleHits, distance,
                obstacleLayers, QueryTriggerInteraction.Ignore);
            RaycastHit[] hits = obstacleHits;
            if (count == obstacleHits.Length)
            {
                hits = Physics.SphereCastAll(pivot, radius, outward, distance,
                    obstacleLayers, QueryTriggerInteraction.Ignore);
                count = hits.Length;
            }

            float clearDistance = distance;
            for (int i = 0; i < count; i++)
            {
                if (!hits[i].collider.transform.IsChildOf(player.transform))
                    clearDistance = Mathf.Min(clearDistance, hits[i].distance);
            }

            return clearDistance;
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
