using UnityEngine;
using UnityEngine.UI;

namespace KarimBusSimulator
{
    public class SaveLoadMenu : MonoBehaviour
    {
        public void BuildSaveLoadUI()
        {
            var canvasObject = new GameObject("SaveLoadCanvas");
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
            text.text = "SAVE / LOAD";
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 26;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.white;

            var save = new GameObject("SaveButton");
            save.transform.SetParent(canvasObject.transform);
            var saveRect = save.AddComponent<RectTransform>();
            saveRect.anchorMin = new Vector2(0.35f, 0.5f);
            saveRect.anchorMax = new Vector2(0.65f, 0.6f);
            save.AddComponent<Image>();
            var saveText = save.AddComponent<Text>();
            saveText.text = "SAVE";
            saveText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            saveText.alignment = TextAnchor.MiddleCenter;
            saveText.color = Color.white;

            var load = new GameObject("LoadButton");
            load.transform.SetParent(canvasObject.transform);
            var loadRect = load.AddComponent<RectTransform>();
            loadRect.anchorMin = new Vector2(0.35f, 0.3f);
            loadRect.anchorMax = new Vector2(0.65f, 0.4f);
            load.AddComponent<Image>();
            var loadText = load.AddComponent<Text>();
            loadText.text = "LOAD";
            loadText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            loadText.alignment = TextAnchor.MiddleCenter;
            loadText.color = Color.white;
        }
    }
}
