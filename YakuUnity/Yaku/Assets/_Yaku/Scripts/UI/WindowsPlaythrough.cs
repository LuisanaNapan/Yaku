#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using Yaku.Dialogue;
using Yaku.Interaction;
using Yaku.Quests;

namespace Yaku.UI
{
    // Launch YAKU.exe -yaku-smoke to verify the packaged scene using the normal gameplay inputs.
    internal sealed class WindowsPlaythrough : MonoBehaviour
    {
        private GameMenu menu;
        private CharacterController controller;
        private CommunityChapter chapter;
        private TutorialQuest tutorial;
        private DialogueController dialogue;
        private ChapterJournal journal;
        private Keyboard keyboard;
        private int checks;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void StartWhenRequested()
        {
            if (Array.IndexOf(Environment.GetCommandLineArgs(), "-yaku-smoke") >= 0)
                new GameObject("YAKU Windows verification").AddComponent<WindowsPlaythrough>();
        }

        private IEnumerator Start()
        {
            yield return null;
            menu = FindAnyObjectByType<GameMenu>();
            Require(menu != null && !menu.IsPlaying && Time.timeScale == 0f, "menú inicial");
            controller = menu.GetComponent<CharacterController>();
            chapter = menu.GetComponent<CommunityChapter>();
            tutorial = menu.GetComponent<TutorialQuest>();
            dialogue = menu.GetComponent<DialogueController>();
            journal = menu.GetComponent<ChapterJournal>();
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;

            var play = EventSystem.current?.currentSelectedGameObject?.GetComponent<Button>();
            Require(play != null, "selección del botón Jugar");
            play.onClick.Invoke();
            Require(menu.IsPlaying && Time.timeScale == 1f, "transición a la comunidad");
            InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
            keyboard = InputSystem.AddDevice<Keyboard>();

            Vector3 origin = menu.transform.position;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W));
            yield return new WaitForSecondsRealtime(0.45f);
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return null;
            Require(Vector3.Distance(menu.transform.position, origin) > 0.2f, "movimiento mediante WASD");

            yield return Press(Key.Escape);
            Require(Time.timeScale == 0f && !menu.IsPlaying, "pausa de Windows");
            yield return Press(Key.Escape);
            Require(Time.timeScale == 1f && menu.IsPlaying, "reanudación");

            yield return Speak(1.10f, 5.40f);
            yield return At(-4.95f, 2.40f);
            yield return At(-2.51f, -3.97f);
            Require(tutorial.CurrentStage == TutorialQuest.Stage.Complete, "tutorial y entrega inicial");

            yield return Speak(1.10f, 5.40f);
            yield return Speak(3.80f, 5.40f);
            yield return At(5.00f, 3.20f);
            yield return Press(Key.Digit1);
            Require(chapter.CompletedCount == 2, "elección incorrecta de agua rechazada");
            yield return Answer(Key.Digit2);
            yield return Speak(1.10f, 5.40f);
            Require(chapter.CompletedCount == 4, "misión del agua");

            yield return Speak(2.65f, 9.70f);
            yield return Speak(0.90f, 9.65f);
            yield return At(0.90f, 9.65f);
            yield return Answer(Key.Digit2);
            yield return At(2.65f, 9.70f);
            yield return Answer(Key.Digit2);
            Require(chapter.CompletedCount == 8, "misión de alimentación");

            yield return Speak(-2.51f, -3.97f);
            yield return Speak(1.10f, 5.40f);
            yield return At(0.05f, 5.55f);
            yield return Answer(Key.Digit1);
            yield return At(1.10f, 5.40f);
            yield return Answer(Key.Digit3);
            Require(chapter.CompletedCount == 12, "misión de orientación");

            yield return Speak(1.10f, 5.40f);
            yield return At(-5.85f, 2.35f);
            yield return At(-4.95f, 2.90f);
            yield return At(-4.00f, 2.40f);
            yield return At(-2.51f, -3.97f);
            yield return At(2.65f, 9.70f);
            yield return At(1.10f, 5.40f);
            Require(chapter.CompletedCount == 14, "tres recogidas y entregas");
            yield return At(1.10f, 5.40f);
            yield return Answer(Key.Digit1);
            yield return Speak(1.10f, 5.40f);
            Require(chapter.IsComplete && journal.IsOpen, "cierre narrativo y resumen del diario");

            yield return Press(Key.J);
            Require(!journal.IsOpen && menu.IsPlaying, "diario se cierra y devuelve el control");
            Debug.Log($"YAKU_WINDOWS_SMOKE_OK · {checks} comprobaciones");
            InputSystem.RemoveDevice(keyboard);
            Application.Quit(0);
        }

        private IEnumerator Speak(float x, float z)
        {
            yield return At(x, z);
            var prompt = menu.GetComponent<InteractionDetector>().Prompt;
            Require(dialogue.IsOpen, $"apertura del diálogo mediante E en ({x}, {z}); aviso {prompt}; suelo {controller.isGrounded}; posición {menu.transform.position}");
            for (int line = 0; line < 8 && dialogue.IsOpen; line++)
                yield return Press(Key.E);
            Require(!dialogue.IsOpen, "avance y cierre del diálogo");
        }

        private IEnumerator At(float x, float z)
        {
            controller.enabled = false;
            var point = new Vector3(x, 0.95f, z);
            var floors = Physics.RaycastAll(point, Vector3.down, 2f, ~0, QueryTriggerInteraction.Ignore)
                .Where(hit => hit.normal.y > 0.65f && hit.collider.gameObject != menu.gameObject)
                .OrderBy(hit => hit.distance);
            var floor = floors.FirstOrDefault();
            Require(floor.collider != null, "suelo en punto de prueba");
            menu.transform.position = floor.point + Vector3.up * 0.12f;
            controller.enabled = true;
            Physics.SyncTransforms();
            controller.Move(Vector3.down * 0.24f);
            yield return new WaitForSecondsRealtime(0.10f);
            Require(controller.isGrounded, $"contacto con el suelo en ({x}, {z}); posición {menu.transform.position}");
            yield return Press(Key.E);
        }

        private IEnumerator Answer(Key choice)
        {
            yield return Press(choice);
            yield return Press(Key.E);
        }

        private IEnumerator Press(Key key)
        {
            keyboard.MakeCurrent();
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(key));
            yield return null;
            yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return null;
            yield return null;
        }

        private void Require(bool condition, string stage)
        {
            if (!condition)
            {
                Debug.LogError($"YAKU_WINDOWS_SMOKE_FAILED · {stage} · {chapter?.CompletedCount}");
                Application.Quit(1);
                throw new InvalidOperationException(stage);
            }
            checks++;
        }
    }
}
#endif
