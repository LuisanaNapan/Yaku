using UnityEngine;
using UnityEngine.InputSystem;
using Yaku.UI;

namespace Yaku.Interaction
{
    public sealed class InteractionDetector : MonoBehaviour
    {
        [SerializeField] private InputActionReference interactInput;
        [SerializeField, Min(0.1f)] private float range = 2f;

        private readonly Collider[] nearby = new Collider[32];
        private InputAction action;
        private IInteractable target;
        private System.Action modalInteraction;
        private GameAudio audioController;

        public string Prompt => modalInteraction == null ? target?.Prompt : null;
        public bool HasModalInteraction => modalInteraction != null;

        public void SetModalInteraction(System.Action action) => modalInteraction = action;

        private void Awake()
        {
            action = interactInput.action.Clone();
            audioController = GetComponent<GameAudio>();
        }
        private void OnEnable() => action?.Enable();

        private void OnDisable()
        {
            action?.Disable();
            target = null;
        }

        private void OnDestroy() => action?.Dispose();

        private void Update()
        {
            if (Time.timeScale <= 0f) return;

            if (modalInteraction != null)
            {
                if (action.WasPressedThisFrame())
                    modalInteraction();
                return;
            }

            target = FindTarget();
            if (target != null && action.WasPressedThisFrame())
            {
                audioController?.Queue(GameAudio.Cue.Interact);
                target.Interact(gameObject);
            }
        }

        private IInteractable FindTarget()
        {
            Vector3 center = transform.position + Vector3.up * 0.9f;
            int count = Physics.OverlapSphereNonAlloc(center, range, nearby, ~0, QueryTriggerInteraction.Collide);
            Collider[] hits = nearby;
            if (count == nearby.Length)
            {
                hits = Physics.OverlapSphere(center, range, ~0, QueryTriggerInteraction.Collide);
                count = hits.Length;
            }

            IInteractable best = null;
            float nearest = range * range;
            for (int i = 0; i < count; i++)
            {
                Collider hit = hits[i];
                if (!hit.isTrigger || !(hit.GetComponentInParent<IInteractable>() is Behaviour candidate) ||
                    !candidate.isActiveAndEnabled)
                    continue;

                float distance = (hit.ClosestPoint(center) - center).sqrMagnitude;
                if (distance >= nearest)
                    continue;

                nearest = distance;
                best = (IInteractable)candidate;
            }

            return best;
        }
    }
}
