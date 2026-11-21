using UnityEngine;
using UnityEngine.UI;
using Yaku.Interaction;

namespace Yaku.UI
{
    [RequireComponent(typeof(InteractionDetector))]
    public sealed class CommunityHud : MonoBehaviour
    {
        private static readonly Color PanelColor = new Color(0.09f, 0.16f, 0.13f, 0.94f);
        private InteractionDetector detector;
        private Canvas canvas;
        private Text prompt;
        private Text notification;
        private float notificationUntil;

        private void Awake()
        {
            detector = GetComponent<InteractionDetector>();
            var overlay = new GameObject("Community HUD", typeof(Canvas), typeof(CanvasScaler));
            overlay.transform.SetParent(transform, false);
            canvas = overlay.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = overlay.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280, 720);
            prompt = CreatePanel("Interaction", new Vector2(0.5f, 0f), new Vector2(0, 48), new Vector2(340, 48), 23);
            notification = CreatePanel("Message", new Vector2(0.5f, 1f), new Vector2(0, -54), new Vector2(520, 54), 20);
        }

        private void LateUpdate()
        {
            string hint = detector.Prompt;
            prompt.transform.parent.gameObject.SetActive(hint != null);
            prompt.text = hint == null ? string.Empty : $"[E] {hint}";
            notification.transform.parent.gameObject.SetActive(Time.unscaledTime < notificationUntil);
        }

        public void Notify(string message)
        {
            notification.text = message;
            notificationUntil = Time.unscaledTime + 3f;
        }

        public void SetVisible(bool visible) => canvas.enabled = visible;

        public Text CreatePanel(string name, Vector2 anchor, Vector2 offset, Vector2 size, int fontSize)
        {
            var panel = new GameObject(name, typeof(RectTransform), typeof(Image));
            panel.transform.SetParent(canvas.transform, false);
            var rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = anchor;
            rect.anchoredPosition = offset;
            rect.sizeDelta = size;
            var image = panel.GetComponent<Image>();
            image.color = PanelColor;
            image.raycastTarget = false;

            var label = new GameObject("Text", typeof(RectTransform), typeof(Text));
            label.transform.SetParent(panel.transform, false);
            var labelRect = label.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = new Vector2(16, 5);
            labelRect.offsetMax = new Vector2(-16, -5);
            var text = label.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.color = new Color(0.99f, 0.95f, 0.84f);
            text.fontSize = fontSize;
            text.alignment = TextAnchor.MiddleCenter;
            text.raycastTarget = false;
            return text;
        }
    }
}
