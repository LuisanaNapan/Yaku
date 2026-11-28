using UnityEngine;

namespace Yaku.Dialogue
{
    public sealed class DialogueData : ScriptableObject
    {
        [SerializeField] private string speaker;
        [SerializeField] private Sprite portrait;
        [SerializeField, TextArea(2, 4)] private string[] lines;
        [SerializeField] private AudioClip[] voiceLines;

        public string Speaker => speaker;
        public Sprite Portrait => portrait;
        public int LineCount => lines?.Length ?? 0;
        public int VoiceLineCount => voiceLines?.Length ?? 0;
        public string GetLine(int index) => lines[index];
        public AudioClip GetVoiceLine(int index) =>
            voiceLines != null && index >= 0 && index < voiceLines.Length ? voiceLines[index] : null;
    }
}
