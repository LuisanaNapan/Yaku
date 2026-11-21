using UnityEngine;
using UnityEngine.UI;
using Yaku.Interaction;

namespace Yaku.UI
{
    [RequireComponent(typeof(InteractionDetector))]
    public sealed class CommunityHud : MonoBehaviour
    {
        private static readonly Color PanelColor = new Color(0.052f, 0.13f, 0.105f, 0.965f);
        internal static readonly Color TextColor = new Color(0.99f, 0.96f, 0.86f);
        internal static readonly Color AccentColor = new Color(0.80f, 0.69f, 0.42f);
        private InteractionDetector detector;
        private Canvas canvas;
        private Text prompt;
        private Text notification;
        private GameAudio audioController;
        private float notificationUntil;

        private void Awake()
        {
            detector = GetComponent<InteractionDetector>();
            audioController = GetComponent<GameAudio>();
            var overlay = new GameObject("Community HUD", typeof(Canvas), typeof(CanvasScaler));
            overlay.transform.SetParent(transform, false);
            canvas = overlay.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = overlay.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280, 720);
            scaler.matchWidthOrHeight = 0.5f;
            prompt = CreatePanel("Interaction", new Vector2(0.5f, 0f), new Vector2(0, 46), new Vector2(410, 48), 22);
            notification = CreatePanel("Message", new Vector2(0.5f, 1f), new Vector2(0, -51), new Vector2(484, 64), 19);
            var packHint = CreatePanel("Inventory Hint", new Vector2(1f, 0f), new Vector2(-98, 35),
                new Vector2(160, 37), 17);
            packHint.text = "<b>[I]</b> Mochila";
        }

        private void LateUpdate()
        {
            string hint = detector.Prompt;
            prompt.transform.parent.gameObject.SetActive(hint != null);
            prompt.text = hint == null ? string.Empty : $"<color=#E3C680><b>[E]</b></color> {hint}";
            notification.transform.parent.gameObject.SetActive(Time.time < notificationUntil);
        }

        public void Notify(string message, GameAudio.Cue cue = GameAudio.Cue.None)
        {
            notification.text = message;
            notificationUntil = Time.time + 3.5f;
            audioController?.Queue(cue);
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
            var border = panel.AddComponent<Outline>();
            border.effectColor = new Color(0.59f, 0.60f, 0.37f, 0.62f);
            border.effectDistance = new Vector2(1.25f, 1.25f);

            var label = new GameObject("Text", typeof(RectTransform), typeof(Text));
            label.transform.SetParent(panel.transform, false);
            var labelRect = label.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = new Vector2(16, 5);
            labelRect.offsetMax = new Vector2(-16, -5);
            var text = label.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.color = TextColor;
            text.fontSize = fontSize;
            text.alignment = TextAnchor.MiddleCenter;
            text.raycastTarget = false;
            return text;
        }
    }
}
