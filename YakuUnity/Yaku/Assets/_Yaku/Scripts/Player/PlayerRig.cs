using UnityEngine;

namespace Yaku.Player
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerRig : MonoBehaviour
    {
        [SerializeField] private Transform visualRoot;
        [SerializeField] private Transform cameraTarget;

        public Transform VisualRoot => visualRoot;
        public Transform CameraTarget => cameraTarget;
    }
}
