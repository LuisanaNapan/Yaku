using UnityEngine;
using Yaku.UI;

namespace Yaku.Interaction
{
    public sealed class InteractionSign : MonoBehaviour, IInteractable
    {
        [SerializeField] private string description = "Sendero comunitario";

        public string Prompt => "Interactuar";
        public void Interact(GameObject actor) => actor.GetComponent<CommunityHud>().Notify(description);
    }
}
