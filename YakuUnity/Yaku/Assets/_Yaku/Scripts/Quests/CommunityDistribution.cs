using System;
using System.Linq;
using UnityEngine;
using Yaku.Dialogue;
using Yaku.Inventory;

namespace Yaku.Quests
{
    [RequireComponent(typeof(CommunityChapter))]
    public sealed class CommunityDistribution : MonoBehaviour
    {
        [Serializable]
        private sealed class Stop
        {
            [SerializeField] private string label;
            [SerializeField] private NpcDialogue recipient;
            [SerializeField] private SupplyPickup pickup;
            [SerializeField] private SupplyDelivery delivery;

            public SupplyPickup Pickup => pickup;
            public SupplyDelivery Delivery => delivery;
            public Transform Recipient => recipient.transform;
            public string Status => $"{label}: {(delivery.Delivered ? "entregado" : "pendiente")}";

            public void SetAvailable(bool available)
            {
                pickup.gameObject.SetActive(available && !delivery.Delivered);
                delivery.Available = false;
                if (available)
                    recipient.AlternateInteraction = delivery;
                else if (ReferenceEquals(recipient.AlternateInteraction, delivery))
                    recipient.AlternateInteraction = null;
            }
        }

        [SerializeField] private Stop[] stops;
        private bool available;

        public int DeliveredCount => stops.Count(stop => stop.Delivery.Delivered);
        public int TotalCount => stops.Length;
        public bool IsComplete => DeliveredCount == TotalCount;
        public Transform Destination
        {
            get
            {
                foreach (var stop in stops)
                    if (!stop.Delivery.Delivered && stop.Pickup.gameObject.activeSelf)
                        return stop.Pickup.transform;
                foreach (var stop in stops)
                    if (!stop.Delivery.Delivered)
                        return stop.Recipient;
                return null;
            }
        }
        public string Status => $"{DeliveredCount}/{TotalCount} · " + string.Join(" · ", stops.Select(stop => stop.Status));

        private void OnEnable()
        {
            foreach (var stop in stops)
            {
                stop.Pickup.Collected += Refresh;
                stop.Delivery.Completed += Refresh;
            }
        }

        private void OnDisable()
        {
            SetAvailable(false);
            foreach (var stop in stops)
            {
                stop.Pickup.Collected -= Refresh;
                stop.Delivery.Completed -= Refresh;
            }
        }

        public void SetAvailable(bool value)
        {
            if (available == value) return;
            available = value;
            foreach (var stop in stops)
                stop.SetAvailable(value);
        }

        private void Refresh(GameObject actor)
        {
            if (!available || actor != gameObject) return;
            foreach (var stop in stops)
                stop.Delivery.Available = !stop.Pickup.gameObject.activeSelf;

            GetComponent<CommunityChapter>().UpdateDistribution(this);
        }
    }
}
