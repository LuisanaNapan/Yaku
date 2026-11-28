using UnityEngine;
using UnityEngine.UI;
using Yaku.Dialogue;
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
        public ChapterObjective Active => current >= 0 && current < objectives.Length ? objectives[current] : null;
        public string LastObservation { get; private set; }

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
                decision.Open(step, () => Complete(step));
            else
                dialogue.Open(step.Conversation);
        }

        private void OnDialogueFinished(DialogueData completed)
        {
            var step = Active;
            if (step != null && !step.HasDecision && step.Conversation == completed)
                Complete(step);
        }

        private void Complete(ChapterObjective step)
        {
            if (step != Active) return;

            step.SetAvailable(false);
            if (!string.IsNullOrEmpty(step.Observation))
                LastObservation = step.Observation;
            current++;
            if (Active != null) Active.SetAvailable(true);

            if (Active == null || Active.Mission != step.Mission)
                hud.Notify($"Misión completada · {step.Mission}");
            Render();
        }

        private void Render()
        {
            var step = Active;
            objectiveText.text = step == null
                ? $"<b>{objectives[objectives.Length - 1].Mission}</b>\nCompletada"
                : $"<b>{step.Mission}</b>\n{step.Instruction}";
        }
    }
}
