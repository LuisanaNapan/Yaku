using UnityEngine;
namespace Yaku.Interaction
{
    public interface IInteractable
    {
        string Prompt { get; }
        void Interact(GameObject actor);
    }
}
