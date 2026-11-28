using System.Collections;
using UnityEngine;
using Yaku.UI;

namespace Yaku.Quests
{
    [RequireComponent(typeof(CommunityChapter), typeof(CommunityHud), typeof(MissionGuide))]
    public sealed class MercurioEncounter : MonoBehaviour
    {
        [SerializeField] private ChapterObjective riverMonitoring;
        [SerializeField] private GameObject silhouette;
        [SerializeField, Min(1f)] private float duration = 7f;

        private CommunityChapter chapter;
        private CommunityHud hud;
        private MissionGuide guide;
        private bool seen;
        private Vector3 rest;
        private Quaternion facing;

        private void Awake()
        {
            chapter = GetComponent<CommunityChapter>();
            hud = GetComponent<CommunityHud>();
            guide = GetComponent<MissionGuide>();
            if (silhouette == null) return;
            rest = silhouette.transform.position;
            facing = silhouette.transform.rotation;
            silhouette.SetActive(false);
        }

        private void OnEnable() => chapter.ObjectiveCompleted += OnObjectiveCompleted;

        private void OnDisable()
        {
            chapter.ObjectiveCompleted -= OnObjectiveCompleted;
            StopAllCoroutines();
            Hide();
        }

        private void OnObjectiveCompleted(ChapterObjective objective)
        {
            if (seen || objective != riverMonitoring || silhouette == null) return;
            seen = true;
            StartCoroutine(Reveal());
        }

        private IEnumerator Reveal()
        {
            Transform figure = silhouette.transform;
            silhouette.SetActive(true);
            guide.FocusOn(figure);
            hud.Notify("Yaku · ¿Qué fue esa sombra en la mina?", GameAudio.Cue.Interact);

            float elapsed = 0f;
            while (elapsed < duration)
            {
                float reveal = Mathf.SmoothStep(0f, 1f, Mathf.Min(elapsed, duration - elapsed) / 1.2f);
                figure.position = rest + Vector3.down * (1f - reveal) * 2.1f;
                figure.rotation = facing * Quaternion.Euler(0f, 13f * (1f - reveal), 0f);
                yield return null;
                elapsed += Time.deltaTime;
            }

            Hide();
            if (chapter.Active != null && !GetComponent<Yaku.Interaction.InteractionDetector>().HasModalInteraction)
                hud.Notify("Una sombra no demuestra contaminación. Decide con los avisos del río.");
        }

        private void Hide()
        {
            if (silhouette == null) return;
            if (guide != null) guide.FocusOn(null);
            silhouette.transform.SetPositionAndRotation(rest, facing);
            silhouette.SetActive(false);
        }
    }
}
