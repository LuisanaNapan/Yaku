using UnityEngine;
using Yaku.Dialogue;
using Yaku.Interaction;

namespace Yaku.Quests
{
    public sealed class ChapterObjective : MonoBehaviour, IInteractable
    {
        [SerializeField] private string mission;
        [SerializeField] private string instruction;
        [SerializeField] private string prompt;
        [SerializeField] private DialogueData conversation;
        [SerializeField] private NpcDialogue npc;
        [SerializeField] private Collider interactionTrigger;
        [SerializeField, TextArea] private string question;
        [SerializeField] private string[] choices;
        [SerializeField] private int correctChoice;
        [SerializeField, TextArea] private string correctFeedback;
        [SerializeField, TextArea] private string retryFeedback;
        [SerializeField, TextArea] private string observation;

        public string Mission => mission;
        public string Instruction => instruction;
        public string Prompt => prompt;
        public DialogueData Conversation => conversation;
        public bool HasDecision => !string.IsNullOrWhiteSpace(question);
        public string Question => question;
        public string[] Choices => choices;
        public int CorrectChoice => correctChoice;
        public string CorrectFeedback => correctFeedback;
        public string RetryFeedback => retryFeedback;
        public string Observation => observation;

        public void SetAvailable(bool available)
        {
            if (interactionTrigger != null) interactionTrigger.enabled = available;
            if (npc == null) return;
            if (available)
                npc.AlternateInteraction = this;
            else if (ReferenceEquals(npc.AlternateInteraction, this))
                npc.AlternateInteraction = null;
        }

        public void Interact(GameObject actor) => actor.GetComponent<CommunityChapter>()?.Begin(this);
    }
}
