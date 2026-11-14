using UnityEngine;
using Yaku.Interaction;

namespace Yaku.Dialogue
{
    public sealed class NpcDialogue : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueData dialogue;

        public string Prompt => $"Hablar con {dialogue.Speaker}";

        public void Interact(GameObject actor) => actor.GetComponent<DialogueController>().Open(dialogue);
    }
}
