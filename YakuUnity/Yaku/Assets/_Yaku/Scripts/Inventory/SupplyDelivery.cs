using System;
using UnityEngine;
using Yaku.Interaction;
using Yaku.UI;

namespace Yaku.Inventory
{
    public sealed class SupplyDelivery : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemData requiredItem;
        [SerializeField, Min(1)] private int requiredQuantity = 2;
        [SerializeField] private string recipient;

        public bool Available { get; set; } = true;
        public bool Delivered { get; private set; }
        public event Action<GameObject> Completed;
        public string Prompt => Delivered ? "Revisar entrega" : $"Entregar a {recipient}";

        public void Interact(GameObject actor)
        {
            var hud = actor.GetComponent<CommunityHud>();
            if (Delivered)
            {
                hud.Notify("La entrega ya se realizó.");
                return;
            }
            if (!Available)
            {
                hud.Notify("Todavía no corresponde entregar estos suministros.");
                return;
            }
            if (!actor.GetComponent<PlayerInventory>().TryRemove(requiredItem, requiredQuantity))
            {
                hud.Notify($"Faltan recursos. Se necesitan {requiredItem.DisplayName} ×{requiredQuantity}.");
                return;
            }

            Delivered = true;
            hud.Notify($"{recipient} recibió los suministros.");
            Completed?.Invoke(actor);
        }
    }
}
