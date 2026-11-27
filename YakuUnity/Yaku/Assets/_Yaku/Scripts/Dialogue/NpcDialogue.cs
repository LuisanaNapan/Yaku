using UnityEngine;
using Yaku.Interaction;

namespace Yaku.Dialogue
{
    public sealed class NpcDialogue : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueData dialogue;

        public IInteractable AlternateInteraction { get; set; }

        public string Prompt => AlternateInteraction?.Prompt ?? $"Hablar con {dialogue.Speaker}";

        public void Interact(GameObject actor)
        {
            if (AlternateInteraction != null)
                AlternateInteraction.Interact(actor);
            else
                actor.GetComponent<DialogueController>().Open(dialogue);
        }
    }
}
