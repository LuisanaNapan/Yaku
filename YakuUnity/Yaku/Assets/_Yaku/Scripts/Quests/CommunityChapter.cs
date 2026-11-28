using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yaku.Dialogue;
using Yaku.Inventory;
using Yaku.UI;

namespace Yaku.Quests
{
    [RequireComponent(typeof(TutorialQuest), typeof(DialogueController), typeof(CommunityHud))]
    public sealed class CommunityChapter : MonoBehaviour
    {
        [SerializeField] private ChapterObjective[] objectives;

        private TutorialQuest tutorial;
        private DialogueController dialogue;
        private CommunityHud hud;
        private ChapterDecision decision;
        private Text objectiveText;
        private int current = -1;

        public int CompletedCount => Mathf.Max(current, 0);
        public int CorrectDecisions { get; private set; }
        public int IncorrectDecisions { get; private set; }
        public IReadOnlyList<ChapterObjective> Objectives => objectives;
        public bool IsComplete => current == objectives.Length;
        public ChapterObjective Active => current >= 0 && current < objectives.Length ? objectives[current] : null;
        public string LastObservation { get; private set; }
        public event Action Completed;

        private void Awake()
        {
            tutorial = GetComponent<TutorialQuest>();
            dialogue = GetComponent<DialogueController>();
            hud = GetComponent<CommunityHud>();
            decision = GetComponent<ChapterDecision>();
        }

        private void OnEnable() => dialogue.Finished += OnDialogueFinished;

        private void OnDisable()
        {
            dialogue.Finished -= OnDialogueFinished;
            if (Active != null) Active.SetAvailable(false);
            current = -1;
            if (objectiveText != null) objectiveText.transform.parent.gameObject.SetActive(false);
            tutorial.SetChapterObjectiveVisible(false);
        }

        private void Start()
        {
            objectiveText = hud.CreatePanel("Chapter Objective", new Vector2(0, 1), new Vector2(224, -86),
                new Vector2(406, 100), 19);
            objectiveText.alignment = TextAnchor.MiddleLeft;
            objectiveText.transform.parent.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (current >= 0 || tutorial.CurrentStage != TutorialQuest.Stage.Complete || objectives.Length == 0)
                return;

            current = 0;
            Active.SetAvailable(true);
            tutorial.SetChapterObjectiveVisible(true);
            objectiveText.transform.parent.gameObject.SetActive(true);
            Render();
        }

        public void Begin(ChapterObjective step)
        {
            if (step != Active) return;
            if (step.HasDecision)
                decision.Open(step, () => Complete(step), () => IncorrectDecisions++);
            else
                dialogue.Open(step.Conversation);
        }

        private void OnDialogueFinished(DialogueData completed)
        {
            var step = Active;
            if (step != null && !step.HasDecision && step.Conversation == completed)
                Complete(step);
        }

        public void UpdateDistribution(CommunityDistribution distribution)
        {
            var step = Active;
            if (step == null || step.Distribution != distribution) return;

            if (distribution.IsComplete)
                Complete(step);
            else
                Render();
        }

        private void Complete(ChapterObjective step)
        {
            if (step != Active) return;
            if (step.Reward != null && !GetComponent<PlayerInventory>().TryAdd(step.Reward, 1))
            {
                hud.Notify("Libera un espacio en la mochila y vuelve a confirmar tu decisión.");
                return;
            }

            step.SetAvailable(false);
            if (step.HasDecision) CorrectDecisions++;
            if (!string.IsNullOrEmpty(step.Observation))
                LastObservation = step.Observation;
            current++;
            if (Active != null) Active.SetAvailable(true);

            if (Active == null || Active.Mission != step.Mission)
                hud.Notify($"Misión completada · {step.Mission}");
            Render();
            if (IsComplete) Completed?.Invoke();
        }

        private void Render()
        {
            var step = Active;
            objectiveText.text = step == null
                ? "<b>Capítulo completado</b> · [J] Diario"
                : $"<b>{step.Mission}</b> · [J] Diario\n{step.Instruction}";
            if (step != null && step.Distribution != null)
                objectiveText.text += $"\n<size=15>{step.Distribution.Status}</size>";
        }
    }
}
