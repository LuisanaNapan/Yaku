using UnityEngine;

namespace Yaku.Quests
{
    public sealed class QuestData : ScriptableObject
    {
        [SerializeField] private string title;
        [SerializeField] private string[] objectives;
        [SerializeField, TextArea] private string completion;

        public string Title => title;
        public int ObjectiveCount => objectives.Length;
        public string Completion => completion;
        public string GetObjective(int index) => objectives[index];
    }
}
