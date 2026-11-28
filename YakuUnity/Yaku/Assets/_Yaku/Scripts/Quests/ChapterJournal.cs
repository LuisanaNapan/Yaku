using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Yaku.Interaction;
using Yaku.Player;
using Yaku.UI;

namespace Yaku.Quests
{
    [RequireComponent(typeof(CommunityChapter), typeof(PlayerMovement), typeof(InteractionDetector))]
    public sealed class ChapterJournal : MonoBehaviour
    {
        private CommunityChapter chapter;
        private TutorialQuest tutorial;
        private CharacterController controller;
        private CommunityDistribution distribution;
        private InteractionDetector detector;
        private PlayerMovement movement;
        private Text label;
        private bool restoreMovement;
        private bool observations;
        private bool finale;

        public bool IsOpen => label != null && label.transform.parent.gameObject.activeSelf;

        private void Awake()
        {
            chapter = GetComponent<CommunityChapter>();
            tutorial = GetComponent<TutorialQuest>();
            controller = GetComponent<CharacterController>();
            distribution = GetComponent<CommunityDistribution>();
            detector = GetComponent<InteractionDetector>();
            movement = GetComponent<PlayerMovement>();
            tutorial.UseChapterJournal = true;
        }

        private void OnEnable() => chapter.Completed += OpenFinale;

        private void Start()
        {
            label = GetComponent<CommunityHud>().CreatePanel("Chapter Journal", new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(770, 510), 21);
            label.alignment = TextAnchor.MiddleLeft;
            label.transform.parent.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Time.timeScale <= 0f) return;
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            if (keyboard.jKey.wasPressedThisFrame)
            {
                if (IsOpen) Close();
                else Open();
                return;
            }
            if (!IsOpen || finale) return;

            if (keyboard.digit1Key.wasPressedThisFrame)
            {
                observations = false;
                Render();
            }
            else if (keyboard.digit2Key.wasPressedThisFrame)
            {
                observations = true;
                Render();
            }
        }

        private void Open() => Open(false);

        private void OpenFinale() => Open(true);

        private void Open(bool showFinale)
        {
            if (label == null || IsOpen || detector.HasModalInteraction || !controller.isGrounded) return;

            observations = false;
            finale = showFinale;
            restoreMovement = movement.enabled;
            movement.enabled = false;
            detector.SetModalInteraction(Close);
            label.transform.parent.gameObject.SetActive(true);
            Render();
        }

        private void Close()
        {
            if (!IsOpen) return;

            label.transform.parent.gameObject.SetActive(false);
            detector.SetModalInteraction(null);
            movement.enabled = restoreMovement;
        }

        private void Render()
        {
            var panel = (RectTransform)label.transform.parent;
            panel.sizeDelta = finale ? new Vector2(700, 310) : new Vector2(770, 510);
            label.alignment = finale ? TextAnchor.MiddleCenter : TextAnchor.MiddleLeft;
            if (finale)
            {
                label.text = "<size=31><color=#E3C680><b>Capítulo completado</b></color></size>\n\n" +
                    "<b>Elena</b> · Gracias, Yaku. Hoy cuidamos juntos a la comunidad.\n\n" +
                    "Consulta los avisos sobre el agua. Verifica el origen de los alimentos.\n" +
                    "Busca orientación en la posta y comparte los suministros.\n" +
                    $"<b>Entregas completadas · {distribution.DeliveredCount}/{distribution.TotalCount}</b>\n\n" +
                    "<size=17>[E/J] Seguir explorando     [Esc] Menú de pausa</size>";
                return;
            }

            var text = new StringBuilder();
            text.AppendLine("<b>Diario de Yaku</b>");
            text.AppendLine("[1] Resumen     [2] Aprendizajes     [J/E] Cerrar").AppendLine();
            if (observations)
                RenderObservations(text);
            else
                RenderSummary(text);
            label.text = text.ToString();
        }

        private void RenderSummary(StringBuilder text)
        {
            bool tutorialDone = tutorial.CurrentStage == TutorialQuest.Stage.Complete;
            text.AppendLine($"El primer encargo · {(tutorialDone ? "Completado" : "En curso")}");
            int index = 0;
            foreach (var mission in chapter.Objectives.GroupBy(step => step.Mission))
            {
                int count = mission.Count();
                int completed = Mathf.Clamp(chapter.CompletedCount - index, 0, count);
                bool active = chapter.Active != null && chapter.Active.Mission == mission.Key;
                string state = completed == count ? "Completada" : active ? "En curso" : "Pendiente";
                text.AppendLine($"{mission.Key} · {state} ({completed}/{count})");
                index += count;
            }

            text.AppendLine();
            if (chapter.Active != null)
                text.AppendLine($"<b>Objetivo activo</b> · {chapter.Active.Instruction}");
            text.AppendLine($"Entregas comunitarias · {distribution.DeliveredCount}/{distribution.TotalCount}");
            text.AppendLine($"Decisiones informadas · {chapter.CorrectDecisions}     Intentos corregidos o pendientes · {chapter.IncorrectDecisions}");
        }

        private void RenderObservations(StringBuilder text)
        {
            var recorded = chapter.Objectives.Take(chapter.CompletedCount)
                .Where(step => !string.IsNullOrWhiteSpace(step.Observation))
                .GroupBy(step => step.Mission);
            bool hasNotes = false;
            foreach (var mission in recorded)
            {
                hasNotes = true;
                text.AppendLine($"<b>{mission.Key}</b> · {mission.Last().Observation}");
            }
            if (!hasNotes)
                text.Append("Completa los objetivos para registrar observaciones y aprendizajes.");
        }

        private void OnDisable()
        {
            chapter.Completed -= OpenFinale;
            Close();
        }
    }
}
