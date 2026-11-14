using System;
using UnityEngine;

namespace Yaku.Inventory
{
    public sealed class PlayerInventory : MonoBehaviour
    {
        public const int Capacity = 6;

        public readonly struct Slot
        {
            public ItemData Item { get; }
            public int Quantity { get; }

            public Slot(ItemData item, int quantity)
            {
                Item = item;
                Quantity = quantity;
            }
        }

        private readonly Slot[] slots = new Slot[Capacity];

        public event Action Changed;
        public Slot GetSlot(int index) => slots[index];

        public long Count(ItemData item)
        {
            if (item == null) return 0;

            long total = 0;
            foreach (var slot in slots)
                if (slot.Item == item)
                    total += slot.Quantity;
            return total;
        }

        public bool TryAdd(ItemData item, int quantity)
        {
            if (item == null || quantity <= 0) return false;

            int limit = item.StackLimit;
            long space = 0;
            foreach (var slot in slots)
            {
                if (slot.Item == null)
                    space += limit;
                else if (slot.Item == item)
                    space += Mathf.Max(0, limit - slot.Quantity);
            }
            if (space < quantity) return false;

            // Fill existing stacks first so free slots remain available for other items.
            for (int i = 0; i < slots.Length && quantity > 0; i++)
            {
                if (slots[i].Item != item) continue;
                int added = Mathf.Min(quantity, Mathf.Max(0, limit - slots[i].Quantity));
                slots[i] = new Slot(item, slots[i].Quantity + added);
                quantity -= added;
            }
            for (int i = 0; i < slots.Length && quantity > 0; i++)
            {
                if (slots[i].Item != null) continue;
                int added = Mathf.Min(quantity, limit);
                slots[i] = new Slot(item, added);
                quantity -= added;
            }
            Changed?.Invoke();
            return true;
        }

        public bool TryRemove(ItemData item, int quantity)
        {
            if (item == null || quantity <= 0 || Count(item) < quantity) return false;

            for (int i = slots.Length - 1; i >= 0 && quantity > 0; i--)
            {
                if (slots[i].Item != item) continue;
                int removed = Mathf.Min(quantity, slots[i].Quantity);
                int left = slots[i].Quantity - removed;
                slots[i] = left == 0 ? default : new Slot(item, left);
                quantity -= removed;
            }
            Changed?.Invoke();
            return true;
        }
    }
}
