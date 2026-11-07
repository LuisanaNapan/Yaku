using UnityEngine;
using Yaku.Interaction;

namespace Yaku.Characters
{
    [DisallowMultipleComponent]
    public sealed class NpcRig : MonoBehaviour
    {
        [SerializeField] private Transform visualRoot;
        [SerializeField] private InteractionAnchor interactionAnchor;

        public Transform VisualRoot => visualRoot;
        public InteractionAnchor InteractionAnchor => interactionAnchor;
    }
}
