using UnityEngine;

namespace Yaku.Inventory
{
    public sealed class ItemData : ScriptableObject
    {
        [SerializeField] private string displayName;
        [SerializeField] private Sprite icon;
        [SerializeField, Min(1)] private int stackLimit = 4;

        public string DisplayName => displayName;
        public Sprite Icon => icon;
        public int StackLimit => Mathf.Max(1, stackLimit);
    }
}
