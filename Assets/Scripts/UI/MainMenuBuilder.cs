using UnityEngine;
using UnityEngine.UI;

namespace KarimBusSimulator
{
    public class MainMenuBuilder : MonoBehaviour
    {
        public MainMenuController menuController;
        public bool buildOnStart = true;

        private void Start()
        {
            if (buildOnStart)
            {
                BuildMenu();
            }
        }

        public void BuildMenu()
        {
            var canvasObject = new GameObject("MainMenuCanvas");
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();

            var panel = new GameObject("BackgroundPanel");
            panel.transform.SetParent(canvasObject.transform);
            var rt = panel.AddComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            var image = panel.AddComponent<Image>();
            image.color = new Color(0.05f, 0.12f, 0.22f, 1f);

            const int buttonWidth = 260;
            const int buttonHeight = 58;
            var y = 420;

            var title = CreateText(canvasObject.transform, "TitleText", "KARIM BUS SIMULATOR TANZANIA", new Vector2(0.5f, 0.9f), 28, TextAnchor.MiddleCenter, new Color(0.75f, 0.85f, 1f));
            var subtitle = CreateText(canvasObject.transform, "SubtitleText", "KARIM GAMES STUDIO LIMITED", new Vector2(0.5f, 0.84f), 18, TextAnchor.MiddleCenter, new Color(0.75f, 0.8f, 0.9f));

            AddButton(canvasObject.transform, "PlayButton", "PLAY", new Vector2(0.5f, y), buttonWidth, buttonHeight, () => menuController?.PlayGame());
            y -= 80;
            AddButton(canvasObject.transform, "GarageButton", "GARAGE", new Vector2(0.5f, y), buttonWidth, buttonHeight, () => menuController?.OpenGarage());
            y -= 80;
            AddButton(canvasObject.transform, "RoutesButton", "ROUTES", new Vector2(0.5f, y), buttonWidth, buttonHeight, () => menuController?.OpenRoutes());
            y -= 80;
            AddButton(canvasObject.transform, "CareerButton", "CAREER", new Vector2(0.5f, y), buttonWidth, buttonHeight, () => menuController?.OpenCareer());
            y -= 80;
            AddButton(canvasObject.transform, "SettingsButton", "SETTINGS", new Vector2(0.5f, y), buttonWidth, buttonHeight, () => menuController?.OpenSettings());
            y -= 80;
            AddButton(canvasObject.transform, "QuitButton", "QUIT", new Vector2(0.5f, y), buttonWidth, buttonHeight, () => menuController?.QuitGame());

            if (menuController == null)
            {
                menuController = canvasObject.AddComponent<MainMenuController>();
            }
        }

        private Text CreateText(Transform parent, string name, string text, Vector2 anchor, int fontSize, TextAnchor alignment, Color color)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(anchor.x, anchor.y);
            rect.anchorMax = new Vector2(anchor.x, anchor.y);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(800f, 60f);
            rect.anchoredPosition = Vector2.zero;

            var uiText = go.AddComponent<Text>();
            uiText.text = text;
            uiText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            uiText.fontSize = fontSize;
            uiText.alignment = alignment;
            uiText.color = color;
            return uiText;
        }

        private void AddButton(Transform parent, string name, string label, Vector2 position, int width, int height, UnityEngine.Events.UnityAction callback)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(position.x - 0.5f, position.y - 0.5f);
            rect.anchorMax = new Vector2(position.x + 0.5f, position.y + 0.5f);
            rect.sizeDelta = new Vector2(width, height);
            rect.anchoredPosition = Vector2.zero;

            var image = go.AddComponent<Image>();
            image.color = new Color(0.22f, 0.48f, 0.75f, 1f);

            var button = go.AddComponent<Button>();
            button.onClick.AddListener(callback);

            var textGo = new GameObject("Label");
            textGo.transform.SetParent(go.transform);
            var labelRect = textGo.AddComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;

            var labelText = textGo.AddComponent<Text>();
            labelText.text = label;
            labelText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            labelText.fontSize = 22;
            labelText.alignment = TextAnchor.MiddleCenter;
            labelText.color = Color.white;
        }
    }
}
