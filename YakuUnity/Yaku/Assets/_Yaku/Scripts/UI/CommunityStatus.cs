using UnityEngine;
using UnityEngine.UI;

namespace Yaku.UI
{
    [RequireComponent(typeof(CommunityHud))]
    public sealed class CommunityStatus : MonoBehaviour
    {
        [SerializeField, Range(0, 100)] private int health = 100;
        [SerializeField, Range(0, 100)] private int hydration = 100;
        [SerializeField] private Transform riverMarker;
        [SerializeField, Min(0)] private float dangerRadius = 3f;

        private readonly Image[] bars = new Image[2];
        private readonly Text[] values = new Text[2];
        private Text alert;
        private int displayedHealth = -1;
        private int displayedHydration = -1;
        private bool wasNearRiver;

        public bool NearRiver
        {
            get
            {
                if (riverMarker == null) return false;
                var offset = transform.position - riverMarker.position;
                offset.y = 0f;
                return offset.sqrMagnitude <= dangerRadius * dangerRadius;
            }
        }

        private void Start()
        {
            var title = GetComponent<CommunityHud>().CreatePanel("Community Status", new Vector2(0, 1),
                new Vector2(192, -50), new Vector2(346, 72), 16);
            title.enabled = false;
            var parent = title.transform.parent;
            CreateGauge(parent, 0, "Salud", new Color(0.55f, 0.80f, 0.38f));
            CreateGauge(parent, 1, "Hidratación", new Color(0.35f, 0.75f, 0.89f));
            alert = Label(parent, "Alerta", new Vector2(0, -23), new Vector2(312, 21));
            alert.alignment = TextAnchor.MiddleLeft;
            RenderAlert(NearRiver);
        }

        private void LateUpdate()
        {
            if (displayedHealth != health)
            {
                displayedHealth = health;
                SetGauge(0, health);
            }
            if (displayedHydration != hydration)
            {
                displayedHydration = hydration;
                SetGauge(1, hydration);
            }

            bool nearRiver = NearRiver;
            if (nearRiver != wasNearRiver) RenderAlert(nearRiver);
        }

        private void RenderAlert(bool nearRiver)
        {
            wasNearRiver = nearRiver;
            alert.text = nearRiver ? "Peligro · Ribera en observación" : "Peligro · Sin alerta local";
            alert.color = nearRiver ? new Color(1f, 0.74f, 0.36f) : new Color(0.78f, 0.87f, 0.73f);
        }

        private void CreateGauge(Transform parent, int index, string caption, Color color)
        {
            float x = index == 0 ? -80f : 79f;
            var label = Label(parent, caption, new Vector2(x, 19), new Vector2(145, 20));
            label.alignment = TextAnchor.MiddleLeft;
            label.text = caption;

            values[index] = Label(parent, caption + " Valor", new Vector2(x, 19), new Vector2(145, 20));
            values[index].alignment = TextAnchor.MiddleRight;

            var track = Strip(parent, caption + " Fondo", new Vector2(x, 2), new Vector2(145, 7),
                new Color(0.24f, 0.34f, 0.29f));
            bars[index] = Strip(track.transform, caption + " Barra", Vector2.zero, new Vector2(145, 7), color);
            var rect = bars[index].rectTransform;
            rect.anchorMin = rect.anchorMax = new Vector2(0, 0.5f);
            rect.pivot = new Vector2(0, 0.5f);
        }

        private void SetGauge(int index, int value)
        {
            bars[index].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 145f * value * 0.01f);
            values[index].text = $"{value}%";
        }

        private static Text Label(Transform parent, string name, Vector2 position, Vector2 size)
        {
            var text = new GameObject(name, typeof(RectTransform), typeof(Text)).GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 15;
            text.color = CommunityHud.TextColor;
            text.raycastTarget = false;
            Place(text.rectTransform, parent, position, size);
            return text;
        }

        private static Image Strip(Transform parent, string name, Vector2 position, Vector2 size, Color color)
        {
            var image = new GameObject(name, typeof(RectTransform), typeof(Image)).GetComponent<Image>();
            image.color = color;
            image.raycastTarget = false;
            Place(image.rectTransform, parent, position, size);
            return image;
        }

        private static void Place(RectTransform rect, Transform parent, Vector2 position, Vector2 size)
        {
            rect.SetParent(parent, false);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
        }
    }
}
