using UnityEngine;
using UnityEngine.UI;

namespace KarimBusSimulator
{
    public class SettingsMenuBuilder : MonoBehaviour
    {
        public GameSettings settings;

        private void Start()
        {
            if (settings == null)
            {
                settings = FindAnyObjectByType<GameSettings>();
            }
        }

        public void BuildSettingsUI()
        {
            var canvasObject = new GameObject("SettingsCanvas");
            canvasObject.AddComponent<Canvas>();
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();

            var title = new GameObject("Title");
            title.transform.SetParent(canvasObject.transform);
            var rect = title.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.95f);
            rect.anchorMax = new Vector2(0.5f, 0.95f);
            rect.sizeDelta = new Vector2(500f, 40f);

            var text = title.AddComponent<Text>();
            text.text = "SETTINGS";
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 26;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.white;

            var qualityLabel = CreateLabel(canvasObject.transform, "Graphics", new Vector2(0.3f, 0.8f));
            var qualityValue = CreateLabel(canvasObject.transform, "GraphicsValue", new Vector2(0.7f, 0.8f));
            qualityValue.text = "HIGH";

            var fpsLabel = CreateLabel(canvasObject.transform, "FPS", new Vector2(0.3f, 0.7f));
            var fpsValue = CreateLabel(canvasObject.transform, "FPSValue", new Vector2(0.7f, 0.7f));
            fpsValue.text = "60";
        }

        private Text CreateLabel(Transform parent, string name, Vector2 anchor)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.sizeDelta = new Vector2(200f, 40f);

            var text = go.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 18;
            text.color = Color.white;
            return text;
        }
    }
}
