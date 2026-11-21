using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Yaku.UI
{
    [RequireComponent(typeof(CommunityHud))]
    public sealed class GameMenu : MonoBehaviour
    {
        private enum ScreenState { Title, Playing, Paused, Settings, Return, Exit }

        private static readonly Color Ink = new Color(0.043f, 0.13f, 0.105f, 0.98f);
        private static readonly Color Sand = new Color(0.98f, 0.93f, 0.78f);
        private static readonly Color Green = new Color(0.19f, 0.37f, 0.27f);
        private ScreenState state;
        private ScreenState previous;
        private CommunityHud hud;
        private GameObject overlay;
        private RectTransform panel;
        private Text heading;
        private Text detail;
        private Button[] buttons;
        private readonly UnityEngine.Events.UnityAction[] options = new UnityEngine.Events.UnityAction[4];
        private InputAction back;
        private bool cursorWasLocked;
        private bool loading;

        public bool IsPlaying => state == ScreenState.Playing;

        private void Start()
        {
            hud = GetComponent<CommunityHud>();
            CreateMenu();
            Show(ScreenState.Title);
        }

        private void Awake() => back = new InputAction("Pause", binding: "<Keyboard>/escape");
        private void OnEnable() => back.Enable();
        private void OnDisable() => back.Disable();

        private void Update()
        {
            if (loading || !back.WasPressedThisFrame()) return;
            switch (state)
            {
                case ScreenState.Playing:
                    cursorWasLocked = Cursor.lockState == CursorLockMode.Locked;
                    Show(ScreenState.Paused);
                    break;
                case ScreenState.Paused:
                    Resume();
                    break;
                case ScreenState.Settings:
                case ScreenState.Return:
                case ScreenState.Exit:
                    Show(previous);
                    break;
            }
        }

        private void CreateMenu()
        {
            overlay = new GameObject("Menu YAKU", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            overlay.transform.SetParent(transform, false);
            var canvas = overlay.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 50;
            var scaler = overlay.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280, 720);
            scaler.matchWidthOrHeight = 0.5f;

            var shade = ImageAt(overlay.transform, "Fondo", new Color(0.025f, 0.10f, 0.085f, 0.61f));
            shade.rectTransform.anchorMin = Vector2.zero;
            shade.rectTransform.anchorMax = Vector2.one;
            shade.rectTransform.offsetMin = shade.rectTransform.offsetMax = Vector2.zero;

            panel = ImageAt(overlay.transform, "Panel", Ink).rectTransform;
            Place(panel, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(500, 494));

            var ribbon = ImageAt(panel, "Borde", new Color(0.81f, 0.64f, 0.31f));
            Place(ribbon.rectTransform, new Vector2(0.5f, 1), new Vector2(0, -3), new Vector2(500, 6));
            heading = Label(panel, "Título", 48);
            Place(heading.rectTransform, new Vector2(0.5f, 1), new Vector2(0, -78), new Vector2(440, 65));
            detail = Label(panel, "Descripción", 23);
            Place(detail.rectTransform, new Vector2(0.5f, 1), new Vector2(0, -149), new Vector2(442, 72));

            buttons = new Button[4];
            for (int i = 0; i < buttons.Length; i++)
            {
                var background = ImageAt(panel, $"Opción {i + 1}", Green);
                Place(background.rectTransform, new Vector2(0.5f, 1), new Vector2(0, -220 - i * 59), new Vector2(338, 49));
                var button = background.gameObject.AddComponent<Button>();
                int index = i;
                button.onClick.AddListener(() => options[index]?.Invoke());
                button.targetGraphic = background;
                var colors = button.colors;
                colors.highlightedColor = new Color(0.83f, 0.91f, 0.67f);
                colors.selectedColor = colors.highlightedColor;
                colors.pressedColor = new Color(0.65f, 0.80f, 0.61f);
                button.colors = colors;

                var label = Label(button.transform, "Texto", 23);
                label.rectTransform.anchorMin = Vector2.zero;
                label.rectTransform.anchorMax = Vector2.one;
                label.rectTransform.offsetMin = label.rectTransform.offsetMax = Vector2.zero;
                buttons[i] = button;
            }

            if (EventSystem.current == null)
            {
                var input = new GameObject("Menu Input", typeof(EventSystem), typeof(InputSystemUIInputModule));
                input.transform.SetParent(transform, false);
            }
        }

        private void Show(ScreenState next)
        {
            state = next;
            bool playing = IsPlaying;
            Time.timeScale = playing ? 1f : 0f;
            overlay.SetActive(!playing);
            hud.SetVisible(playing);
            Cursor.lockState = playing && cursorWasLocked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = Cursor.lockState != CursorLockMode.Locked;
            if (playing) return;

            foreach (var button in buttons) button.gameObject.SetActive(false);
            switch (state)
            {
                case ScreenState.Title:
                    heading.text = "YAKU";
                    detail.text = "Guardianes de la Vida";
                    Option(0, "Jugar", StartGame);
                    Option(1, "Configuración", () => OpenFrom(ScreenState.Settings));
                    Option(2, "Salir", () => OpenFrom(ScreenState.Exit));
                    break;
                case ScreenState.Paused:
                    heading.text = "Pausa";
                    detail.text = "Tu progreso se conserva al reanudar.";
                    Option(0, "Reanudar", Resume);
                    Option(1, "Configuración", () => OpenFrom(ScreenState.Settings));
                    Option(2, "Volver al inicio", () => OpenFrom(ScreenState.Return));
                    Option(3, "Salir", () => OpenFrom(ScreenState.Exit));
                    break;
                case ScreenState.Settings:
                    heading.text = "Configuración";
                    detail.text = "WASD · Mover   Shift · Correr\nEspacio · Saltar   E · Interactuar";
                    Option(0, Screen.fullScreen ? "Usar modo ventana" : "Usar pantalla completa", ToggleDisplay);
                    Option(1, "Volver", () => Show(previous));
                    break;
                case ScreenState.Return:
                case ScreenState.Exit:
                    bool exit = state == ScreenState.Exit;
                    heading.text = exit ? "Salir de YAKU" : "Volver al inicio";
                    detail.text = previous == ScreenState.Paused ? "El progreso de la partida actual se perderá." : "¿Quieres cerrar el juego?";
                    Option(0, "Cancelar", () => Show(previous));
                    Option(1, exit ? "Confirmar salida" : "Confirmar regreso", exit ? (UnityEngine.Events.UnityAction)Quit : ReturnToTitle);
                    break;
            }
            EventSystem.current?.SetSelectedGameObject(buttons[0].gameObject);
        }

        private void StartGame()
        {
            cursorWasLocked = true;
            Resume();
        }

        private void Resume()
        {
            EventSystem.current?.SetSelectedGameObject(null);
            Show(ScreenState.Playing);
        }

        private void OpenFrom(ScreenState next)
        {
            previous = state;
            Show(next);
        }

        private void ToggleDisplay()
        {
            bool full = !Screen.fullScreen;
            Screen.fullScreen = full;
            buttons[0].GetComponentInChildren<Text>().text = full ? "Usar modo ventana" : "Usar pantalla completa";
        }

        private void ReturnToTitle()
        {
            if (loading) return;
            loading = true;
#if UNITY_EDITOR
            UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsyncInPlayMode(
                gameObject.scene.path, new LoadSceneParameters(LoadSceneMode.Single));
#else
            SceneManager.LoadSceneAsync(gameObject.scene.buildIndex, LoadSceneMode.Single);
#endif
        }

        private static void Quit() => Application.Quit();

        private void Option(int index, string text, UnityEngine.Events.UnityAction callback)
        {
            var button = buttons[index];
            options[index] = callback;
            button.GetComponentInChildren<Text>(true).text = text;
            button.gameObject.SetActive(true);
        }

        private static Image ImageAt(Transform parent, string name, Color color)
        {
            var image = new GameObject(name, typeof(RectTransform), typeof(Image)).GetComponent<Image>();
            image.transform.SetParent(parent, false);
            image.color = color;
            return image;
        }

        private static Text Label(Transform parent, string name, int size)
        {
            var label = new GameObject(name, typeof(RectTransform), typeof(Text)).GetComponent<Text>();
            label.transform.SetParent(parent, false);
            label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            label.color = Sand;
            label.fontSize = size;
            label.alignment = TextAnchor.MiddleCenter;
            label.raycastTarget = false;
            return label;
        }

        private static void Place(RectTransform rect, Vector2 anchor, Vector2 position, Vector2 size)
        {
            rect.anchorMin = rect.anchorMax = anchor;
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
        }

        private void OnDestroy()
        {
            back.Dispose();
            if (loading) return;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
