using System;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Yaku.Interaction;
using Yaku.Player;
using Yaku.UI;

namespace Yaku.Quests
{
    [RequireComponent(typeof(InteractionDetector), typeof(PlayerMovement), typeof(CommunityHud))]
    public sealed class ChapterDecision : MonoBehaviour
    {
        private Text message;
        private PlayerMovement movement;
        private InteractionDetector detector;
        private CharacterController controller;
        private ChapterObjective active;
        private Action onCorrect;
        private bool restoreMovement;
        private bool accepted;

        public bool IsOpen => active != null;

        private void Start()
        {
            detector = GetComponent<InteractionDetector>();
            movement = GetComponent<PlayerMovement>();
            controller = GetComponent<CharacterController>();
            message = GetComponent<CommunityHud>().CreatePanel("Decision", new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(750, 330), 23);
            message.alignment = TextAnchor.MiddleLeft;
            message.transform.parent.gameObject.SetActive(false);
        }

        public void Open(ChapterObjective step, Action completed)
        {
            if (message == null || IsOpen || !controller.isGrounded) return;

            active = step;
            onCorrect = completed;
            accepted = false;
            restoreMovement = movement.enabled;
            movement.enabled = false;
            detector.SetModalInteraction(Dismiss);
            message.transform.parent.gameObject.SetActive(true);
            Render();
        }

        private void Update()
        {
            if (!IsOpen || accepted) return;

            var keyboard = Keyboard.current;
            if (keyboard == null) return;
            if (keyboard.digit1Key.wasPressedThisFrame) Choose(0);
            else if (keyboard.digit2Key.wasPressedThisFrame) Choose(1);
            else if (keyboard.digit3Key.wasPressedThisFrame) Choose(2);
        }

        private void Choose(int index)
        {
            if (index >= active.Choices.Length) return;

            accepted = index == active.CorrectChoice;
            if (accepted)
                message.text = $"<b>Decisión informada</b>\n\n{active.CorrectFeedback}\n\n[E] Continuar";
            else
                Render(active.RetryFeedback);
        }

        private void Render(string feedback = null)
        {
            var text = new StringBuilder().Append("<b>").Append(active.Question).AppendLine("</b>").AppendLine();
            for (int i = 0; i < active.Choices.Length; i++)
                text.Append('[').Append(i + 1).Append("] ").AppendLine(active.Choices[i]);
            if (feedback != null) text.AppendLine().AppendLine(feedback);
            text.AppendLine().Append("[E] Volver");
            message.text = text.ToString();
        }

        private void Dismiss()
        {
            if (!IsOpen) return;
            var completed = accepted ? onCorrect : null;
            Close();
            completed?.Invoke();
        }

        private void Close()
        {
            if (!IsOpen) return;
            active = null;
            onCorrect = null;
            message.transform.parent.gameObject.SetActive(false);
            detector.SetModalInteraction(null);
            movement.enabled = restoreMovement;
        }

        private void OnDisable() => Close();
    }
}
