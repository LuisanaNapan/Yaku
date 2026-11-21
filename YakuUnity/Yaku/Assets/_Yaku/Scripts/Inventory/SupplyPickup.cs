using System;
using UnityEngine;
using Yaku.Interaction;
using Yaku.UI;

namespace Yaku.Inventory
{
    public sealed class SupplyPickup : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemData item;
        [SerializeField, Min(1)] private int quantity = 1;

        public bool Available { get; set; } = true;
        public event Action<GameObject> Collected;
        public string Prompt => $"Recoger {item.DisplayName}";

        public void Interact(GameObject actor)
        {
            if (!isActiveAndEnabled) return;
            var hud = actor.GetComponent<CommunityHud>();
            if (!Available)
            {
                hud.Notify("Consulta el objetivo antes de recoger estos suministros.");
                return;
            }

            if (!actor.GetComponent<PlayerInventory>().TryAdd(item, quantity))
            {
                hud.Notify("No hay espacio para estos suministros.");
                return;
            }

            gameObject.SetActive(false);
            hud.Notify($"Recogiste {item.DisplayName} ×{quantity}.", GameAudio.Cue.Collect);
            Collected?.Invoke(actor);
        }
    }
}
