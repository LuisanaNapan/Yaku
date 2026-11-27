using UnityEngine;

namespace Yaku.Dialogue
{
    public sealed class DialogueData : ScriptableObject
    {
        [SerializeField] private string speaker;
        [SerializeField] private Sprite portrait;
        [SerializeField, TextArea(2, 4)] private string[] lines;

        public string Speaker => speaker;
        public Sprite Portrait => portrait;
        public int LineCount => lines?.Length ?? 0;
        public string GetLine(int index) => lines[index];
    }
}
