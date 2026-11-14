using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Yaku.Dialogue;
using Yaku.Inventory;
using Yaku.UI;

namespace Yaku.Quests
{
    [RequireComponent(typeof(CommunityHud), typeof(DialogueController))]
    public sealed class TutorialQuest : MonoBehaviour
    {
        public enum Stage
        {
            Briefing,
            Collection,
            Delivery,
            Complete
        }

        [SerializeField] private QuestData quest;
        [SerializeField] private DialogueData briefing;
        [SerializeField] private SupplyPickup supplies;
        [SerializeField] private SupplyDelivery delivery;
        [SerializeField] private NpcDialogue recipient;
        [SerializeField] private GameObject dockGate;

        private DialogueController dialogue;
        private CommunityHud hud;
        private Text objective;
        private Text journal;

        public Stage CurrentStage { get; private set; }

        private void Awake()
        {
            dialogue = GetComponent<DialogueController>();
            hud = GetComponent<CommunityHud>();
            ApplyStage();
        }

        private void OnEnable()
        {
            dialogue.Finished += OnDialogueFinished;
            supplies.Collected += OnCollected;
            delivery.Completed += OnDelivered;
        }

        private void OnDisable()
        {
            dialogue.Finished -= OnDialogueFinished;
            supplies.Collected -= OnCollected;
            delivery.Completed -= OnDelivered;
            recipient.AlternateInteraction = null;
        }

        private void Start()
        {
            objective = hud.CreatePanel("Objective", new Vector2(0, 1), new Vector2(224, -86),
                new Vector2(406, 100), 19);
            objective.alignment = TextAnchor.MiddleLeft;
            journal = hud.CreatePanel("Journal", new Vector2(0.5f, 0.5f), Vector2.zero,
                new Vector2(620, 210), 23);
            journal.transform.parent.gameObject.SetActive(false);
            Render();
        }

        private void Update()
        {
            if (CurrentStage != Stage.Complete || Keyboard.current?.jKey.wasPressedThisFrame != true)
                return;
            var panel = journal.transform.parent.gameObject;
            panel.SetActive(!panel.activeSelf);
        }

        private void OnDialogueFinished(DialogueData completed)
        {
            if (CurrentStage == Stage.Briefing && completed == briefing)
                TransitionTo(Stage.Collection);
        }

        private void OnCollected(GameObject actor)
        {
            if (actor == gameObject && CurrentStage == Stage.Collection)
                TransitionTo(Stage.Delivery);
        }

        private void OnDelivered(GameObject actor)
        {
            if (actor != gameObject || CurrentStage != Stage.Delivery)
                return;
            TransitionTo(Stage.Complete);
            hud.Notify("Misión completada. Diario y embarcadero habilitados.");
        }

        private void TransitionTo(Stage next)
        {
            CurrentStage = next;
            ApplyStage();
            Render();
        }

        private void ApplyStage()
        {
            supplies.Available = CurrentStage == Stage.Collection;
            delivery.Available = CurrentStage == Stage.Delivery;
            recipient.AlternateInteraction = delivery.Available ? delivery : null;
            dockGate.SetActive(CurrentStage != Stage.Complete);
        }

        private void Render()
        {
            if (objective == null)
                return;
            if (CurrentStage == Stage.Complete)
            {
                objective.text = $"<b>{quest.Title}</b> · {quest.ObjectiveCount}/{quest.ObjectiveCount}\nCompletada · [J] Diario";
                journal.text = $"<b>{quest.Title}</b>\n\n{quest.Completion}\n\n[J] Cerrar";
                return;
            }
            int index = (int)CurrentStage;
            objective.text = $"<b>{quest.Title}</b> · {index}/{quest.ObjectiveCount}\n{quest.GetObjective(index)}";
        }
    }
}
