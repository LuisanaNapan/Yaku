using System;
using UnityEngine;
using UnityEngine.UI;
using Yaku.Interaction;
using Yaku.Player;
using Yaku.UI;

namespace Yaku.Dialogue
{
    [RequireComponent(typeof(CommunityHud))]
    public sealed class DialogueController : MonoBehaviour
    {
        private InteractionDetector detector;
        private PlayerMovement movement;
        private Text message;
        private Image portrait;
        private DialogueData active;
        private int line;
        private bool restoreMovement;

        public bool IsOpen => active != null;
        public event Action<DialogueData> Finished;

        private void Start()
        {
            detector = GetComponent<InteractionDetector>();
            movement = GetComponent<PlayerMovement>();
            message = GetComponent<CommunityHud>().CreatePanel("Dialogue", new Vector2(0.5f, 0f),
                new Vector2(0, 132), new Vector2(840, 172), 23);
            message.supportRichText = true;
            var icon = new GameObject("Portrait", typeof(RectTransform), typeof(Image));
            icon.transform.SetParent(message.transform.parent, false);
            portrait = icon.GetComponent<Image>();
            portrait.rectTransform.anchorMin = portrait.rectTransform.anchorMax = new Vector2(0, 0.5f);
            portrait.rectTransform.anchoredPosition = new Vector2(75, 0);
            portrait.rectTransform.sizeDelta = new Vector2(100, 100);
            portrait.preserveAspect = true;
            portrait.raycastTarget = false;
            message.transform.parent.gameObject.SetActive(false);
        }

        public bool Open(DialogueData data)
        {
            if (message == null || IsOpen || data == null || data.LineCount == 0 ||
                !GetComponent<CharacterController>().isGrounded)
                return false;

            active = data;
            line = 0;
            restoreMovement = movement.enabled;
            movement.enabled = false;
            detector.SetModalInteraction(Advance);
            portrait.sprite = data.Portrait;
            portrait.gameObject.SetActive(portrait.sprite != null);
            message.rectTransform.offsetMin = new Vector2(portrait.sprite == null ? 16 : 136, 5);
            message.transform.parent.gameObject.SetActive(true);
            RenderLine();
            return true;
        }

        private void Advance()
        {
            if (++line < active.LineCount)
            {
                RenderLine();
                return;
            }

            DialogueData finished = active;
            Close();
            Finished?.Invoke(finished);
        }

        private void RenderLine()
        {
            string action = line + 1 < active.LineCount ? "Continuar" : "Cerrar";
            message.text = $"<color=#E3C680><b>{active.Speaker}</b></color>\n{active.GetLine(line)}\n<size=19>[E] {action}</size>";
        }

        private void Close()
        {
            if (!IsOpen) return;
            active = null;
            message.transform.parent.gameObject.SetActive(false);
            detector.SetModalInteraction(null);
            movement.enabled = restoreMovement;
        }

        private void OnDisable() => Close();
    }
}
