using UnityEngine;
using UnityEngine.UI;
using Yaku.Interaction;
using Yaku.UI;

namespace Yaku.Quests
{
    [RequireComponent(typeof(TutorialQuest), typeof(CommunityChapter), typeof(InteractionDetector))]
    public sealed class MissionGuide : MonoBehaviour
    {
        [SerializeField] private Transform posta;
        [SerializeField] private Camera view;

        private TutorialQuest tutorial;
        private CommunityChapter chapter;
        private InteractionDetector detector;
        private Text marker;
        private RectTransform markerRect;
        private RectTransform canvasRect;
        private Transform focus;

        private float focusHeight;

        public void FocusOn(Transform point, float height = 0f)
        {
            focus = point;
            focusHeight = height;
        }

        private void Start()
        {
            tutorial = GetComponent<TutorialQuest>();
            chapter = GetComponent<CommunityChapter>();
            detector = GetComponent<InteractionDetector>();
            marker = GetComponent<CommunityHud>().CreatePanel("Mission Waypoint", Vector2.one * 0.5f,
                Vector2.zero, new Vector2(88f, 32f), 16);
            marker.color = CommunityHud.AccentColor;
            marker.rectTransform.offsetMin = Vector2.zero;
            marker.rectTransform.offsetMax = Vector2.zero;
            markerRect = (RectTransform)marker.transform.parent;
            canvasRect = (RectTransform)markerRect.parent;
            markerRect.gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            Transform destination = tutorial.CurrentStage == TutorialQuest.Stage.Briefing
                ? posta : tutorial.Destination;
            if (tutorial.CurrentStage == TutorialQuest.Stage.Complete)
                destination = chapter.Active?.Destination;
            if (focus != null) destination = focus;

            bool visible = destination != null && view != null && Time.timeScale > 0f &&
                !detector.HasModalInteraction &&
                (destination.position - transform.position).sqrMagnitude > 6.25f;
            markerRect.gameObject.SetActive(visible);
            if (!visible) return;

            float markerHeight = focus == null ? 1.85f : focusHeight;
            Vector3 screen = view.WorldToViewportPoint(destination.position + Vector3.up * markerHeight);
            Vector2 direction = new Vector2(screen.x - 0.5f, screen.y - 0.5f);
            if (screen.z <= 0f)
            {
                float side = direction.x > 0f ? -1f : 1f;
                direction = new Vector2(side, 0f);
            }

            Rect bounds = canvasRect.rect;
            float x = Mathf.Clamp(direction.x * bounds.width, bounds.xMin + 58f, bounds.xMax - 58f);
            float y = Mathf.Clamp(direction.y * bounds.height, bounds.yMin + 116f, bounds.yMax - 116f);
            markerRect.anchoredPosition = new Vector2(x, y);
            string symbol = screen.z <= 0f || Mathf.Abs(direction.x) >= 0.5f
                ? (direction.x < 0f ? "‹" : "›") : "◆";
            marker.text = focus == null
                ? $"{symbol} {Mathf.RoundToInt(Vector3.Distance(transform.position, destination.position))} m"
                : $"{symbol} Mina";
        }
    }
}
