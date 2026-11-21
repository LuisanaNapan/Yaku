using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Yaku.Interaction;
using Yaku.UI;

namespace Yaku.Inventory
{
    [RequireComponent(typeof(PlayerInventory), typeof(CommunityHud))]
    public sealed class InventoryHud : MonoBehaviour
    {
        private readonly Text[] labels = new Text[PlayerInventory.Capacity];
        private readonly Image[] icons = new Image[PlayerInventory.Capacity];
        private PlayerInventory inventory;
        private GameObject panel;
        private InteractionDetector detector;
        private bool inventoryOpen;

        private void Start()
        {
            inventory = GetComponent<PlayerInventory>();
            detector = GetComponent<InteractionDetector>();
            var hud = GetComponent<CommunityHud>();
            var heading = hud.CreatePanel("Inventory", Vector2.one, new Vector2(-179, -211),
                new Vector2(318, 338), 22);
            panel = heading.transform.parent.gameObject;
            heading.alignment = TextAnchor.UpperCenter;
            heading.text = "SUMINISTROS  ·  [I]";
            heading.color = CommunityHud.AccentColor;
            heading.rectTransform.offsetMax = new Vector2(-16, -12);

            for (int i = 0; i < labels.Length; i++)
            {
                var row = new GameObject($"Slot {i + 1}", typeof(RectTransform), typeof(Image));
                row.transform.SetParent(panel.transform, false);
                var rect = row.GetComponent<RectTransform>();
                rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 1);
                rect.anchoredPosition = new Vector2(0, -63 - i * 45);
                rect.sizeDelta = new Vector2(284, 40);
                var background = row.GetComponent<Image>();
                background.color = new Color(0.20f, 0.29f, 0.22f);
                background.raycastTarget = false;

                icons[i] = new GameObject("Icon", typeof(RectTransform), typeof(Image)).GetComponent<Image>();
                icons[i].transform.SetParent(row.transform, false);
                icons[i].rectTransform.anchoredPosition = new Vector2(-119, 0);
                icons[i].rectTransform.sizeDelta = new Vector2(27, 27);
                icons[i].preserveAspect = true;
                icons[i].raycastTarget = false;

                labels[i] = new GameObject("Label", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
                labels[i].transform.SetParent(row.transform, false);
                labels[i].rectTransform.anchoredPosition = new Vector2(21, 0);
                labels[i].rectTransform.sizeDelta = new Vector2(230, 34);
                labels[i].font = heading.font;
                labels[i].fontSize = 18;
                labels[i].color = heading.color;
                labels[i].alignment = TextAnchor.MiddleLeft;
                labels[i].raycastTarget = false;
            }

            Render();
            panel.SetActive(false);
        }

        private void Update()
        {
            if (Time.timeScale <= 0f) return;
            if (Keyboard.current?.iKey.wasPressedThisFrame == true)
                inventoryOpen = !inventoryOpen;
            bool visible = inventoryOpen && !detector.HasModalInteraction;
            if (panel.activeSelf != visible)
                panel.SetActive(visible);
            if (panel.activeSelf)
                Render();
        }

        private void Render()
        {
            for (int i = 0; i < labels.Length; i++)
            {
                var slot = inventory.GetSlot(i);
                labels[i].text = slot.Item == null ? "Vacío" : $"{slot.Item.DisplayName}   ×{slot.Quantity}";
                icons[i].sprite = slot.Item == null ? null : slot.Item.Icon;
                icons[i].enabled = icons[i].sprite != null;
            }
        }
    }
}
